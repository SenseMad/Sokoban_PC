using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Sokoban.LevelManagement;
using Sokoban.Achievement;
using Sokoban.Save;
using System;
using System.Linq;

namespace Sokoban.GameManagement
{
  public sealed class GameManager : SingletonInGame<GameManager>
  {
    private TransitionBetweenScenes transitionBetweenScenes;

    //======================================

    public PlatformManager PlatformManager { get; private set; }

    public Achievements Achievements { get; private set; }

    public SaveLoadManager SaveLoadManager { get; private set; }

    public ProgressData ProgressData { get; set; }

    public SettingsData SettingsData { get; set; }

    //======================================

    protected override void Awake()
    {
      base.Awake();

      transitionBetweenScenes = FindAnyObjectByType<TransitionBetweenScenes>();

      StartCoroutine(Init());
    }

    private void Start()
    {
      Levels.GetFullNumberLevelsLocation();

      SettingsData.CreateResolutions();

      SettingsData.ApplyResolution();

      /*#if UNITY_PS4
            Screen.fullScreen = true;
      #else
            SettingsData.CreateResolutions();

            SettingsData.ApplyResolution();
      #endif*/
    }

    private void OnApplicationQuit()
    {
      SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
      if (pause)
        SaveData();
      else
        LoadData();
    }

    //======================================

    private IEnumerator Init()
    {
      bool initScene = SceneManager.GetActiveScene().name == "InitScene";

      SaveLoadManager = new();
      ProgressData = new();
      SettingsData = new();

      SettingsData.CurrentLanguage = Language.English;

      LoadData();

      yield return new WaitForSeconds(3.0f);

      if (initScene)
        transitionBetweenScenes.StartSceneChange("GameScene");
    }


    /*private IEnumerator Init()
    {
      bool initScene = SceneManager.GetActiveScene().name == "InitScene";

      SaveLoadManager = new SaveLoadManager();

      ProgressData = new();
      SettingsData = new();

      SettingsData.CurrentLanguage = Language.English;

      PlatformManager = new PlatformManager();

      if (PlatformManager.Initialize())
      {
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => PlatformManager.Main.IsInitialized);
        yield return new WaitUntil(() => PlatformManager.LocalUserProfiles.IsInitialized);
        yield return new WaitUntil(() => PlatformManager.Achievements.IsInitialized);
        yield return new WaitUntil(() => 
        {
          var progress = PlatformManager.Achievements.GetAchievementsProgress(PlatformManager.LocalUserProfiles.GetPrimaryLocalUserId());
          return progress.Count() > 0;
        });

        InstallingSystemLanguage();

        yield return new WaitUntil(() => PlatformManager.SaveLoad.IsInitialized);
        LoadData();

        Achievements = Achievements.Instance;
      }
      if (initScene)
      {
        transitionBetweenScenes.StartSceneChange("GameScene");
      }
    }*/

    private void InstallingSystemLanguage()
    {
      /*switch (PlatformManager.Main.SystemLanguage)
      {
        case Alekrus.UnivarsalPlatform.SystemLanguage.JAPANESE:
          SettingsData.CurrentLanguage = Language.Japan;
          break;

        case Alekrus.UnivarsalPlatform.SystemLanguage.FRENCH:
        case Alekrus.UnivarsalPlatform.SystemLanguage.FRENCH_CA:
          SettingsData.CurrentLanguage = Language.French;
          break;

        case Alekrus.UnivarsalPlatform.SystemLanguage.SPANISH:
        case Alekrus.UnivarsalPlatform.SystemLanguage.SPANISH_LA:
          SettingsData.CurrentLanguage = Language.Spanish;
          break;

        case Alekrus.UnivarsalPlatform.SystemLanguage.GERMAN:
          SettingsData.CurrentLanguage = Language.German;
          break;

        case Alekrus.UnivarsalPlatform.SystemLanguage.RUSSIAN:
          SettingsData.CurrentLanguage = Language.Russian;
          break;

        case Alekrus.UnivarsalPlatform.SystemLanguage.CHINESE_T:
        case Alekrus.UnivarsalPlatform.SystemLanguage.CHINESE_S:
          SettingsData.CurrentLanguage = Language.Chinese;
          break;

        case Alekrus.UnivarsalPlatform.SystemLanguage.PORTUGUESE_PT:
        case Alekrus.UnivarsalPlatform.SystemLanguage.PORTUGUESE_BR:
          SettingsData.CurrentLanguage = Language.Portuguese;
          break;

        default:
          SettingsData.CurrentLanguage = Language.English;
          break;
      }*/
    }

    //======================================

    public void SaveData()
    {
      SaveLoadManager.SaveData();
    }

    public void ResetAndSaveFile()
    {
      SaveLoadManager.ResetAndSaveFile();
    }

    private void LoadData()
    {
      SaveLoadManager.LoadData();
    }

    //======================================
  }
}