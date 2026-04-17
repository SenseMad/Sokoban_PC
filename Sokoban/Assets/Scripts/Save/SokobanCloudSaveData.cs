using System;
using System.Collections.Generic;
using Sokoban.LevelManagement;

namespace Sokoban.Save
{
    [Serializable]
    public sealed class SokobanCloudSaveData
    {
        public int musicValue = 25;
        public int soundValue = 25;
        public bool fullScreenValue = true;
        public bool vSyncValue = true;
        public int currentLanguage = 0;

        public int currentActiveIndexSkin = 0;
        public int locationLastLevelPlayed = 0;
        public int indexLastLevelPlayed = 1;
        public int amountFoodCollected = 0;
        public int totalFoodCollected = 0;
        public int totalNumberMoves = 0;
        public int totalNumberMovesBox = 0;

        public float bestTotalTime = 0f;
        public int bestFoodCollected = 0;
        public int currentRunFoodCollected = 0;

        public List<int> purchasedSkins = new() { 0 };
        public List<LocationProgressEntry> completedLocations = new();
        public List<LevelProgressEntry> levelProgressEntries = new();
    }

    [Serializable]
    public struct LocationProgressEntry
    {
        public int location;
        public int completedLevels;
    }

    [Serializable]
    public struct LevelProgressEntry
    {
        public int location;
        public int levelNumber;
        public LevelProgressDataData progress;
    }

    [Serializable]
    public struct LevelProgressDataData
    {
        public int numberMoves;
        public float timeOnLevel;
    }
}