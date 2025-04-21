using System.Collections.Generic;
using UnityEngine;

namespace Sokoban.GridEditor
{
  public class RandomGrassSpawn : MonoBehaviour
  {
    [SerializeField] private TypeBlockObjects _typeBlockObject;

    private List<Block> currentBlocks = new();

    //======================================

    /// <summary>
    /// Определить места для спавна
    /// </summary>
    /// <param name="parFreeSpaceSpawn">Свободный места для спавна</param>
    /// <returns>Места для спавна</returns>
    public Vector3Int[] CheckSpawn(Vector3Int[] parFreeSpaceSpawn)
    {
      int newArraySize = (int)(parFreeSpaceSpawn.Length / 1.8f);
      Vector3Int[] newArray = new Vector3Int[newArraySize];

      System.Random random = new();
      List<Vector3Int> tempFreeSpaceSpawn = new();

      int i = 0;
      while (i < newArraySize)
      {
        int k = random.Next(0, parFreeSpaceSpawn.Length);

        if (tempFreeSpaceSpawn.Contains(parFreeSpaceSpawn[k]))
          continue;

        tempFreeSpaceSpawn.Add(parFreeSpaceSpawn[k]);
        newArray[i] = parFreeSpaceSpawn[k];
        i++;
      }

      return newArray;
    }

    public Block GetRandomTypeBlockObjects()
    {
      System.Random random = new();
      currentBlocks = new();

      int i = 0;
      while (true)
      {
        if (i >= _typeBlockObject.GetListBlockObjects().Count)
          currentBlocks = new();

        int k = random.Next(0, _typeBlockObject.GetListBlockObjects().Count);

        if (currentBlocks.Contains(_typeBlockObject.GetBlockObjectFromList(k)))
        {
          i++;
          continue;
        }

        currentBlocks.Add(_typeBlockObject.GetBlockObjectFromList(k));
        return _typeBlockObject.GetBlockObjectFromList(k);
      }
    }

    //======================================
  }
}