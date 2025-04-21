using System;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

using Sokoban.GameManagement;
using Sokoban.GridEditor;
using Sokoban.UI;

namespace Sokoban.LevelManagement
{
  public sealed class LevelManager : SingletonInSceneNoInstance<LevelManager>
  {
    [SerializeField] private LevelData _currentLevelData;
    [SerializeField] private LevelProgressData _currentLevelProgressData;

    [Space(10)]
    [SerializeField] private GridLevel _gridLevel;

    [Space(10)]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _levelCompleteMenu;
    [SerializeField] private GameObject _levelFailedMenu;
    [SerializeField] private Panel _menuPanel;

    [Space(10)]
    [SerializeField] private TypeDifficultyGame _currentTypeDifficultyGame;

    //--------------------------------------

    private GameManager gameManager;

    private CinemachineCamera cinemachineVirtual;

    private bool isCameraRotation;

    private AudioManager audioManager;

    private LevelSounds levelSounds;

    private int tempAmountFoodCollected = 0;

    private DifficultyGame difficultyGame;

    //======================================

    public bool LevelCompleted { get; set; }

    public bool LevelFailed { get; set; }

    public bool IsPause { get; set; }

    public bool IsLevelMenu { get; private set; }
    
    public GridLevel GridLevel => _gridLevel;

    public CinemachineCamera CinemachineVirtual
    {
      get => cinemachineVirtual;
      set => cinemachineVirtual = value;
    }

    public bool IsLevelRunning { get; set; } = false;

    //======================================

    public UnityEvent<float> ChangeTimeOnLevel { get; } = new UnityEvent<float>();

    public UnityEvent<int> ChangeNumberMoves { get; } = new UnityEvent<int>();

    public event Action ChangeDifficultyGame;

    public UnityEvent<bool> OnPauseEvent { get; } = new UnityEvent<bool>();

    public event Action OnLevelCompleted;

    public UnityEvent IsNextLevel { get; } = new UnityEvent();

    public UnityEvent<LevelData> IsNextLevelData { get; } = new UnityEvent<LevelData>();

    public UnityEvent IsReloadLevel { get; } = new UnityEvent();

    public UnityEvent IsMenu { get; } = new UnityEvent();

    //======================================

    public LevelData GetCurrentLevelData() => _currentLevelData;

    public LevelProgressData GetCurrentLevelProgressData() => _currentLevelProgressData;

    //======================================

    public float TimeOnLevel
    {
      get => _currentLevelProgressData.TimeOnLevel;
      private set
      {
        _currentLevelProgressData.TimeOnLevel = value;
        ChangeTimeOnLevel?.Invoke(value);
      }
    }

    public int NumberMoves
    {
      get => _currentLevelProgressData.NumberMoves;
      set
      {
        _currentLevelProgressData.NumberMoves = value;
        ChangeNumberMoves?.Invoke(value);
      }
    }

    public TypeDifficultyGame CurrentTypeDifficultyGame { get => _currentTypeDifficultyGame; set => _currentTypeDifficultyGame = value; }

    //======================================

    private new void Awake()
    {
      gameManager = GameManager.Instance;

      cinemachineVirtual = FindAnyObjectByType<CinemachineCamera>();

      audioManager = AudioManager.Instance;

      levelSounds = GetComponent<LevelSounds>();

      difficultyGame = new DifficultyGame();
    }

    private void Start()
    {
      MenuLevel();
      //_currentLevelData = Levels.CurrentSelectedLevelData;

      //ReloadLevel();
    }

    private void LateUpdate()
    {
      CameraRotationLoop();

      ResetCameraRotation();

      if (_currentLevelData == null)
        return;

      if (IsPause)
        return;

      if (LevelCompleted)
        return;

      if (!_gridLevel.IsLevelCreated)
        return;
      
      TimeOnLevel += Time.deltaTime;

      /*if (_currentTypeDifficultyGame == TypeDifficultyGame.Easy)
        return;

      difficultyGame.IsLevelFailedTime();*/
    }

    private void OnEnable()
    {
      OnLevelCompleted += LevelManager_OnLevelCompleted;
      difficultyGame.OnLevelFailed += DifficultyGame_OnLevelFailed;
    }

    private void OnDisable()
    {
      OnLevelCompleted -= LevelManager_OnLevelCompleted;
      difficultyGame.OnLevelFailed -= DifficultyGame_OnLevelFailed;
    }

    //======================================

    private void CameraRotationLoop()
    {
      if (!IsLevelMenu)
        return;

      float yRotation = cinemachineVirtual.transform.rotation.eulerAngles.y + Time.deltaTime * 5f;
      Quaternion rotation = Quaternion.Euler(48f, yRotation, 0f);
      cinemachineVirtual.transform.rotation = rotation;
    }

    private void ResetCameraRotation()
    {
      if (!isCameraRotation)
        return;

      Quaternion currentRotation = cinemachineVirtual.transform.rotation;
      Quaternion targetQuaternion = Quaternion.Euler(48.0f, 0.0f, 0.0f);
      Quaternion newRotation = Quaternion.Slerp(currentRotation, targetQuaternion, 3f * Time.deltaTime);

      cinemachineVirtual.transform.rotation = newRotation;

      if (Quaternion.Angle(currentRotation, targetQuaternion) < 0.01f)
      {
        isCameraRotation = false;
        cinemachineVirtual.transform.rotation = targetQuaternion;
      }
    }

    private void IsLevelComplete()
    {
      if (LevelCompleted)
        return;

      OnLevelCompleted?.Invoke();
    }

    public bool IsFoodCollected()
    {
      var foodObjects = _gridLevel.GetListFoodObjects();
      tempAmountFoodCollected++;
      for (int i = 0; i < foodObjects.Count; i++)
      {
        if (!foodObjects[i].IsFoodCollected)
          return false;
      }

      IsLevelComplete();
      return true;
    }

    public void SetPause(bool parValue)
    {
      IsPause = parValue;
      OnPauseEvent?.Invoke(parValue);
    }

    //======================================

    private void LevelManager_OnLevelCompleted()
    {
      LevelCompleted = true;
      /*LevelFailed = false;

      if (LevelFailed)
      {
        tempAmountFoodCollected = 0;
        return;
      }*/

      GameManager.Instance.ProgressData.AmountFoodCollected += tempAmountFoodCollected;
      GameManager.Instance.ProgressData.TotalFoodCollected += tempAmountFoodCollected;
      tempAmountFoodCollected = 0;

      audioManager.OnPlaySound?.Invoke(levelSounds.LevelComplete);

      OpenNextLevel();
    }

    private void DifficultyGame_OnLevelFailed()
    {
      LevelFailed = true;

      //OnLevelCompleted?.Invoke();
    }

    //======================================

    public string UpdateTextTimeLevel()
    {
      int hours = Mathf.FloorToInt(TimeOnLevel / 3600f);
      int minutes = Mathf.FloorToInt((TimeOnLevel % 3600f) / 60f);
      int seconds = Mathf.FloorToInt(TimeOnLevel % 60f);

      if (hours > 0)
        return $"{hours:00}:{minutes:00}:{seconds:00}";
      else if (minutes > 0)
        return $"{minutes:00}:{seconds:00}";
      else
        return $"{minutes:00}:{seconds:00}";
    }

    /// <summary>
    /// Загрузить новый уровень
    /// </summary>
    public void UploadNewLevel()
    {
      IsNextLevel?.Invoke();

      var levelData = Levels.GetNextLevelData(_currentLevelData.Location, _currentLevelData.LevelNumber);
      if (levelData != null)
        _currentLevelData = levelData;

      IsNextLevelData?.Invoke(_currentLevelData);

      isCameraRotation = true;

      ReloadLevel(_currentLevelData);
    }

    public void ReloadLevel()
    {
      isCameraRotation = true;
      IsReloadLevel?.Invoke();
      _gridLevel.CreatingLevelGrid();

      TimeOnLevel = 0;
      NumberMoves = 0;
      LevelCompleted = false;

      tempAmountFoodCollected = 0;
    }

    public void ReloadLevel(LevelData levelData)
    {
      _currentLevelData = levelData;

      IsNextLevelData?.Invoke(_currentLevelData);

      isCameraRotation = true;

      IsReloadLevel?.Invoke();
      _gridLevel.CreatingLevelGrid();

      TimeOnLevel = 0;
      NumberMoves = 0;
      LevelCompleted = false;
      IsLevelMenu = false;

      _pauseMenu.SetActive(true);
      _levelCompleteMenu.SetActive(true);

      tempAmountFoodCollected = 0;
    }

    public void MenuLevel()
    {
      ProgressData progress = gameManager.ProgressData;

      LevelData oldLevelData = _currentLevelData;
      _currentLevelData = Levels.GetLevelData(progress.LocationLastLevelPlayed, progress.IndexLastLevelPlayed + 1);
      _currentLevelData ??= oldLevelData;

      _gridLevel.CreatingLevelGrid();
      LevelCompleted = true;
      IsLevelMenu = true;
    }

    public void SkinReplace()
    {
      _gridLevel.SkinReplace();
    }

    public void ExitMenu()
    {
      //_gridLevel.DeletingLevelObjects();
      _gridLevel.CreatingLevelGrid();
      IsLevelMenu = true;

      _pauseMenu.SetActive(false);
      _levelCompleteMenu.SetActive(false);

      //var targetObject = new GameObject("targetObject");
      //targetObject.transform.position = new Vector3(3, 2, 6f);
      //cinemachineVirtual.Follow = targetObject.transform;
      isCameraRotation = true;

      IsMenu?.Invoke();

      PanelController.Instance.CloseAllPanels();
      PanelController.Instance.SetActivePanel(_menuPanel);

      IsLevelRunning = false;

      //Destroy(targetObject, 5f);
    }

    private void OpenNextLevel()
    {
      gameManager.ProgressData.SaveProgressLevelData(_currentLevelProgressData, _currentLevelData.Location, _currentLevelData.LevelNumber);

      gameManager.ProgressData.OpenNextLevel(_currentLevelData.Location, _currentLevelData.LevelNumber);

      if (gameManager.Achievements != null)
      {
        gameManager.Achievements.UpdateAchivementLevels();
        gameManager.Achievements.UpdateAchivementChapter();
      }

      gameManager.SaveData();
    }

    //======================================
  }
}