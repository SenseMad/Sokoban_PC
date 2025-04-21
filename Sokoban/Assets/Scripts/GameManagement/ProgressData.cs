using System.Collections.Generic;
using UnityEngine;
using System;

using Sokoban.LevelManagement;

namespace Sokoban.GameManagement
{
  public sealed class ProgressData
  {
    private int amountFoodCollected = 0;
    private int totalFoodCollected = 0;

    private int totalNumberMoves = 0;
    private int totalNumberMovesBox = 0;

    //======================================

    #region (Tables) Locations/Levels

    public Dictionary<Location, int> NumberCompletedLevelsLocation = new()
    {
      { Location.Chapter_1, 0 }
    };

    public Dictionary<Location, Dictionary<int, LevelProgressData>> LevelProgressData = new();

    #endregion

    public Location LocationLastLevelPlayed { get; set; } = Location.Chapter_1;

    public int IndexLastLevelPlayed { get; set; } = 1;

    public int AmountFoodCollected
    {
      get => amountFoodCollected;
      set
      {
        amountFoodCollected = value;
        OnAmountFoodCollected?.Invoke(value);
      }
    }

    public int TotalFoodCollected
    {
      get => totalFoodCollected;
      set
      {
        totalFoodCollected = value;
        OnTotalFoodCollected?.Invoke();
      }
    }

    public int CurrentActiveIndexSkin { get; set; } = 0;

    public SortedSet<int> PurchasedSkins { get; set; } = new() { 0 };

    public int TotalNumberMoves
    {
      get => totalNumberMoves;
      set
      {
        totalNumberMoves = value;
        OnTotalNumberMoves?.Invoke();
      }
    }

    public int TotalNumberMovesBox
    {
      get => totalNumberMovesBox;
      set
      {
        totalNumberMovesBox = value;
        OnTotalNumberMovesBox?.Invoke();
      }
    }

    //======================================

    public event Action<int> OnAmountFoodCollected;
    public event Action OnTotalFoodCollected;

    public event Action OnTotalNumberMoves;
    public event Action OnTotalNumberMovesBox;

    //======================================

    public bool OpenLocation(Location parLocation)
    {
      if (!Levels.GetLocationTable(parLocation))
        return false;

      if (NumberCompletedLevelsLocation.ContainsKey(parLocation))
      {
        Debug.Log($"Локация {parLocation} уже открыта!");
        return false;
      }

      NumberCompletedLevelsLocation[parLocation] = 0;
      return true;
    }

    public bool OpenNextLocation(Location parCurrentLocation, int parCurrentLevel)
    {
      if (parCurrentLevel < Levels.GetNumberLevelsLocation(parCurrentLocation))
        return false;

      if ((int)parCurrentLocation + 1 > GetLocation.GetNamesAllLocation().Length - 1)
        return false;

      if (!OpenLocation(parCurrentLocation + 1))
        return false;

      Debug.Log($"Локация {parCurrentLocation + 1} открыта!");
      return true;
    }

    public bool OpenNextLevel(Location parCurrentLocation, int parCurrentLevel)
    {
      if (!NumberCompletedLevelsLocation.ContainsKey(parCurrentLocation))
      {
        Debug.Log($"Локация {parCurrentLocation} еще не открыта!");
        return false;
      }

      int currentNumberLevel = GetNumberLevelsCompleted(parCurrentLocation);
      if (parCurrentLevel <= currentNumberLevel)
        return false;

      currentNumberLevel++;

      if (currentNumberLevel >= Levels.GetNumberLevelsLocation(parCurrentLocation))
      {
        OpenNextLocation(parCurrentLocation, parCurrentLevel);
        NumberCompletedLevelsLocation[parCurrentLocation] = currentNumberLevel;
        return true;
      }

      NumberCompletedLevelsLocation[parCurrentLocation] = currentNumberLevel;

      Debug.Log($"Уровень {parCurrentLevel + 1} открыт!");
      return true;
    }

    public int GetNumberLevelsCompleted(Location parLocation)
    {
      if (!NumberCompletedLevelsLocation.ContainsKey(parLocation))
      {
        Debug.Log($"Локация {parLocation} еще не открыта!");
        return 0;
      }

      return NumberCompletedLevelsLocation[parLocation];
    }

    public void SaveProgressLevelData(LevelProgressData parLevelProgressData, Location parLocation, int parLevelNumber)
    {
      if (!LevelProgressData.ContainsKey(parLocation))
      {
        LevelProgressData[parLocation] = new Dictionary<int, LevelProgressData>();
      }

      LevelProgressData[parLocation][parLevelNumber] = parLevelProgressData;
    }

    //======================================

    public bool IsLocationOpen(Location parLocation)
    {
      return NumberCompletedLevelsLocation.ContainsKey(parLocation);
    }

    //======================================
  }
}