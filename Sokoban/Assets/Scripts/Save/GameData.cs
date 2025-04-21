using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sokoban.LevelManagement;

namespace Sokoban.Save
{
  [Serializable]
  public class GameData
  {
    #region Settings

    public int MusicValue = 25;
    public int SoundValue = 25;
#if !UNITY_PS4
    public bool FullScreenValue = true;
    public bool VSyncValue = true;
#endif

    public Language CurrentLanguage = Language.English;

    #endregion

    public int AmountFoodCollected = 0;
    public int TotalFoodCollected = 0;

    public int CurrentActiveIndexSkin = 0;

    public SortedSet<int> PurchasedSkins = new() { 0 };

    public int TotalNumberMoves = 0;
    public int TotalNumberMovesBox = 0;

    #region Level

    public Location LocationLastLevelPlayed = Location.Chapter_1;

    public int IndexLastLevelPlayed = 1;

    public Dictionary<Location, int> NumberCompletedLevelsLocation = new()
    {
      { Location.Chapter_1, 0 }
    };

    public Dictionary<Location, Dictionary<int, LevelProgressData>> LevelProgressData = new();

    #endregion
  }
}