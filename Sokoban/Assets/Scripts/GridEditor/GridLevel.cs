using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sokoban.GameManagement;
using Sokoban.LevelManagement;

namespace Sokoban.GridEditor
{
  public class GridLevel : MonoBehaviour
  {
    [SerializeField, Tooltip("Список типов блочных объектов")]
    private ListBlockObjectTypes _listBlockObjectTypes;

    [SerializeField] private AnimationCurve animationCurve;

    //--------------------------------------

    private StatesLevel statesLevel;

    private Coroutine myCoroutine;

    //======================================

    private GameManager gameManager;

    private LevelManager levelManager;

    private RandomGrassSpawn randomGrassSpawn;

    private Block[,,] blockObjects;

    private List<FoodObject> listFoodObjects = new();

    private List<DoorObject> listDoorObjects = new();

    public List<Vector3Int> listEmptyCells { get; set; } = new();

    public List<Vector3Int> listAllAvailableCells { get; set; } = new();

    //======================================

    public Block[,,] BlockObjects { get => blockObjects; set => blockObjects = value; }

    public bool IsLevelCreated { get; private set; }

    public bool IsLevelDeleted { get; private set; } = true;

    public ListBlockObjectTypes ListBlockObjectTypes { get => _listBlockObjectTypes; }

    //======================================

    public UnityEvent OnLevelCreated { get; } = new UnityEvent();

    public UnityEvent OnLevelDeleted { get; } = new UnityEvent();

    //======================================

    private void Awake()
    {
      randomGrassSpawn = GetComponent<RandomGrassSpawn>();
    }

    private void Start()
    {
      gameManager = GameManager.Instance;

      levelManager = LevelManager.Instance;

      transform.localPosition = new Vector3(0, -2, 0);
    }

    //======================================

    #region Поиск еды на уровне

    private void FindAllFoodObjects()
    {
      if (blockObjects == null)
        return;

      listFoodObjects = new List<FoodObject>();

      foreach (var blockObject in blockObjects)
      {
        if (blockObject == null)
          continue;

        if (!blockObject.TryGetComponent(out FoodObject foodObject))
          continue;

        listFoodObjects.Add(foodObject);
      }
    }

    public List<FoodObject> GetListFoodObjects()
    {
      return listFoodObjects;
    }

    #endregion

    #region Поиск объектов дверей на уровне

    private void FindAllDoorObjects()
    {
      if (blockObjects == null)
        return;

      listDoorObjects = new List<DoorObject>();

      foreach (var blockObject in blockObjects)
      {
        if (blockObject == null)
          continue;

        if (!blockObject.TryGetComponent(out DoorObject doorObject))
          continue;

        listDoorObjects.Add(doorObject);
      }
    }

    public List<DoorObject> GetListDoorObjects()
    {
      return listDoorObjects;
    }

    #endregion

    //======================================

    public void CreatingLevelGrid()
    {
      DeletingLevelObjects();

      StartCoroutine(CreateLevel());
    }

    public void DeletingLevelObjects()
    {
      if (blockObjects == null)
        return;

      myCoroutine = StartCoroutine(DeleteLevel());
    }

    public void SkinReplace()
    {
      foreach (var block in blockObjects)
      {
        if (block == null)
          continue;

        if (!block.GetComponent<PlayerObjects>())
          continue;

        foreach (var skinData in ShopData.Instance.SkinDatas)
        {
          if (skinData.IndexSkin != gameManager.ProgressData.CurrentActiveIndexSkin)
            continue;

          block.GetComponentInChildren<MeshFilter>().sharedMesh = skinData.ObjectSkin.GetComponentInChildren<MeshFilter>().sharedMesh;
          block.GetComponentInChildren<MeshRenderer>().sharedMaterial = skinData.ObjectSkin.GetComponentInChildren<Renderer>().sharedMaterial;
          break;
        }

        break;
      }
    }

    private void SpawnGrass()
    {
      List<Vector3Int> emptyPositionsList = new();
      List<Vector3Int> ignorePositonsList = new();

      foreach (var block in blockObjects)
      {
        if (block == null)
          continue;

        if (!block.GetComponent<GroundObject>())
          continue;

        if (block.transform.position.y == 3)
          break;

        Vector3Int positionUpCurrentBlock = new((int)block.transform.position.x, (int)block.transform.position.y + 1, (int)block.transform.position.z);
        emptyPositionsList.Add(positionUpCurrentBlock);

        foreach (var blockUp in blockObjects)
        {
          if (blockUp == null)
            continue;

          if (blockUp.transform.position.y != 3)
            continue;

          if (blockUp.transform.position == positionUpCurrentBlock)
          {
            ignorePositonsList.Add(positionUpCurrentBlock);
            break;
          }
        }
      }

      List<Vector3Int> positionsList = new();
      for (int i = 0; i < emptyPositionsList.Count; i++)
      {
        if (ignorePositonsList.Contains(emptyPositionsList[i]))
          continue;

        positionsList.Add(emptyPositionsList[i]);
      }

      Vector3Int[] tempArray = new Vector3Int[positionsList.Count];
      for (int i = 0; i < tempArray.Length; i++)
        tempArray[i] = positionsList[i];

      Vector3Int[] placeSpawn = randomGrassSpawn.CheckSpawn(tempArray);
      for (int i = 0; i < placeSpawn.Length; i++)
      {
        Block newBlockObject = Instantiate(randomGrassSpawn.GetRandomTypeBlockObjects(), transform);

        Vector3Int posPlaceSpawn = placeSpawn[i];
        newBlockObject.transform.position = posPlaceSpawn;
        newBlockObject.SetPositionObject(posPlaceSpawn);
        blockObjects[posPlaceSpawn.x, posPlaceSpawn.y, posPlaceSpawn.z] = newBlockObject;
      }
    }

    private IEnumerator CreateLevel()
    {
      while (myCoroutine != null || !IsLevelDeleted)
      {
        yield return null;
      }

      IsLevelDeleted = false;
      statesLevel = StatesLevel.Created;
      //transform.position = new Vector3(0, -3, 0);

      LevelData levelData = levelManager.GetCurrentLevelData();
      blockObjects = new Block[levelData.FieldSize.x, levelData.FieldSize.y, levelData.FieldSize.z];

      foreach (var levelObject in levelManager.GetCurrentLevelData().ListLevelObjects)
      {
        //Block newBlockObject = Instantiate(_listBlockObjectTypes.GetBlockObject(levelObject.TypeObject, levelObject.IndexObject), transform);
        var blockPrefab = _listBlockObjectTypes.GetBlockObject(levelObject.TypeObject, levelObject.IndexObject);

        if (blockPrefab == null)
        {
          Debug.LogError($"Префаб не найден! TypeObject: {levelObject.TypeObject}, IndexObject: {levelObject.IndexObject}");
          continue; // или yield break / throw, если нужно остановить процесс
        }

        Block newBlockObject = Instantiate(blockPrefab, transform);

        #region Select Skin

        var skinDatas = ShopData.Instance;

        if (skinDatas != null)
        {
          if (newBlockObject.GetComponent<PlayerObjects>() != null)
          {
            foreach (var skinData in skinDatas.SkinDatas)
            {
              if (skinData.IndexSkin != gameManager.ProgressData.CurrentActiveIndexSkin)
                continue;

              newBlockObject.GetComponentInChildren<MeshFilter>().sharedMesh = skinData.ObjectSkin.GetComponentInChildren<MeshFilter>().sharedMesh;
              newBlockObject.GetComponentInChildren<MeshRenderer>().sharedMaterial = skinData.ObjectSkin.GetComponentInChildren<Renderer>().sharedMaterial;
              break;
            }
          }
        }

        #endregion

        if (!newBlockObject.GetComponent<GroundObject>())
          newBlockObject.transform.localScale = Vector3.zero;
        /*if (newBlockObject.GetTypeObject() != TypeObject.staticObject)
          newBlockObject.transform.localScale = Vector3.zero;*/

        newBlockObject.transform.position = levelObject.PositionObject;
        newBlockObject.SetPositionObject(levelObject.PositionObject);
        blockObjects[levelObject.PositionObject.x, levelObject.PositionObject.y, levelObject.PositionObject.z] = newBlockObject;
      }

      #region The appearance of the platform (Ground)

      float timer = 0f;

      while (timer < 1)
      {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / 1);

        transform.localPosition = new Vector3(0, -2, 0) * animationCurve.Evaluate(1 - t);

        yield return null;
      }

      #endregion

      foreach (var block in blockObjects)
      {
        if (block == null)
          continue;

        block.BoxCollider.enabled = true;

        /*if (block.TryGetComponent(out DynamicObjects dynamicObjects))
          dynamicObjects.rigidbody.useGravity = true;*/

        /*if (block.TryGetComponent(out PlayerObjects playerObjects))
          playerObjects.rigidbody.useGravity = true;*/
      }

      #region Random Grass

      SpawnGrass();

      IdentifyEmptyCells();

      #endregion

      foreach (var block in blockObjects)
      {
        if (block == null)
          continue;

        if (block.TryGetComponent(out DecoreObject decoreObject))
        {
          if (!decoreObject.IsEnableBoxCollider)
            block.BoxCollider.enabled = false;
        }
      }

      #region Animation of the appearance of all block except Ground

      timer = 0f;

      while (timer < 1)
      {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / 1);

        foreach (var block in blockObjects)
        {
          if (block == null)
            continue;

          if (block.GetComponent<GroundObject>())
            continue;
          /*if (block.GetTypeObject() == TypeObject.staticObject)
            continue;*/

          block.transform.localScale = Vector3.one * animationCurve.Evaluate(t);
        }

        yield return null;
      }

      #endregion

      FindAllFoodObjects();
      FindAllDoorObjects();
      statesLevel = StatesLevel.Completed;
      OnLevelCreated?.Invoke();

      IsLevelCreated = true;
      IsLevelDeleted = true;
    }

    private IEnumerator DeleteLevel()
    {
      if (blockObjects == null)
      {
        myCoroutine = null;
        yield break;
      }

      IsLevelCreated = false;
      IsLevelDeleted = false;
      statesLevel = StatesLevel.Deleted;

      float timer = 0f;

      #region Animation of the appearance of all block except Ground

      while (timer < 1)
      {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / 1);

        foreach (var block in blockObjects)
        {
          if (block == null)
            continue;

          if (block.GetComponent<GroundObject>())
            continue;
          /*if (block.GetTypeObject() == TypeObject.staticObject)
            continue;*/

          if (block.GetTypeObject() == TypeObject.dynamicObject || block.GetTypeObject() == TypeObject.playerObject)
            block.RemoveRigidbody();

          block.transform.localScale = Vector3.one * animationCurve.Evaluate(1 - t);
        }

        yield return null;
      }

      #endregion

      #region Platform Immersion (Ground)

      timer = 0;

      while (timer < 1)
      {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / 1);

        transform.localPosition = new Vector3(0, -2, 0) * animationCurve.Evaluate(t);
        yield return null;
      }

      #endregion

      foreach (var block in blockObjects)
      {
        if (block == null)
          continue;

        Destroy(block.gameObject);
      }

      myCoroutine = null;
      blockObjects = null;

      IsLevelDeleted = true;
    }

    public bool TryStatesLevel()
    {
      return statesLevel == StatesLevel.Created || statesLevel == StatesLevel.Deleted;
    }

    //======================================

    public void IdentifyEmptyCells()
    {
      LevelData levelData = levelManager.GetCurrentLevelData();

      listEmptyCells = new();
      listAllAvailableCells = new();

      List<Vector3Int> listIgnore = new();

      foreach (var block in blockObjects)
      {
        if (block == null)
          continue;

        Vector3Int positionBlock = new((int)block.transform.position.x, (int)block.transform.position.y, (int)block.transform.position.z);
        listIgnore.Add(positionBlock);
      }

      for (int x = 0; x < levelData.FieldSize.x; x++)
      {
        for (int z = 0; z < levelData.FieldSize.z; z++)
        {
          Vector3Int position = new(x, 2, z);

          listAllAvailableCells.Add(position);

          if (listIgnore.Contains(position))
            continue;

          listEmptyCells.Add(position);
          //Instantiate(_listBlockObjectTypes.GetBlockObject(TypeObject.staticObject, 1), new Vector3Int(x, 2, z), Quaternion.identity);
        }
      }
    }

    //======================================

    public enum StatesLevel
    {
      Completed,
      Created,
      Deleted
    }

    //======================================
  }
}