using System.Collections.Generic;
using UnityEngine;

namespace Sokoban.LevelManagement
{
  [System.Serializable]
  [CreateAssetMenu(fileName = "New Level Data", menuName = "Data/Level Data", order = 51)]
  public class LevelData : ScriptableObject
  {
    [SerializeField] private int _levelNumber;

    [SerializeField] private Location _location;

    [SerializeField] private Vector3Int _fieldSize;

    [SerializeField] private List<GridData> _listLevelObjects;

    [SerializeField] private int _maximumNumberSteps;

    [SerializeField] private float _maxTimeOnLevel;

    //======================================

    public int LevelNumber { get => _levelNumber; set => _levelNumber = value; }

    public Location Location { get => _location; set => _location = value; }

    public List<GridData> ListLevelObjects { get => _listLevelObjects; set => _listLevelObjects = value; }

    public Vector3Int FieldSize { get => _fieldSize; set => _fieldSize = value; }

    public int MaximumNumberMoves { get => _maximumNumberSteps; set => _maximumNumberSteps = value; }

    public float MaxTimeOnLevel { get => _maxTimeOnLevel; set => _maxTimeOnLevel = value; }

    //======================================

    [System.Serializable]
    public class GridData
    {
      [SerializeField] private TypeObject _typeObject;

      [SerializeField] private int _indexObject;

      [SerializeField] private Vector3Int _positionObject;

      //======================================

      public TypeObject TypeObject { get => _typeObject; set => _typeObject = value; }

      public int IndexObject { get => _indexObject; set => _indexObject = value; }

      public Vector3Int PositionObject { get => _positionObject; set => _positionObject = value; }

      //======================================
    }

    //======================================
  }
}