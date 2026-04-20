using UnityEngine;
using YG;

public sealed class LeaderboardService : MonoBehaviour
{
    private const string LB_TIME = "BestTotalTime1";
    private const string LB_FOOD = "most_food_collected";

    public void TrySendTotalTime(float newTime, float bestTime)
    {
        // ¬рем€ Ч меньше = лучше
        if (bestTime > 0f && newTime >= bestTime)
            return;

        YG2.SetLBTimeConvert(LB_TIME, newTime);

        Debug.Log($"[LB] New BEST TIME: {newTime}");
    }

    public void TrySendFood(int newFood, int bestFood)
    {
        // ≈да Ч больше = лучше
        if (newFood <= bestFood)
            return;

        YG2.SetLeaderboard(LB_FOOD, newFood);

        Debug.Log($"[LB] New BEST FOOD: {newFood}");
    }
}