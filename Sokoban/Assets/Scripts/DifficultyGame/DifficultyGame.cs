using System;

using Sokoban.LevelManagement;

public class DifficultyGame
{
  public event Action OnLevelFailed;

  //======================================

  public bool IsLevelFailedMoves()
  {
    LevelManager levelManager = LevelManager.Instance;

    if (levelManager.CurrentTypeDifficultyGame == TypeDifficultyGame.Easy)
      return false;

    LevelData levelData = levelManager.GetCurrentLevelData();

    if (levelData.MaximumNumberMoves <= levelManager.NumberMoves)
    {
      OnLevelFailed?.Invoke();
      return true;
    }

    return false;
  }

  public bool IsLevelFailedTime()
  {
    LevelManager levelManager = LevelManager.Instance;

    if (levelManager.CurrentTypeDifficultyGame != TypeDifficultyGame.Hard)
      return false;

    LevelData levelData = levelManager.GetCurrentLevelData();

    if (levelData.MaxTimeOnLevel < levelManager.TimeOnLevel)
    {
      OnLevelFailed?.Invoke();
      return true;
    }

    return false;
  }

  //======================================
}