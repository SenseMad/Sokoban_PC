using UnityEngine;
using System.Collections.Generic;

using Sokoban.GameManagement;

namespace Sokoban.UI
{
    public class UILanguageMenu : MenuUI
    {
        [SerializeField] private Panel _languageSelectPanel;
        [SerializeField] private RectTransform _content;
        [SerializeField] private UILanguageButton _languageButton;
        [SerializeField] private List<LanguageData> _listLanguagesData = new();

        private GameManager gameManager;
        private readonly List<UILanguageButton> languageButtons = new();

        public List<LanguageData> ListLanguagesData => _listLanguagesData;

        protected override void Awake()
        {
            base.Awake();
            gameManager = GameManager.Instance;
        }

        private void Start()
        {
            AddLanguagesList();
            SetCurrentLanguageButton();
        }

        protected override void Update()
        {
            MoveMenuVertically(1);
        }

        private void AddLanguagesList()
        {
            _listButtons.Clear();
            languageButtons.Clear();

            foreach (var languageData in _listLanguagesData)
            {
                var listLanguageInstance = Instantiate(_languageButton, _content);
                UILanguageButton button = listLanguageInstance.GetComponent<UILanguageButton>();

                button.Initialize(languageData);
                button.EnableDisableLanguageDisplay(false);
                button.Button.onClick.AddListener(() => button.ChangeLanguage());

                _listButtons.Add(button.Button);
                languageButtons.Add(button);
            }
        }

        private void SetCurrentLanguageButton()
        {
            if (languageButtons.Count == 0)
            {
                indexActiveButton = 0;
                return;
            }

            indexActiveButton = GetLanguageButtonIndex(gameManager.SettingsData.CurrentLanguage);

            if (indexActiveButton < 0 || indexActiveButton >= languageButtons.Count)
                indexActiveButton = 0;

            languageButtons[indexActiveButton].EnableDisableLanguageDisplay(true);
            IsSelectedButton = true;
        }

        private int GetLanguageButtonIndex(Language language)
        {
            for (int i = 0; i < _listLanguagesData.Count; i++)
            {
                if (_listLanguagesData[i].Language == language)
                    return i;
            }

            return 0;
        }

        protected override void MoveMenuVertically(int parValue)
        {
            if (_listButtons.Count == 0 || languageButtons.Count == 0)
                return;

            if (indexActiveButton < 0 || indexActiveButton >= languageButtons.Count)
                indexActiveButton = 0;

            if (Time.time > nextTimeMoveNextValue)
            {
                nextTimeMoveNextValue = Time.time + timeMoveNextValue;

                float navigationInput = inputHandler.GetNavigationInput();

                if (navigationInput > 0)
                {
                    languageButtons[indexActiveButton].EnableDisableLanguageDisplay(false);
                    IsSelectedButton = false;

                    indexActiveButton -= parValue;

                    if (indexActiveButton < 0)
                        indexActiveButton = _listButtons.Count - 1;

                    Sound();
                    IsSelectedButton = true;
                    languageButtons[indexActiveButton].EnableDisableLanguageDisplay(true);
                }
                else if (navigationInput < 0)
                {
                    languageButtons[indexActiveButton].EnableDisableLanguageDisplay(false);
                    IsSelectedButton = false;

                    indexActiveButton += parValue;

                    if (indexActiveButton > _listButtons.Count - 1)
                        indexActiveButton = 0;

                    Sound();
                    IsSelectedButton = true;
                    languageButtons[indexActiveButton].EnableDisableLanguageDisplay(true);
                }
            }

            if (inputHandler.GetNavigationInput() == 0)
            {
                nextTimeMoveNextValue = Time.time;
            }
        }
    }
}