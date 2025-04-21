using System.Collections.Generic;
using UnityEngine;

namespace Sokoban.GridEditor
{
  [CreateAssetMenu(fileName = "New Type Block Objects", menuName = "Data/Type Block Objects", order = 51)]
  public class TypeBlockObjects : ScriptableObject
  {
    [SerializeField] private TypeObject _typeObjects;

    [SerializeField] private List<Block> _listBlockObjects = new();

    //======================================

    public TypeObject GetTypeObjects => _typeObjects;

    public List<Block> GetListBlockObjects() => _listBlockObjects;

    //======================================

    /// <summary>
    /// Получить объект блока из списка
    /// </summary>
    /// <param name="parObjectIndex">Индекс объекта</param>
    public Block GetBlockObjectFromList(int parObjectIndex)
    {
      if (parObjectIndex > _listBlockObjects.Count - 1)
      {
        Debug.LogError("Индекс вышел за пределы массива!");
        return null;
      }

      return _listBlockObjects[parObjectIndex];
    }

    //======================================
  }
}