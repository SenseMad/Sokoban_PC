using UnityEngine;

namespace Sokoban.LevelManagement
{
  [System.Serializable]
  public sealed class LevelProgressData
  {
    [SerializeField] private int _numberMoves;

    [SerializeField] private float _timeOnLevel;

    //======================================

    public int NumberMoves { get => _numberMoves; set => _numberMoves = value; }

    public float TimeOnLevel { get => _timeOnLevel; set => _timeOnLevel = value; }

    //======================================
  }
}