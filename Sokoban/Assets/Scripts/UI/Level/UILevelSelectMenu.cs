using Sokoban.GameManagement;
using Sokoban.LevelManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YandexManager;

namespace Sokoban.UI
{
    public class UILevelSelectMenu : MenuUI
    {
        [SerializeField] private RectTransform _levelSelectPanel;

        [Space(10)]
        [SerializeField] private UILevelSelectButton _prefabButtonLevelSelect;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _textNumberCompletedLevels;

        [Space(10)]
        [SerializeField] private List<GameObject> _listDisabledObjects = new();

        //--------------------------------------

        private GameManager gameManager;

        private List<UILevelSelectButton> listUILevelSelectButton = new();

        private Location currentLocation;

        private float nextTimeMoveNextValue1;

        private bool _isStartingLevel;

        //======================================

        protected override void Awake()
        {
            base.Awake();

            gameManager = GameManager.Instance;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _isStartingLevel = false;
        }

        protected override void Update()
        {
            MoveMenuHorizontally();

            MoveMenuVertically(5);
        }

        //======================================

        protected override void MoveMenuHorizontally()
        {
            if (_listButtons.Count == 0)
                return;

            if (Time.time > nextTimeMoveNextValue)
            {
                nextTimeMoveNextValue = Time.time + timeMoveNextValue;

                if (inputHandler.GetChangingValuesInput() > 0)
                {
                    IsSelectedButton = false;

                    listUILevelSelectButton[indexActiveButton].ChangeSprite(false);
                    indexActiveButton++;

                    if (indexActiveButton > _listButtons.Count - 1) indexActiveButton = 0;
                    if (gameManager.ProgressData.GetNumberLevelsCompleted(currentLocation) < indexActiveButton)
                        indexActiveButton = 0;

                    listUILevelSelectButton[indexActiveButton].ChangeSprite(true);

                    Sound();
                    IsSelectedButton = true;
                }

                if (inputHandler.GetChangingValuesInput() < 0)
                {
                    IsSelectedButton = false;

                    listUILevelSelectButton[indexActiveButton].ChangeSprite(false);
                    indexActiveButton--;

                    if (indexActiveButton < 0) indexActiveButton = _listButtons.Count - 1;
                    while (gameManager.ProgressData.GetNumberLevelsCompleted(currentLocation) < indexActiveButton)
                        indexActiveButton--;

                    listUILevelSelectButton[indexActiveButton].ChangeSprite(true);

                    Sound();
                    IsSelectedButton = true;
                }
            }

            if (inputHandler.GetChangingValuesInput() == 0)
            {
                nextTimeMoveNextValue = Time.time;
            }
        }

        protected override void MoveMenuVertically(int parValue)
        {
            if (_listButtons.Count == 0)
                return;

            if (Time.time > nextTimeMoveNextValue1)
            {
                nextTimeMoveNextValue1 = Time.time + timeMoveNextValue;

                if (inputHandler.GetNavigationInput() > 0)
                {
                    if (indexActiveButton - parValue < 0)
                        return;

                    IsSelectedButton = false;

                    listUILevelSelectButton[indexActiveButton].ChangeSprite(false);
                    indexActiveButton -= parValue;
                    listUILevelSelectButton[indexActiveButton].ChangeSprite(true);

                    Sound();
                    IsSelectedButton = true;
                }

                if (inputHandler.GetNavigationInput() < 0)
                {
                    if (indexActiveButton + parValue > _listButtons.Count - 1)
                        return;
                    if (gameManager.ProgressData.GetNumberLevelsCompleted(currentLocation) < indexActiveButton + parValue)
                        return;

                    IsSelectedButton = false;

                    listUILevelSelectButton[indexActiveButton].ChangeSprite(false);
                    indexActiveButton += parValue;
                    listUILevelSelectButton[indexActiveButton].ChangeSprite(true);

                    Sound();
                    IsSelectedButton = true;
                }
            }

            if (inputHandler.GetNavigationInput() == 0)
            {
                nextTimeMoveNextValue1 = Time.time;
            }
        }

        public void DisplayLevelSelectionButtonsUI(Location parLocation)
        {
            _isStartingLevel = false;

            ClearButtonsUI();

            indexActiveButton = -1;

            foreach (var levelData in Levels.GetListLevelData(parLocation))
            {
                UILevelSelectButton button = Instantiate(_prefabButtonLevelSelect, _levelSelectPanel);

                button.Button = button.GetComponent<Button>();
                if (gameManager.ProgressData.GetNumberLevelsCompleted(parLocation) >= levelData.LevelNumber - 1)
                {
                    button.ChangeColor();
                    button.Button.interactable = true;
                    button.Button.onClick.AddListener(() => SelectLevel(levelData));
                    indexActiveButton++;
                }

                listUILevelSelectButton.Add(button);
                _listButtons.Add(button.GetComponent<Button>());
                button.Initialize(levelData);
            }

            currentLocation = parLocation;

            listUILevelSelectButton[indexActiveButton].ChangeSprite(true);

            _textNumberCompletedLevels.text = $"{gameManager.ProgressData.GetNumberLevelsCompleted(parLocation)}/{Levels.GetNumberLevelsLocation(parLocation)}";

            base.OnEnable();
        }

        private void ClearButtonsUI()
        {
            for (int i = 0; i < listUILevelSelectButton.Count; i++)
            {
                Destroy(listUILevelSelectButton[i].gameObject);
            }

            listUILevelSelectButton = new List<UILevelSelectButton>();
            _listButtons = new List<Button>();
        }

        protected override void OnSelected()
        {
            if (_listButtons.Count == 0)
                return;

            var listButtons = _listButtons[indexActiveButton];

            var rectTransform = listButtons.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);
        }

        protected override void OnDeselected()
        {
            if (_listButtons.Count == 0)
                return;

            Button listButtons = _listButtons[indexActiveButton];

            RectTransform rectTransform = listButtons.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
        }

        //======================================

        private void SelectLevel(LevelData levelData)
        {
            if (_isStartingLevel)
                return;

            var levelManager = LevelManager.Instance;
            if (levelManager == null)
                return;

            if (!levelManager.GridLevel.IsLevelDeleted)
                return;

            _isStartingLevel = true;

            gameManager.ProgressData.LocationLastLevelPlayed = levelData.Location;
            gameManager.ProgressData.IndexLastLevelPlayed = levelData.LevelNumber - 1;
            gameManager.SaveData();

            var adService = YandexAdService.Instance;
            bool canShowAd = adService != null && adService.CanShowInterstitial();

            if (canShowAd)
            {
                adService.ShowInterstitial(wasShown =>
                {
                    LaunchLevel(levelData);
                });
                return;
            }

            LaunchLevel(levelData);
        }

        /*private void SelectLevel(LevelData levelData)
        {
            if (_isStartingLevel)
                return;

            var levelManager = LevelManager.Instance;
            if (levelManager == null)
                return;

            if (!levelManager.GridLevel.IsLevelDeleted)
                return;

            PanelController.Instance.CloseAllPanels1();

            levelManager.OnPauseEvent?.Invoke(false);

            gameManager.ProgressData.LocationLastLevelPlayed = levelData.Location;
            gameManager.ProgressData.IndexLastLevelPlayed = levelData.LevelNumber - 1;

            gameManager.SaveData();

            var adService = YandexAdService.Instance;

            bool canShowAd = adService != null && adService.CanShowInterstitial();
            //Debug.Log($"SelectLevel: CanShowInterstitial = {canShowAd}");

            if (canShowAd)
            {
                adService.ShowInterstitial(_ =>
                {
                    StartLevel(levelData);
                });
                return;
            }

            StartLevel(levelData);
        }*/

        private void StartLevel(LevelData levelData)
        {
            var levelManager = LevelManager.Instance;
            if (levelManager == null)
            {
                _isStartingLevel = false;
                return;
            }

            _isStartingLevel = true;

            levelManager.ReloadLevel(levelData);
            levelManager.IsLevelRunning = true;
        }

        private void LaunchLevel(LevelData levelData)
        {
            var levelManager = LevelManager.Instance;
            if (levelManager == null)
            {
                _isStartingLevel = false;
                return;
            }

            PanelController.Instance.CloseAllPanels1();
            levelManager.OnPauseEvent?.Invoke(false);

            levelManager.ReloadLevel(levelData);
            levelManager.IsLevelRunning = true;
        }

        //======================================
    }
}