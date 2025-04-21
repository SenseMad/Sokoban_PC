using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Sokoban.GameManagement;
using Sokoban.LevelManagement;

namespace Sokoban.UI
{
  public class SettingsManager : MenuUI
  {
    [SerializeField] private RangeSpinBox _musicValue;
    [SerializeField] private RangeSpinBox _soundValue;

    [SerializeField] private RectTransform _videoTitle;
    [SerializeField] private ToggleSpinBox _fullscreenValue;
    [SerializeField] private SwitchSpinBox _resolutionValue;
    [SerializeField] private ToggleSpinBox _vSyncValue;

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
#if !UNITY_PS4
      _fullscreenValue.OnValueChanged += FullscreenValue_OnValueChanged;
      _resolutionValue.OnValueChanged += ResolutionValue_OnValueChanged;
      _vSyncValue.OnValueChanged += VSyncValue_OnValueChanged;
#else
      _videoTitle.gameObject.SetActive(false);
      _fullscreenValue.gameObject.SetActive(false);
      _resolutionValue.gameObject.SetActive(false);
      _vSyncValue.gameObject.SetActive(false);
#endif
    }

    private void Start()
    {
      if (_musicValue) spinBoxBases.Add(_musicValue);
      if (_soundValue) spinBoxBases.Add(_soundValue);
#if !UNITY_PS4
      if (_fullscreenValue) spinBoxBases.Add(_fullscreenValue);
      if (_resolutionValue) spinBoxBases.Add(_resolutionValue);
      if (_vSyncValue) spinBoxBases.Add(_vSyncValue);
#endif
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
        _listButtons.Add(button);

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
#if !UNITY_PS4
      _fullscreenValue.SetValueWithoutNotify(gameManager.SettingsData.FullScreenValue);
      _resolutionValue.SetValueWithoutNotify(gameManager.SettingsData.CurrentSelectedResolution);
      _resolutionValue.UpdateText(UpdateResolutionText());
      _vSyncValue.SetValueWithoutNotify(gameManager.SettingsData.VSyncValue);
#endif
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
#if !UNITY_PS4
      _fullscreenValue.OnValueChanged -= FullscreenValue_OnValueChanged;
      _resolutionValue.OnValueChanged -= ResolutionValue_OnValueChanged;
      _vSyncValue.OnValueChanged -= VSyncValue_OnValueChanged;
#endif
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

#if !UNITY_PS4
    private void FullscreenValue_OnValueChanged(bool parValue)
    {
      gameManager.SettingsData.FullScreenValue = parValue;
      Screen.fullScreen = parValue;

      Sound();
    }

    private void ResolutionValue_OnValueChanged(int parValue)
    {
      List<Resolution> resolutions = gameManager.SettingsData.Resolutions;
      if (parValue > resolutions.Count - 1)
      {
        parValue = 0;
        _resolutionValue.SetValueWithoutNotify(0);
      }

      if (parValue < 0)
      {
        parValue = resolutions.Count - 1;
        _resolutionValue.SetValueWithoutNotify(resolutions.Count - 1);
      }

      gameManager.SettingsData.CurrentSelectedResolution = parValue;
      _resolutionValue.UpdateText(UpdateResolutionText());

      gameManager.SettingsData.ApplyResolution();
      Sound();
    }

    private string UpdateResolutionText()
    {
      return $"{gameManager.SettingsData.GetResolution().width} X {gameManager.SettingsData.GetResolution().height}";
    }

    private void VSyncValue_OnValueChanged(bool parValue)
    {
      gameManager.SettingsData.VSyncValue = parValue;

      gameManager.SettingsData.ApplyResolution();

      Sound();
    }
#endif

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