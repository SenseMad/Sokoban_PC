using Sokoban.LevelManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using YandexManager;

namespace Sokoban.UI
{
    public class PauseManager : MenuUI
    {
        [SerializeField, Tooltip("Панель паузы")]
        private Panel _pausePanel;

        [SerializeField] private bool _useRestartAdLimit = true;
        [SerializeField, Min(1)] private int _showAdEveryRestarts = 3;

        //--------------------------------------

        private bool isPause;

        private int _restartCounter;

        private LevelManager levelManager;

        //======================================

        private bool IsPause
        {
            get => isPause;
            set
            {
                isPause = value;
                levelManager.IsPause = isPause;
            }
        }

        //======================================

        protected override void Awake()
        {
            base.Awake();

            levelManager = LevelManager.Instance;
        }

        protected override void OnEnable()
        {
            indexActiveButton = 0;

            base.OnEnable();

            inputHandler.AI_Player.UI.Pause.performed += OnPause;

            levelManager.OnPauseEvent.AddListener(OnPause);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            inputHandler.AI_Player.UI.Pause.performed -= OnPause;

            levelManager.OnPauseEvent.RemoveListener(OnPause);
        }

        //======================================

        public void Pause()
        {
            if (!IsPause)
            {
                if (levelManager.LevelCompleted)
                    return;

                IsPause = true;
                panelController.ShowPanel(_pausePanel);
                return;
            }
        }

        private void OnPause(bool parValue)
        {
            IsPause = false;
        }

        //======================================

        protected override void CloseMenu()
        {
            if (!levelManager.GridLevel.IsLevelCreated)
                return;

            if (!IsPause)
            {
                if (levelManager.LevelCompleted)
                    return;

                IsPause = true;
                panelController.ShowPanel(_pausePanel);
                return;
            }

            if (panelController.GetCurrentActivePanel() == _pausePanel)
                base.CloseMenu();

            if (panelController.listAllOpenPanels.Count != 0)
            {
                if (panelController.GetCurrentActivePanel() != _pausePanel)
                    return;
            }

            IsPause = false;

            IsSelectedButton = false;
            indexActiveButton = 0;
            IsSelectedButton = true;
        }

        protected override void ButtonClick()
        {
            if (panelController.GetCurrentActivePanel() != _pausePanel)
                return;

            base.ButtonClick();
        }

        protected override void MoveMenuVertically(int parValue)
        {
            if (panelController.GetCurrentActivePanel() != _pausePanel)
                return;

            base.MoveMenuVertically(parValue);
        }

        //======================================

        private void ClosePausePanel()
        {
            IsSelectedButton = false;
            indexActiveButton = 0;
            IsSelectedButton = true;

            IsPause = false;
            panelController.CloseAllPanels();
        }

        public void ContinueButton()
        {
            ClosePausePanel();
        }

        public void RestartButton()
        {
            ClosePausePanel();

            _restartCounter++;

            bool shouldShowAd = _restartCounter >= _showAdEveryRestarts;

            var adService = YandexAdService.Instance;
            if (shouldShowAd && adService != null && adService.CanShowInterstitial())
            {
                adService.ShowInterstitial(wasShown =>
                {
                    if (wasShown)
                        _restartCounter = 0;

                    if (levelManager != null)
                        levelManager.ReloadLevel();
                });

                return;
            }

            levelManager.ReloadLevel();
        }

        public void ExitMenuButton()
        {
            IsPause = false;
            levelManager.ExitMenu();
        }

        //======================================

        public void OnPause(InputAction.CallbackContext context)
        {

        }

        //======================================
    }
}