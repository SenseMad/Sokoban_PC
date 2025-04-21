using System.Collections.Generic;
using UnityEngine;

namespace Sokoban.LevelManagement
{
  public sealed class Levels
  {
    private readonly static Dictionary<Location, int> tableNumberLevelsLocation = new();

    //======================================

    public static string GetPathToStorageLevels(Location parLocation)
    {
      return $"Locations/{parLocation}";
    }

    public static string GetPathToStorageLevels(Location parLocation, int parNumLevel)
    {
      return $"Locations/{parLocation}/{parLocation}_{parNumLevel}";
    }

    //======================================

    public static int GetNumberLevelsLocation(Location parLocation)
    {
      if (!tableNumberLevelsLocation.ContainsKey(parLocation))
      {
        Debug.LogError($"Локация {parLocation} не найдена!");
        return 0;
      }

      return tableNumberLevelsLocation[parLocation];
    }

    public static bool GetLocationTable(Location parLocation)
    {
      return tableNumberLevelsLocation.ContainsKey(parLocation);
    }

    //======================================

    public static void GetFullNumberLevelsLocation()
    {
      foreach (var location in GetLocation.GetNamesAllLocation())
      {
        LevelData[] levelData = Resources.LoadAll<LevelData>(GetPathToStorageLevels(location));

        if (levelData.Length == 0)
          continue;

        if (!tableNumberLevelsLocation.ContainsKey(location))
          tableNumberLevelsLocation[location] = 0;

        tableNumberLevelsLocation[location] = levelData.Length;
      }
    }

    //======================================

    public static LevelData GetLevelData(Location parLocation, int parNumLevel)
    {
      if (!GetLocationTable(parLocation))
        return null;

      LevelData retLevelData = Resources.Load<LevelData>(GetPathToStorageLevels(parLocation, parNumLevel));

      if (retLevelData == null)
      {
        //Debug.LogError($"Локация {parLocation} с номером {parNumLevel} не найдена!");
        return null;
      }

      return retLevelData;
    }

    public static LevelData GetNextLevelData(Location parCurrentLocation, int parCurrentLevel)
    {
      if (parCurrentLevel < GetNumberLevelsLocation(parCurrentLocation))
        return GetLevelData(parCurrentLocation, parCurrentLevel + 1);

      if ((int)parCurrentLocation + 1 <= GetLocation.GetNamesAllLocation().Length - 1)
        return GetLevelData(parCurrentLocation + 1, 1);

      return GetLevelData(parCurrentLocation, parCurrentLevel);
    }

    public static List<LevelData> GetListLevelData(Location parLocation)
    {
      List<LevelData> retLevelData = new();

      LevelData[] listDataLevels = Resources.LoadAll<LevelData>(GetPathToStorageLevels(parLocation));

      for (int i = 0; i < listDataLevels.Length - 1; i++)
      {
        for (int j = 0; j < listDataLevels.Length - i - 1; j++)
        {
          if (listDataLevels[j].LevelNumber > listDataLevels[j + 1].LevelNumber)
          {
            LevelData temp = listDataLevels[j];
            listDataLevels[j] = listDataLevels[j + 1];
            listDataLevels[j + 1] = temp;
          }
        }
      }

      for (int i = 0; i < listDataLevels.Length; i++)
      {
        retLevelData.Add(listDataLevels[i]);
      }

      return retLevelData;
    }

    /*public static List<Location> GetListLocation()
    {
      List<Location> retLocations = new();

      foreach (var location in GetLocation.GetNamesAllLocation())
      {
        retLocations.Add(location);
      }

      return retLocations;
    }*/

    //======================================
  }
}