using Sokoban.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
  public List<LevelData> levelDatas = new List<LevelData>();

  private void Start()
  {
    int number = 0;

    foreach (var levelData in levelDatas)
    {
      foreach (var levelObject in levelData.ListLevelObjects)
      {
        if (levelObject.TypeObject != TypeObject.foodObject)
          continue;

        number++;
      }
    }

    Debug.Log($"{number}");
  }
}