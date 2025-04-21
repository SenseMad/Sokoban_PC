using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

using Sokoban.LevelManagement;
using UnityEngine.UIElements;

namespace Sokoban.GridEditor
{
  public class GridEditor : MonoBehaviour
  {
    /// <summary>
    /// True, если включен редактор сетки
    /// </summary>
    public static bool GridEditorEnabled = false;

    [Header("НАСТРОЙКИ")]
    [SerializeField, Tooltip("Размер поля")]
    private Vector3Int _fieldSize;
    [SerializeField, Tooltip("Размер клетки")]
    private int _gridSize = 1;
    [SerializeField, Tooltip("Уровень поля")]
    private int _gridLevel = 0;

    [Header("ОБЪЕКТЫ")]
    [SerializeField, Tooltip("Список типов блочных объектов")]
    private ListBlockObjectTypes _listBlockObjectsTypes;
    [SerializeField, Tooltip("Текущий выбранный тип объекта")]
    private TypeObject _typeSelectedObject = TypeObject.staticObject;
    [SerializeField, Tooltip("Индекс выбранного объекта")]
    private int _indexSelectedObject = 0;

    [Header("НАСТРОЙКИ УРОВНЯ")]
    [SerializeField, Tooltip("Локация для которой создать уровень")]
    private Location _selectedLocation = Location.Chapter_1;
    [SerializeField, Tooltip("Выбранный номер уровня")]
    private int _selectedLevelNumber = 1;

    [Space(15)]
    [SerializeField] private bool _isRandom = false;

    //--------------------------------------

    private Block[,,] blockObjects;

    /// <summary>
    /// True, если сетка скрыта
    /// </summary>
    public bool hideGrid;

    /// <summary>
    /// True, если отображать по одному уровню
    /// </summary>
    public bool DisplayLevel { get; set; }

    /// <summary>
    /// Режим редактирования
    /// </summary>
    public bool EditingMode;
    /// <summary>
    /// Режим перемещения камеры
    /// </summary>
    public bool CameraMovementMode;

    /// <summary>
    /// Режим редактирования
    /// </summary>
    public bool EditMode { get; set; } = true;
    /// <summary>
    /// Режим удаления
    /// </summary>
    public bool DeleteMode { get; set; } = false;

    public LevelData CurrentLevelData { get; set; }

    private BoxCollider boxCollider;

    private Camera mainCamera;

    //======================================

    #region ОБЪЕКТЫ

    /// <summary>
    /// Текущий выбранный тип объекта
    /// </summary>
    public TypeObject TypeSelectedObject { get => _typeSelectedObject; set => _typeSelectedObject = value; }
    /// <summary>
    /// Индекс выбранного объекта
    /// </summary>
    public int IndexSelectedObject { get => _indexSelectedObject; set => _indexSelectedObject = value; }

    #endregion

    #region НАСТРОЙКИ УРОВНЯ

    /// <summary>
    /// Локация для которой создать уровень
    /// </summary>
    public Location SelectedLocation { get => _selectedLocation; set => _selectedLocation = value; }
    /// <summary>
    /// Выбранный номер уровня
    /// </summary>
    public int SelectedLevelNumber { get => _selectedLevelNumber; set => _selectedLevelNumber = value; }

    #endregion

    //======================================

    public Block[,,] GetBlockObjects() => blockObjects;

    public Vector3Int GetFieldSize() => _fieldSize;

    public int GetGridLevel() => _gridLevel;

    public ListBlockObjectTypes GetListBlockObjectsTypes() => _listBlockObjectsTypes;

    //======================================

    private void Awake()
    {
      boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
      mainCamera = Camera.main;

      GridEditorEnabled = true;

      blockObjects = new Block[_fieldSize.x, _fieldSize.y, _fieldSize.z];

      ChangeSizeBoxCollider();
    }

    private void Update()
    {
      if (CameraMovementMode)
        return;

      if (EditMode)
      {
        if (Mouse.current.leftButton.wasPressedThisFrame)  // Mouse.current.leftButton.IsPressed()
        {
          Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
          if (Physics.Raycast(ray, out RaycastHit hit))
          {
            GetXYZ(hit.point, out Vector3Int position);
            RemoveObject(position);
            if (!_isRandom)
              AddObject(position, _listBlockObjectsTypes.GetBlockObject(_typeSelectedObject, _indexSelectedObject));
            else
            {
              int indexRandom = _listBlockObjectsTypes.GetRandomIndexBlockObject(_typeSelectedObject);
              AddObject(position, _listBlockObjectsTypes.GetBlockObject(_typeSelectedObject, indexRandom));
            }
          }
        }
      }

      if (DeleteMode)
      {
        if (Mouse.current.leftButton.wasPressedThisFrame) // Mouse.current.rightButton.IsPressed()
        {
          Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
          if (Physics.Raycast(ray, out RaycastHit hit))
          {
            GetXYZ(hit.point, out Vector3Int position);
            RemoveObject(position);
          }
        }
      }
    }

    //======================================

    #region Добавить/Удалить объект, ПОлучить координаты

    /// <summary>
    /// Добавить объект
    /// </summary>
    private void AddObject(Vector3Int position, Block blockObject)
    {
      Block newBlockObject = Instantiate(blockObject, transform);

      newBlockObject.RemoveRigidbody();
      newBlockObject.transform.position = position;
      Vector3Int newPosition = new(position.x, position.y + 2, position.z);
      newBlockObject.SetPositionObject(newPosition);

      blockObjects[position.x, position.y, position.z] = newBlockObject;
    }

    /// <summary>
    /// Удалить объект
    /// </summary>
    private void RemoveObject(Vector3Int position)
    {
      if (blockObjects[position.x, position.y, position.z] == null)
        return;

      Destroy(blockObjects[position.x, position.y, position.z].gameObject);
      blockObjects[position.x, position.y, position.z] = null;
    }

    /// <summary>
    /// Получить локальные координаты XYZ
    /// </summary>
    /// <param name="worldPosition">Мировая позиция мыши</param>
    /// <param name="vector3Int">Локальная позиция мыши</param>
    private void GetXYZ(Vector3 worldPosition, out Vector3Int vector3Int)
    {
      vector3Int = new Vector3Int
      {
        x = Mathf.RoundToInt((worldPosition.x - transform.position.x) / _gridSize),
        y = _gridLevel,
        z = Mathf.RoundToInt((worldPosition.z - transform.position.z) / _gridSize)
      };

      if (vector3Int.x > _fieldSize.x - 1) vector3Int.x -= 1;
      if (vector3Int.z > _fieldSize.z - 1) vector3Int.z -= 1;
    }

    #endregion

    #region Размер поля

    private void ChangeSizeBoxCollider()
    {
      boxCollider.size = new Vector3(_fieldSize.x, 1, _fieldSize.z) * _gridSize;
      boxCollider.center = (boxCollider.size - new Vector3(_gridSize, _gridSize, _gridSize)) * 0.5f;
      boxCollider.center += new Vector3(0, _gridLevel, 0);
    }

    /// <summary>
    /// Изменить размер поля
    /// </summary>
    public void ChangeFieldSize(Vector3Int fieldSize)
    {
      _fieldSize = fieldSize;

      var tempBlockObjects = blockObjects;
      blockObjects = new Block[_fieldSize.x, _fieldSize.y, _fieldSize.z];

      if (blockObjects.Length > tempBlockObjects.Length)
      {
        for (int i = 0; i < tempBlockObjects.GetLength(0); i++)
        {
          for (int j = 0; j < tempBlockObjects.GetLength(1); j++)
          {
            for (int k = 0; k < tempBlockObjects.GetLength(2); k++)
            {
              if (tempBlockObjects[i, j, k] == null)
                continue;

              blockObjects[i, j, k] = tempBlockObjects[i, j, k];
            }
          }
        }
      }
      else
      {
        for (int i = 0; i < blockObjects.GetLength(0); i++)
        {
          for (int j = 0; j < blockObjects.GetLength(1); j++)
          {
            for (int k = 0; k < blockObjects.GetLength(2); k++)
            {
              if (tempBlockObjects[i, j, k] == null)
                continue;

              blockObjects[i, j, k] = tempBlockObjects[i, j, k];
              tempBlockObjects[i, j, k] = null;
            }
          }
        }

        // Очистить оставшиеся блоки за границей уровня
        for (int i = 0; i < tempBlockObjects.GetLength(0); i++)
        {
          for (int j = 0; j < tempBlockObjects.GetLength(1); j++)
          {
            for (int k = 0; k < tempBlockObjects.GetLength(2); k++)
            {
              if (tempBlockObjects[i, j, k] == null)
                continue;

              Destroy(tempBlockObjects[i, j, k].gameObject);
            }
          }
        }
      }

      if (_gridLevel + 1 > _fieldSize.y)
        _gridLevel = _fieldSize.y - 1;

      ChangeSizeBoxCollider();
    }

    #endregion

    #region Уровень поля

    /// <summary>
    /// Изменить уровень поля
    /// </summary>
    public void ChangeGridLevel(bool parValue)
    {
      //ShowHideSublevels(false);

      if (parValue && _gridLevel + 1 <= _fieldSize.y - 1)
        _gridLevel++;
      else if (!parValue && _gridLevel - 1 >= 0)
        _gridLevel--;

      //ShowHideSublevels(true);

      ChangeSizeBoxCollider();
    }

    /// <summary>
    /// Отобразить/Скрыть подуровень
    /// </summary>
    public void ShowHideSublevels(bool parValue)
    {
      if (!DisplayLevel)
        return;

      for (int i = 0; i < blockObjects.GetLength(0); i++)
      {
        for (int k = 0; k < blockObjects.GetLength(2); k++)
        {
          if (blockObjects[i, _gridLevel, k] != null)
            blockObjects[i, _gridLevel, k].gameObject.SetActive(parValue);
        }
      }
    }

    /// <summary>
    /// Отобразить/Скрыть все подуровни
    /// </summary>
    public void ShowHideAllSublevels(bool parValue)
    {
      for (int i = 0; i < blockObjects.GetLength(0); i++)
      {
        for (int j = 0; j < blockObjects.GetLength(1); j++)
        {
          for (int k = 0; k < blockObjects.GetLength(2); k++)
          {
            if (blockObjects[i, j, k] != null)
            {
              if (parValue)
              {
                blockObjects[i, j, k].gameObject.SetActive(true);
              }
              else if (!parValue && j != _gridLevel)
              {
                blockObjects[i, j, k].gameObject.SetActive(false);
              }
            }
          }
        }
      }
    }

    #endregion

    //======================================

    #region Создание/Сохранение/Загрузка уровней

    /// <summary>
    /// Создание данных уровня
    /// </summary>
    public void CreateLevelData()
    {
      LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

      int numLevel = 1;

      while (Levels.GetLevelData(_selectedLocation, numLevel) != null)
      {
        numLevel++;
      }

      levelData.name = $"{_selectedLocation}_{numLevel}";
      levelData.LevelNumber = numLevel;
      levelData.Location = _selectedLocation;
      levelData.FieldSize = _fieldSize;

      levelData.ListLevelObjects = new List<LevelData.GridData>();

      foreach (var blockObject in blockObjects)
      {
        if (blockObject == null)
          continue;

        levelData.ListLevelObjects.Add(new LevelData.GridData()
        {
          TypeObject = blockObject.GetTypeObject(),
          IndexObject = blockObject.GetIndexObject(),
          PositionObject = blockObject.GetObjectPosition()
        });
      }

#if UNITY_EDITOR
      string path = Levels.GetPathToStorageLevels(SelectedLocation, numLevel);
      AssetDatabase.CreateAsset(levelData, $"Assets/Resources/{path}.asset");
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
#endif

      Levels.GetFullNumberLevelsLocation();
    }

    /// <summary>
    /// Загрузить данные уровня
    /// </summary>
    public void LoadLevelData()
    {
      LevelData levelData = Levels.GetLevelData(_selectedLocation, _selectedLevelNumber);

      if (levelData == null)
        return;

      CurrentLevelData = levelData;
      _fieldSize = levelData.FieldSize;

      ClearLevelObjects();

      blockObjects = new Block[levelData.FieldSize.x, levelData.FieldSize.y, levelData.FieldSize.z];

      foreach (var levelObject in levelData.ListLevelObjects)
      {
        Block newBlockObject = Instantiate(_listBlockObjectsTypes.GetBlockObject(levelObject.TypeObject, levelObject.IndexObject), transform);
        newBlockObject.transform.position = levelObject.PositionObject;
        Vector3Int position = new(levelObject.PositionObject.x, levelObject.PositionObject.y + 2, levelObject.PositionObject.z);
        newBlockObject.SetPositionObject(position);
        blockObjects[levelObject.PositionObject.x, levelObject.PositionObject.y, levelObject.PositionObject.z] = newBlockObject;
      }
    }

    /// <summary>
    /// Очистить объекты уровня
    /// </summary>
    public void ClearLevelObjects()
    {
      if (blockObjects == null)
        return;

      foreach (var blockObject in blockObjects)
      {
        if (blockObject == null)
          continue;

        Destroy(blockObject.gameObject);
      }

      blockObjects = new Block[_fieldSize.x, _fieldSize.y, _fieldSize.z];
    }

    /// <summary>
    /// Сохранить данные уровня
    /// </summary>
    public void SaveLevelData()
    {
      if (CurrentLevelData == null)
        return;

      CurrentLevelData.FieldSize = _fieldSize;
      CurrentLevelData.ListLevelObjects = new List<LevelData.GridData>();

      foreach (var blockObject in blockObjects)
      {
        if (blockObject == null)
          continue;

        CurrentLevelData.ListLevelObjects.Add(new LevelData.GridData()
        {
          TypeObject = blockObject.GetTypeObject(),
          IndexObject = blockObject.GetIndexObject(),
          PositionObject = blockObject.GetObjectPosition()
        });
      }

#if UNITY_EDITOR
      EditorUtility.SetDirty(CurrentLevelData);
#endif
    }

#endregion

    //======================================

    private void OnDrawGizmos()
    {
      if (hideGrid)
        return;

      Gizmos.color = Color.white;

      for (int x = 0; x < _fieldSize.x; x++)
      {
        for (int y = 0; y < _fieldSize.z; y++)
        {
          Gizmos.DrawWireCube(transform.position + new Vector3(x * _gridSize, _gridLevel, y * _gridSize), new Vector3(_gridSize, _gridSize, _gridSize));
        }
      }
    }

    //======================================
  }
}