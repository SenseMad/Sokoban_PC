using Sokoban.GameManagement;
using Sokoban.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Sokoban.UI
{
    public class SettingsManager : MenuUI
    {
        [SerializeField] private RangeSpinBox _musicValue;
        [SerializeField] private RangeSpinBox _soundValue;

        /*[SerializeField] private RectTransform _videoTitle;
        [SerializeField] private ToggleSpinBox _fullscreenValue;
        [SerializeField] private SwitchSpinBox _resolutionValue;
        [SerializeField] private ToggleSpinBox _vSyncValue;*/

        [SerializeField] private RectTransform _languageTitle;
        [SerializeField] private ButtonSpinBox _languageButton;
        [SerializeField] private RectTransform _deleteSavesTitle;
        [SerializeField] private ButtonSpinBox _deleteSavesButton;

        [Space(10)]
        [SerializeField] private UILanguageMenu _languageMenu;
        [SerializeField] private Image _iconSelectedLanguage;
        [SerializeField] private TextMeshProUGUI _textLanguage;

        //--------------------------------------

        private GameManager gameManager;

        private LevelManager levelManager;

        private List<SpinBoxBase> spinBoxBases = new();

        private bool isGameRunning = false;

        //======================================

        protected override void Awake()
        {
            base.Awake();

            gameManager = GameManager.Instance;

            levelManager = LevelManager.Instance;

            _musicValue.OnValueChanged += MusicValue_OnValueChanged;
            _soundValue.OnValueChanged += SoundValue_OnValueChanged;
        }

        private void Start()
        {
            if (_musicValue) spinBoxBases.Add(_musicValue);
            if (_soundValue) spinBoxBases.Add(_soundValue);
            if (_languageButton) spinBoxBases.Add(_languageButton);
            if (_deleteSavesButton) spinBoxBases.Add(_deleteSavesButton);
            isGameRunning = true;

            foreach (var spinBoxBase in spinBoxBases)
            {
                spinBoxBase.IsSelected = false;
            }

            OnSelected();
        }

        protected override void OnEnable()
        {
            spinBoxBases.Remove(_languageButton);
            spinBoxBases.Remove(_deleteSavesButton);

            if (levelManager.IsLevelRunning)
            {
                _languageButton.gameObject.SetActive(false);
                _deleteSavesButton.gameObject.SetActive(false);
                _languageTitle.gameObject.SetActive(false);
                _deleteSavesTitle.gameObject.SetActive(false);
            }
            else
            {
                if (isGameRunning)
                {
                    spinBoxBases.Add(_languageButton);
                    spinBoxBases.Add(_deleteSavesButton);
                }

                _languageButton.gameObject.SetActive(true);
                _deleteSavesButton.gameObject.SetActive(true);
                _languageTitle.gameObject.SetActive(true);
                _deleteSavesTitle.gameObject.SetActive(true);
            }

            Button[] buttons = GetComponentsInChildren<Button>(false);
            foreach (var button in buttons)
            {
                if (button.GetComponent<WithoutNavigation>())
                    continue;

                _listButtons.Add(button);
            }

            indexActiveButton = 0;

            base.OnEnable();

            if (_languageMenu != null)
            {
                foreach (var language in _languageMenu.ListLanguagesData)
                {
                    if (gameManager.SettingsData.CurrentLanguage != language.Language)
                        continue;

                    ChangeIconSelectedLanguage(language.LanguageSprite, language.LanguageName.ToUpper());
                }
            }

            _musicValue.SetValueWithoutNotify(gameManager.SettingsData.MusicValue);
            _soundValue.SetValueWithoutNotify(gameManager.SettingsData.SoundValue);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _listButtons = new List<Button>();
        }

        private void OnDestroy()
        {
            _musicValue.OnValueChanged -= MusicValue_OnValueChanged;
            _soundValue.OnValueChanged -= SoundValue_OnValueChanged;
        }

        //======================================

        private void MusicValue_OnValueChanged(int parValue)
        {
            gameManager.SettingsData.MusicValue = parValue;
            Sound();
        }

        private void SoundValue_OnValueChanged(int parValue)
        {
            gameManager.SettingsData.SoundValue = parValue;
            Sound();
        }

        private void ChangeIconSelectedLanguage(Sprite parSprite, string parText)
        {
            _iconSelectedLanguage.sprite = parSprite;
            _textLanguage.text = parText;
        }

        //======================================

        protected override void OnSelected()
        {
            base.OnSelected();

            if (indexActiveButton > spinBoxBases.Count - 1)
                return;

            spinBoxBases[indexActiveButton].IsSelected = true;
        }

        protected override void OnDeselected()
        {
            base.OnDeselected();

            if (indexActiveButton > spinBoxBases.Count - 1)
                return;

            spinBoxBases[indexActiveButton].IsSelected = false;
        }

        //======================================
    }
}