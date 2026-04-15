using Sokoban.LevelManagement;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using YandexManager;

namespace Sokoban.UI
{
    public class UILevelComplete : MenuUI
    {
        [Header("ПАНЕЛЬ")]
        [SerializeField] private Panel _levelCompletePanel;
        [SerializeField] private GameObject _topPanelObjectMenu;

        [Header("ТЕКСТЫ")]
        [SerializeField] private TextMeshProUGUI _textLevelCompletedTime;
        [SerializeField] private TextMeshProUGUI _textLevelNumber;
        [SerializeField] private TextMeshProUGUI _textNumberMoves;

        [SerializeField, Min(1)] private int _showAdEveryNextLevels = 1;
        [SerializeField, Min(1)] private int _showAdEveryRestartLevel = 2;

        //--------------------------------------

        private LevelManager levelManager;

        private int _nextLevelCounter;
        private int _restartLevelCounter;

        //======================================

        protected override void Awake()
        {
            base.Awake();

            levelManager = LevelManager.Instance;
        }

        protected override void OnEnable()
        {
            IsSelectedButton = false;
            indexActiveButton = _listButtons.Count - 1;

            base.OnEnable();

            inputHandler.AI_Player.UI.Select.performed += OnSelect;
            inputHandler.AI_Player.UI.Reload.performed += OnReload;
            inputHandler.AI_Player.UI.Pause.performed += OnExitMenu;

            levelManager.IsNextLevel.AddListener(panelController.CloseAllPanels);

            levelManager.OnLevelCompleted += UpdateText;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            inputHandler.AI_Player.UI.Select.performed -= OnSelect;
            inputHandler.AI_Player.UI.Reload.performed -= OnReload;
            inputHandler.AI_Player.UI.Pause.performed -= OnExitMenu;

            levelManager.IsNextLevel.RemoveListener(panelController.CloseAllPanels);

            levelManager.OnLevelCompleted -= UpdateText;
        }

        //======================================

        /// <summary>
        /// Обновить текст при завершении уровня
        /// </summary>
        private void UpdateText()
        {
            if (levelManager.LevelFailed)
                return;

            UpdateTextTimeLevel();
            UpdateTextLevelNumber();
            UpdateTextNumberMoves();

            panelController.ShowPanel(_levelCompletePanel);

            _topPanelObjectMenu.SetActive(false);
        }

        private void UpdateTextTimeLevel()
        {
            _textLevelCompletedTime.text = $"{levelManager.UpdateTextTimeLevel()}";
        }

        private void UpdateTextLevelNumber()
        {
            //_textLevelNumber.text = $"Level {levelManager.GetCurrentLevelData().LevelNumber}";
            _textLevelNumber.text = $"{UpdateText("LEVEL_KEY")} {levelManager.GetCurrentLevelData().LevelNumber}";
        }

        private void UpdateTextNumberMoves()
        {
            _textNumberMoves.text = $"{levelManager.NumberMoves}";
        }

        protected override void CloseMenu()
        {
            ExitMenu();

            Sound();
        }

        private string UpdateText(string parKey)
        {
            if (parKey == "") { return ""; }

            var localisationSystem = LocalisationSystem.Instance;
            string value = localisationSystem.GetLocalisedValue(parKey);

            value = value.TrimStart(' ', '"');
            value = value.Replace("\"", "");

            return value;
        }

        //======================================

        public void NextLevel()
        {
            if (!levelManager.LevelCompleted)
                return;

            _nextLevelCounter++;

            var adService = YandexAdService.Instance;
            bool shouldShowAd = _nextLevelCounter >= _showAdEveryNextLevels;

            if (shouldShowAd && adService != null && adService.CanShowInterstitial())
            {
                adService.ShowInterstitial(wasShown =>
                {
                    if (wasShown)
                        _nextLevelCounter = 0;

                    levelManager.UploadNewLevel();

                    ResetSelection();
                });

                return;
            }

            levelManager.UploadNewLevel();

            ResetSelection();
        }

        public void ReloadLevel()
        {
            if (!levelManager.LevelCompleted)
                return;

            panelController.CloseAllPanels();

            _restartLevelCounter++;

            var adService = YandexAdService.Instance;
            bool shouldShowAd = _restartLevelCounter >= _showAdEveryRestartLevel;

            if (shouldShowAd && adService != null && adService.CanShowInterstitial())
            {
                adService.ShowInterstitial(wasShown =>
                {
                    if (wasShown)
                        _restartLevelCounter = 0;

                    levelManager.ReloadLevel();

                    ResetSelection();
                });

                return;
            }

            levelManager.ReloadLevel();

            ResetSelection();
        }

        public void ExitMenu()
        {
            if (!levelManager.LevelCompleted)
                return;

            levelManager.ExitMenu();

            IsSelectedButton = false;
            indexActiveButton = _listButtons.Count - 1;
            IsSelectedButton = true;
        }

        private void ResetSelection()
        {
            IsSelectedButton = false;
            indexActiveButton = _listButtons.Count - 1;
            IsSelectedButton = true;
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            NextLevel();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            ReloadLevel();
        }

        public void OnExitMenu(InputAction.CallbackContext context)
        {
            ExitMenu();
        }

        //======================================
    }
}