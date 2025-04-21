using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using Sokoban.GameManagement;
using System;

namespace Sokoban.Save
{
  public sealed class SaveLoadManager
  {
    public static string PATH = $"{Application.persistentDataPath}/SaveData.dat";

    //======================================

    public void SaveData()
    {
      GameData data = GameData();

      FileStream fileStream = new FileStream(PATH, FileMode.Create);
      BinaryFormatter binaryFormatter = new BinaryFormatter();

      binaryFormatter.Serialize(fileStream, data);
      fileStream.Close();

      /*Debug.Log($"[{typeof(SaveLoadManager)}] SaveData()");

      try
      {
        if (SaveLoad == null)
        {
          Debug.LogError($"[{typeof(SaveLoadManager)}] SaveData : saveLoad = null!");
          return;
        }

        GameData data = GameData();

        MemoryStream memoryStream = new();
        BinaryFormatter binaryFormatter = new();
        binaryFormatter.Serialize(memoryStream, data);

        GameSlotDetails gameSlotDetails = new()
        {
          Title = "FLINE SaveData",
          SubTitle = "Your game progress",
          Detail = "This is where your suffering is stored!"
        };

        SaveLoad.Save(LocalUser, memoryStream.GetBuffer(), gameSlotDetails);
      }
      catch(Exception e)
      {
        Debug.LogError($"[{typeof(SaveLoadManager)}] Error SaveData : {e.Message}");
      }*/
    }

    public void LoadData()
    {
      //Debug.Log($"[{typeof(SaveLoadManager)}] LoadData()");

      if (File.Exists(PATH))
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(PATH, FileMode.Open);
        GameData data = (GameData)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();

        SetGameData(data);
      }

      /*if (SaveLoad == null)
      {
        Debug.LogError($"[{typeof(SaveLoadManager)}] LoadData : saveLoad = null!");
        if (File.Exists(PATH))
        {
          BinaryFormatter binaryFormatter = new();
          FileStream fileStream = new(PATH, FileMode.Open);
          GameData data = (GameData)binaryFormatter.Deserialize(fileStream);
          fileStream.Close();

          SetGameData(data);
        }
        return;
      }*/

      /*SaveLoad.GameLoaded += UserSaveLoad_GameLoaded;

      SaveLoad.Load(LocalUser);*/
    }

    /*private void UserSaveLoad_GameLoaded(LoadedReceivedArgs parArgs)
    {
      SaveLoad.GameLoaded -= UserSaveLoad_GameLoaded;

      try
      {
        if (parArgs.Data == null)
        {
          Debug.LogError($"[{typeof(SaveLoadManager)}] UserSaveLoad_GameLoaded : Load Data = null!");
          return;
        }
        Debug.Log($"[{typeof(SaveLoadManager)}] Load Data = {parArgs.Data}");

        MemoryStream input = new(parArgs.Data);
        BinaryFormatter binaryFormatter = new();
        GameData data = (GameData)binaryFormatter.Deserialize(input);

        SetGameData(data);
      }
      catch (Exception e)
      {
        Debug.LogError($"[{typeof(SaveLoadManager)}] Error UserSaveLoad_GameLoaded : {e.Message}");
      }
    }*/

    //======================================

    public void ResetAndSaveFile()
    {
      ResetGameData();
      SaveData();
    }

    private void ResetGameData()
    {
      GameManager gameManager = GameManager.Instance;

      SettingsData settingsData = gameManager.SettingsData;

      GameData gameData = new()
      {
        MusicValue = settingsData.MusicValue,
        SoundValue = settingsData.SoundValue,
#if !UNITY_PS4
        FullScreenValue = settingsData.FullScreenValue,
        VSyncValue = settingsData.VSyncValue,
#endif
        CurrentLanguage = settingsData.CurrentLanguage
      };

      SetGameData(gameData);
    }

    //======================================

    public GameData GameData()
    {
      GameManager gameManager = GameManager.Instance;

      ProgressData progressData = gameManager.ProgressData;
      SettingsData settingsData = gameManager.SettingsData;

      return new GameData()
      {
#region Settings

        MusicValue = settingsData.MusicValue,
        SoundValue = settingsData.SoundValue,
#if !UNITY_PS4
        FullScreenValue = settingsData.FullScreenValue,
        VSyncValue = settingsData.VSyncValue,
#endif
        CurrentLanguage = settingsData.CurrentLanguage,

#endregion

#region ProgressData

        NumberCompletedLevelsLocation = progressData.NumberCompletedLevelsLocation,
        LevelProgressData = progressData.LevelProgressData,

        CurrentActiveIndexSkin = progressData.CurrentActiveIndexSkin,
        LocationLastLevelPlayed = progressData.LocationLastLevelPlayed,
        IndexLastLevelPlayed = progressData.IndexLastLevelPlayed,
        AmountFoodCollected = progressData.AmountFoodCollected,
        TotalFoodCollected = progressData.TotalFoodCollected,
        PurchasedSkins = progressData.PurchasedSkins,
        TotalNumberMoves = progressData.TotalNumberMoves,
        TotalNumberMovesBox = progressData.TotalNumberMovesBox

#endregion
      };
    }

    public void SetGameData(GameData parData)
    {
      GameManager gameManager = GameManager.Instance;

      ProgressData progressData = gameManager.ProgressData;
      SettingsData settingsData = gameManager.SettingsData;

#region Settings

      settingsData.MusicValue = parData.MusicValue;
      settingsData.SoundValue = parData.SoundValue;
#if !UNITY_PS4
      settingsData.FullScreenValue = parData.FullScreenValue;
      settingsData.VSyncValue = parData.VSyncValue;
#endif
      settingsData.CurrentLanguage = parData.CurrentLanguage;

#endregion

#region ProgressData

      progressData.NumberCompletedLevelsLocation = parData.NumberCompletedLevelsLocation;
      progressData.LevelProgressData = parData.LevelProgressData ?? new Dictionary<Location, Dictionary<int, LevelManagement.LevelProgressData>>();
      
      progressData.CurrentActiveIndexSkin = parData.CurrentActiveIndexSkin;
      progressData.LocationLastLevelPlayed = parData.LocationLastLevelPlayed;
      progressData.IndexLastLevelPlayed = parData.IndexLastLevelPlayed;
      progressData.AmountFoodCollected = parData.AmountFoodCollected;
      progressData.TotalFoodCollected = parData.TotalFoodCollected;
      progressData.PurchasedSkins = parData.PurchasedSkins;
      progressData.TotalNumberMoves = parData.TotalNumberMoves;
      progressData.TotalNumberMovesBox = parData.TotalNumberMovesBox;

#endregion
    }

    //======================================
  }
}