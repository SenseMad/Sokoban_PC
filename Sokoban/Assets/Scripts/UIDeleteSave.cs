using Sokoban.GameManagement;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Sokoban.UI
{
    public class UIDeleteSave : MenuUI
    {
        [SerializeField] private Button _yesButton;

        [SerializeField] private Button _noButton;

        [Header("Mobile Button")]
        [SerializeField] private Button _backButton;

        //--------------------------------------

        private GameManager gameManager;

        private bool menuClosed = true;

        //======================================

        protected override void Awake()
        {
            base.Awake();

            gameManager = GameManager.Instance;
        }

        private void Start()
        {
            _listButtons.Add(_yesButton);
            _listButtons.Add(_noButton);

            indexActiveButton = 1;
            OnSelected();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_listButtons != null)
            {
                OnDeselected();
                indexActiveButton = 1;
                OnSelected();
                menuClosed = false;
            }

            _yesButton.onClick.AddListener(() => OnDeleteSave());

            _noButton.onClick.AddListener(() => ClosePanel());

            if (YG2.envir.isMobile)
            {
                if (_backButton != null)
                {
                    _backButton.gameObject.SetActive(true);
                    _backButton.onClick.AddListener(CloseMenu);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _yesButton.onClick.RemoveListener(() => OnDeleteSave());

            _noButton.onClick.RemoveListener(() => ClosePanel());

            if (YG2.envir.isMobile)
            {
                if (_backButton != null)
                {
                    _backButton.gameObject.SetActive(false);
                    _backButton.onClick.RemoveListener(CloseMenu);
                }
            }
        }

        protected override void Update()
        {
            MoveMenuHorizontally();
        }

        //======================================

        private void ClosePanel()
        {
            if (menuClosed == true)
                return;

            menuClosed = true;
            CloseMenu();
        }

        private void OnDeleteSave()
        {
            gameManager.ResetAndSaveFile();
            //panelController.ClosePanel();
            panelController.CloseAllPanels();
        }

        //======================================
    }
}