using UnityEngine;

using Sokoban.GameManagement;

namespace Sokoban.Achievement
{
  public sealed class Achievements : SingletonInGame<Achievements>
  {
    private GameManager gameManager;

    //======================================

    protected override void Awake()
    {
      base.Awake();

      gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
      gameManager.ProgressData.OnTotalNumberMoves += UpdateAchivementMove;

      gameManager.ProgressData.OnTotalNumberMovesBox += UpdateAchivementMoveBox;

      gameManager.ProgressData.OnTotalFoodCollected += UpdateAchivementFood;
    }

    private void OnDisable()
    {
      gameManager.ProgressData.OnTotalNumberMoves -= UpdateAchivementMove;

      gameManager.ProgressData.OnTotalNumberMovesBox -= UpdateAchivementMoveBox;

      gameManager.ProgressData.OnTotalFoodCollected -= UpdateAchivementFood;
    }
    
    //======================================

    public void UpdateAchivementChapter()
    {
      Debug.Log($"UpdateAchivementChapter()");
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_2) >= 15)
        UpdateAchivement(Achievement.CHAPTER_2);

      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_1) >= 15)
        UpdateAchivement(Achievement.CHAPTER_1);
    }
    
    public void UpdateAchivementLevels()
    {
      Debug.Log($"UpdateAchivementLevels()");
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_2) >= 15)
        UpdateAchivement(Achievement.LEVEL_30);
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_2) >= 10)
        UpdateAchivement(Achievement.LEVEL_25);
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_2) >= 5)
        UpdateAchivement(Achievement.LEVEL_20);

      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_1) >= 15)
        UpdateAchivement(Achievement.LEVEL_15);
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_1) >= 10)
        UpdateAchivement(Achievement.LEVEL_10);
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_1) >= 5)
        UpdateAchivement(Achievement.LEVEL_5);
      if (gameManager.ProgressData.GetNumberLevelsCompleted(Location.Chapter_1) >= 1)
        UpdateAchivement(Achievement.LEVEL_1);
    }

    public void UpdateAchivementMove()
    {
      Debug.Log($"UpdateAchivementMove()");
      if (gameManager.ProgressData.TotalNumberMoves >= 250)
        UpdateAchivement(Achievement.MOVE_250);
      if (gameManager.ProgressData.TotalNumberMoves >= 200)
        UpdateAchivement(Achievement.MOVE_200);
      if (gameManager.ProgressData.TotalNumberMoves >= 150)
        UpdateAchivement(Achievement.MOVE_150);
      if (gameManager.ProgressData.TotalNumberMoves >= 100)
        UpdateAchivement(Achievement.MOVE_100);
      if (gameManager.ProgressData.TotalNumberMoves >= 50)
        UpdateAchivement(Achievement.MOVE_50);
      if (gameManager.ProgressData.TotalNumberMoves >= 25)
        UpdateAchivement(Achievement.MOVE_25);
      if (gameManager.ProgressData.TotalNumberMoves >= 10)
        UpdateAchivement(Achievement.MOVE_10);
      if (gameManager.ProgressData.TotalNumberMoves >= 1)
        UpdateAchivement(Achievement.MOVE_1);
    }

    public void UpdateAchivementMoveBox()
    {
      Debug.Log($"UpdateAchivementMoveBox()");
      if (gameManager.ProgressData.TotalNumberMovesBox >= 250)
        UpdateAchivement(Achievement.MOVE_BOX_250);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 200)
        UpdateAchivement(Achievement.MOVE_BOX_200);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 150)
        UpdateAchivement(Achievement.MOVE_BOX_150);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 100)
        UpdateAchivement(Achievement.MOVE_BOX_100);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 75)
        UpdateAchivement(Achievement.MOVE_BOX_75);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 50)
        UpdateAchivement(Achievement.MOVE_BOX_50);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 25)
        UpdateAchivement(Achievement.MOVE_BOX_25);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 15)
        UpdateAchivement(Achievement.MOVE_BOX_15);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 10)
        UpdateAchivement(Achievement.MOVE_BOX_10);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 5)
        UpdateAchivement(Achievement.MOVE_BOX_5);
      if (gameManager.ProgressData.TotalNumberMovesBox >= 1)
        UpdateAchivement(Achievement.MOVE_BOX_1);
    }

    public void UpdateAchivementBuySkin()
    {
      Debug.Log($"UpdateAchivementBuySkin()");

      if (gameManager.ProgressData.PurchasedSkins.Contains(9))
        UpdateAchivement(Achievement.BUY_SKIN_10);
      if (gameManager.ProgressData.PurchasedSkins.Contains(8))
        UpdateAchivement(Achievement.BUY_SKIN_9);
      if (gameManager.ProgressData.PurchasedSkins.Contains(7))
        UpdateAchivement(Achievement.BUY_SKIN_8);
      if (gameManager.ProgressData.PurchasedSkins.Contains(6))
        UpdateAchivement(Achievement.BUY_SKIN_7);
      if (gameManager.ProgressData.PurchasedSkins.Contains(5))
        UpdateAchivement(Achievement.BUY_SKIN_6);
      if (gameManager.ProgressData.PurchasedSkins.Contains(4))
        UpdateAchivement(Achievement.BUY_SKIN_5);
      if (gameManager.ProgressData.PurchasedSkins.Contains(3))
        UpdateAchivement(Achievement.BUY_SKIN_4);
      if (gameManager.ProgressData.PurchasedSkins.Contains(2))
        UpdateAchivement(Achievement.BUY_SKIN_3);
      if (gameManager.ProgressData.PurchasedSkins.Contains(1))
        UpdateAchivement(Achievement.BUY_SKIN_2);
    }

    public void UpdateAchivementFood()
    {
      Debug.Log($"UpdateAchivementFood()");
      if (gameManager.ProgressData.TotalFoodCollected >= 85)
        UpdateAchivement(Achievement.COLLECT_EAT_85);
      if (gameManager.ProgressData.TotalFoodCollected >= 75)
        UpdateAchivement(Achievement.COLLECT_EAT_75);
      if (gameManager.ProgressData.TotalFoodCollected >= 65)
        UpdateAchivement(Achievement.COLLECT_EAT_65);
      if (gameManager.ProgressData.TotalFoodCollected >= 55)
        UpdateAchivement(Achievement.COLLECT_EAT_55);
      if (gameManager.ProgressData.TotalFoodCollected >= 45)
        UpdateAchivement(Achievement.COLLECT_EAT_45);
      if (gameManager.ProgressData.TotalFoodCollected >= 35)
        UpdateAchivement(Achievement.COLLECT_EAT_35);
      if (gameManager.ProgressData.TotalFoodCollected >= 25)
        UpdateAchivement(Achievement.COLLECT_EAT_25);
      if (gameManager.ProgressData.TotalFoodCollected >= 15)
        UpdateAchivement(Achievement.COLLECT_EAT_15);
      if (gameManager.ProgressData.TotalFoodCollected >= 10)
        UpdateAchivement(Achievement.COLLECT_EAT_10);
      if (gameManager.ProgressData.TotalFoodCollected >= 5)
        UpdateAchivement(Achievement.COLLECT_EAT_5);
      if (gameManager.ProgressData.TotalFoodCollected >= 1)
        UpdateAchivement(Achievement.COLLECT_EAT_1);
    }

    //======================================

    private void UpdateAchivement(Achievement parAchievement)
    {
      /*if (gameManager.PlatformManager.Achievements != null && 
        gameManager.PlatformManager.LocalUserProfiles != null)
      {
        var userId = gameManager.PlatformManager.LocalUserProfiles.GetPrimaryLocalUserId();

        var progress = gameManager.PlatformManager.Achievements.GetAchievementProgress(userId, parAchievement);
        if (progress != null)
        {
          if (!progress.IsUnlocked)
          {
            Debug.Log($"UpdateAchivement({parAchievement}) : LocalUserId={userId}");
            gameManager.PlatformManager.Achievements.UnlockAchievement(userId, parAchievement);
          }
          else
          {
            Debug.LogWarning($"UpdateAchivement({parAchievement}) : Achievement already unlocked! LocalUserId={userId}");
          }
        }
        else
        {
          Debug.LogWarning($"UpdateAchivement({parAchievement}) : No progress has been made for the player! LocalUserId={userId}");
        }
      }
      else
      {
        Debug.LogError($"Error UpdateAchivement({parAchievement}): " +
          $"PlatformManager = {gameManager.PlatformManager}, " +
          $"Achievements = {gameManager.PlatformManager?.Achievements}, " +
          $"LocalUserProfiles = {gameManager.PlatformManager?.LocalUserProfiles} ");
      }*/
    }

    //======================================
  }
}