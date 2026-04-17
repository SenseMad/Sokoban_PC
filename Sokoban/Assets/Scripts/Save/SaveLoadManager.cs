using Sokoban.GameManagement;
using Sokoban.LevelManagement;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Sokoban.Save
{
    public sealed class SaveLoadManager
    {
        public static string PATH = $"{Application.persistentDataPath}/SaveData.dat";

        //======================================

        public void SaveData()
        {
            if (!YandexManager.YandexSdkBootstrap.IsInitialized)
            {
                Debug.LogWarning("[Save] SDK is not initialized yet. Save skipped.");
                return;
            }

            var gameManager = GameManager.Instance;
            if (gameManager == null)
                return;

            var cloud = YG2.saves.sokoban;
            WriteToCloudData(cloud, gameManager);

            YG2.SaveProgress();
            Debug.Log("[Save] Progress saved to PluginYG storage");
        }

        public void LoadData()
        {
            if (!YandexManager.YandexSdkBootstrap.IsInitialized)
            {
                Debug.LogWarning("[Save] SDK is not initialized yet. Load skipped.");
                return;
            }

            var gameManager = GameManager.Instance;
            if (gameManager == null)
                return;

            var cloud = YG2.saves.sokoban;
            ReadFromCloudData(cloud, gameManager);

            Debug.Log("[Save] Progress loaded from PluginYG storage");
        }

        public void ResetAndSaveFile()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null)
                return;

            ResetGameData(gameManager);
            SaveData();
        }

        /*private void ResetGameData(GameManager gameManager)
        {
            var settingsData = gameManager.SettingsData;

            var resetData = new SokobanCloudSaveData
            {
                musicValue = settingsData.MusicValue,
                soundValue = settingsData.SoundValue,
#if !UNITY_PS4
                fullScreenValue = settingsData.FullScreenValue,
                vSyncValue = settingsData.VSyncValue,
#endif
                currentLanguage = (int)settingsData.CurrentLanguage
            };

            YG2.saves.sokoban = resetData;
            ReadFromCloudData(resetData, gameManager);
        }*/

        private void ResetGameData(GameManager gameManager)
        {
            var settingsData = gameManager.SettingsData;

            var resetData = new SokobanCloudSaveData
            {
                musicValue = settingsData.MusicValue,
                soundValue = settingsData.SoundValue,

                currentLanguage = (int)settingsData.CurrentLanguage,

                currentActiveIndexSkin = 0,
                locationLastLevelPlayed = (int)Location.Chapter_1,
                indexLastLevelPlayed = 1,
                amountFoodCollected = 0,
                totalFoodCollected = 0,
                totalNumberMoves = 0,
                totalNumberMovesBox = 0,

                bestTotalTime = 0f,
                bestFoodCollected = 0,
                currentRunFoodCollected = 0,

                purchasedSkins = new List<int> { 0 },
                completedLocations = new List<LocationProgressEntry>
                {
                    new LocationProgressEntry
                    {
                        location = (int)Location.Chapter_1,
                        completedLevels = 0
                    }
                },
                levelProgressEntries = new List<LevelProgressEntry>()
            };

            YG2.saves.sokoban = resetData;
            ReadFromCloudData(resetData, gameManager);
        }

        private static void WriteToCloudData(SokobanCloudSaveData cloud, GameManager gameManager)
        {
            var progress = gameManager.ProgressData;
            var settings = gameManager.SettingsData;

            cloud.musicValue = settings.MusicValue;
            cloud.soundValue = settings.SoundValue;
#if !UNITY_PS4
            cloud.fullScreenValue = settings.FullScreenValue;
            cloud.vSyncValue = settings.VSyncValue;
#endif
            cloud.currentLanguage = (int)settings.CurrentLanguage;

            cloud.currentActiveIndexSkin = progress.CurrentActiveIndexSkin;
            cloud.locationLastLevelPlayed = (int)progress.LocationLastLevelPlayed;
            cloud.indexLastLevelPlayed = progress.IndexLastLevelPlayed;
            cloud.amountFoodCollected = progress.AmountFoodCollected;
            cloud.totalFoodCollected = progress.TotalFoodCollected;
            cloud.totalNumberMoves = progress.TotalNumberMoves;
            cloud.totalNumberMovesBox = progress.TotalNumberMovesBox;

            cloud.bestTotalTime = progress.BestTotalTime;
            cloud.bestFoodCollected = progress.BestFoodCollected;
            cloud.currentRunFoodCollected = progress.CurrentRunFoodCollected;

            cloud.purchasedSkins = new List<int>(progress.PurchasedSkins);

            cloud.completedLocations = new List<LocationProgressEntry>();
            foreach (var pair in progress.NumberCompletedLevelsLocation)
            {
                cloud.completedLocations.Add(new LocationProgressEntry
                {
                    location = (int)pair.Key,
                    completedLevels = pair.Value
                });
            }

            cloud.levelProgressEntries = new List<LevelProgressEntry>();
            foreach (var locationPair in progress.LevelProgressData)
            {
                foreach (var levelPair in locationPair.Value)
                {
                    cloud.levelProgressEntries.Add(new LevelProgressEntry
                    {
                        location = (int)locationPair.Key,
                        levelNumber = levelPair.Key,
                        progress = new LevelProgressDataData
                        {
                            numberMoves = levelPair.Value.NumberMoves,
                            timeOnLevel = levelPair.Value.TimeOnLevel
                        }
                    });
                }
            }
        }

        private static void ReadFromCloudData(SokobanCloudSaveData cloud, GameManager gameManager)
        {
            var progress = gameManager.ProgressData;
            var settings = gameManager.SettingsData;

            settings.MusicValue = cloud.musicValue;
            settings.SoundValue = cloud.soundValue;
#if !UNITY_PS4
            settings.FullScreenValue = cloud.fullScreenValue;
            settings.VSyncValue = cloud.vSyncValue;
#endif
            settings.CurrentLanguage = (Language)cloud.currentLanguage;

            progress.NumberCompletedLevelsLocation = new Dictionary<Location, int>();
            if (cloud.completedLocations != null)
            {
                foreach (var entry in cloud.completedLocations)
                    progress.NumberCompletedLevelsLocation[(Location)entry.location] = entry.completedLevels;
            }

            if (progress.NumberCompletedLevelsLocation.Count == 0)
                progress.NumberCompletedLevelsLocation[Location.Chapter_1] = 0;

            progress.LevelProgressData = new Dictionary<Location, Dictionary<int, LevelProgressData>>();
            if (cloud.levelProgressEntries != null)
            {
                foreach (var entry in cloud.levelProgressEntries)
                {
                    var location = (Location)entry.location;

                    if (!progress.LevelProgressData.ContainsKey(location))
                        progress.LevelProgressData[location] = new Dictionary<int, LevelProgressData>();

                    progress.LevelProgressData[location][entry.levelNumber] = new LevelProgressData
                    {
                        NumberMoves = entry.progress.numberMoves,
                        TimeOnLevel = entry.progress.timeOnLevel
                    };
                }
            }

            progress.CurrentActiveIndexSkin = cloud.currentActiveIndexSkin;
            progress.LocationLastLevelPlayed = (Location)cloud.locationLastLevelPlayed;
            progress.IndexLastLevelPlayed = Mathf.Max(1, cloud.indexLastLevelPlayed);
            progress.AmountFoodCollected = cloud.amountFoodCollected;
            progress.TotalFoodCollected = cloud.totalFoodCollected;
            progress.TotalNumberMoves = cloud.totalNumberMoves;
            progress.TotalNumberMovesBox = cloud.totalNumberMovesBox;

            progress.BestTotalTime = cloud.bestTotalTime;
            progress.BestFoodCollected = cloud.bestFoodCollected;
            progress.CurrentRunFoodCollected = cloud.currentRunFoodCollected;

            progress.PurchasedSkins = cloud.purchasedSkins != null && cloud.purchasedSkins.Count > 0
                ? new SortedSet<int>(cloud.purchasedSkins)
                : new SortedSet<int> { 0 };
        }

        /*public void SaveData()
        {
            GameData data = GameData();

            FileStream fileStream = new FileStream(PATH, FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public void LoadData()
        {
            if (File.Exists(PATH))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(PATH, FileMode.Open);
                GameData data = (GameData)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();

                SetGameData(data);
            }
        }*/

        //======================================

        /*public void ResetAndSaveFile()
        {
            ResetGameData();
            SaveData();
        }*/

        private void ResetGameData()
        {
            GameManager gameManager = GameManager.Instance;

            SettingsData settingsData = gameManager.SettingsData;

            GameData gameData = new()
            {
                MusicValue = settingsData.MusicValue,
                SoundValue = settingsData.SoundValue,
#if !UNITY_PS4
                FullScreenValue = settingsData.FullScreenValue,
                VSyncValue = settingsData.VSyncValue,
#endif
                CurrentLanguage = settingsData.CurrentLanguage
            };

            SetGameData(gameData);
        }

        //======================================

        public GameData GameData()
        {
            GameManager gameManager = GameManager.Instance;

            ProgressData progressData = gameManager.ProgressData;
            SettingsData settingsData = gameManager.SettingsData;

            return new GameData()
            {
                #region Settings

                MusicValue = settingsData.MusicValue,
                SoundValue = settingsData.SoundValue,
#if !UNITY_PS4
                FullScreenValue = settingsData.FullScreenValue,
                VSyncValue = settingsData.VSyncValue,
#endif
                CurrentLanguage = settingsData.CurrentLanguage,

                #endregion

                #region ProgressData

                NumberCompletedLevelsLocation = progressData.NumberCompletedLevelsLocation,
                LevelProgressData = progressData.LevelProgressData,

                CurrentActiveIndexSkin = progressData.CurrentActiveIndexSkin,
                LocationLastLevelPlayed = progressData.LocationLastLevelPlayed,
                IndexLastLevelPlayed = progressData.IndexLastLevelPlayed,
                AmountFoodCollected = progressData.AmountFoodCollected,
                TotalFoodCollected = progressData.TotalFoodCollected,
                PurchasedSkins = progressData.PurchasedSkins,
                TotalNumberMoves = progressData.TotalNumberMoves,
                TotalNumberMovesBox = progressData.TotalNumberMovesBox,

                BestTotalTime = progressData.BestTotalTime,
                BestFoodCollected = progressData.BestFoodCollected,
                CurrentRunFoodCollected = progressData.CurrentRunFoodCollected

                #endregion
            };
        }

        public void SetGameData(GameData parData)
        {
            GameManager gameManager = GameManager.Instance;

            ProgressData progressData = gameManager.ProgressData;
            SettingsData settingsData = gameManager.SettingsData;

            #region Settings

            settingsData.MusicValue = parData.MusicValue;
            settingsData.SoundValue = parData.SoundValue;
#if !UNITY_PS4
            settingsData.FullScreenValue = parData.FullScreenValue;
            settingsData.VSyncValue = parData.VSyncValue;
#endif
            settingsData.CurrentLanguage = parData.CurrentLanguage;

            #endregion

            #region ProgressData

            progressData.NumberCompletedLevelsLocation = parData.NumberCompletedLevelsLocation;
            progressData.LevelProgressData = parData.LevelProgressData ?? new Dictionary<Location, Dictionary<int, LevelManagement.LevelProgressData>>();

            progressData.CurrentActiveIndexSkin = parData.CurrentActiveIndexSkin;
            progressData.LocationLastLevelPlayed = parData.LocationLastLevelPlayed;
            progressData.IndexLastLevelPlayed = parData.IndexLastLevelPlayed;
            progressData.AmountFoodCollected = parData.AmountFoodCollected;
            progressData.TotalFoodCollected = parData.TotalFoodCollected;
            progressData.PurchasedSkins = parData.PurchasedSkins;
            progressData.TotalNumberMoves = parData.TotalNumberMoves;
            progressData.TotalNumberMovesBox = parData.TotalNumberMovesBox;

            progressData.BestTotalTime = parData.BestTotalTime;
            progressData.BestFoodCollected = parData.BestFoodCollected;
            progressData.CurrentRunFoodCollected = parData.CurrentRunFoodCollected;

            #endregion
        }

        //======================================
    }
}