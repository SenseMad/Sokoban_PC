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

    //--------------------------------------

    private GameManager gameManager;

    private readonly List<UILanguageButton> languageButtons = new();

    public List<LanguageData> ListLanguagesData => _listLanguagesData;

    //======================================

    protected override void Awake()
    {
      base.Awake();

      gameManager = GameManager.Instance;
    }

    private void Start()
    {
      indexActiveButton = (int)gameManager.SettingsData.CurrentLanguage;

      AddLanguagesList();
    }

    protected override void Update()
    {
      MoveMenuVertically(1);
    }

    //======================================

    private void AddLanguagesList()
    {
      foreach (var languageData in _listLanguagesData)
      {
        var listLanguageInstance = Instantiate(_languageButton, _content);
        UILanguageButton button = listLanguageInstance.GetComponent<UILanguageButton>();

        button.EnableDisableLanguageDisplay(false);

        if (languageData.Language == gameManager.SettingsData.CurrentLanguage)
        {
          button.EnableDisableLanguageDisplay(true);
          indexActiveButton = (int)gameManager.SettingsData.CurrentLanguage;
        }

        button.Initialize(languageData);
        button.Button.onClick.AddListener(() => button.ChangeLanguage());

        _listButtons.Add(button.Button);
        languageButtons.Add(button);
      }
    }

    //======================================

    protected override void MoveMenuVertically(int parValue)
    {
      if (_listButtons.Count == 0)
        return;

      if (Time.time > nextTimeMoveNextValue)
      {
        nextTimeMoveNextValue = Time.time + timeMoveNextValue;

        if (inputHandler.GetNavigationInput() > 0)
        {
          languageButtons[indexActiveButton].EnableDisableLanguageDisplay(false);
          IsSelectedButton = false;

          indexActiveButton -= parValue;

          if (indexActiveButton < 0) indexActiveButton = _listButtons.Count - 1;

          Sound();
          IsSelectedButton = true;
          languageButtons[indexActiveButton].EnableDisableLanguageDisplay(true);
        }

        if (inputHandler.GetNavigationInput() < 0)
        {
          languageButtons[indexActiveButton].EnableDisableLanguageDisplay(false);
          IsSelectedButton = false;

          indexActiveButton += parValue;

          if (indexActiveButton > _listButtons.Count - 1) indexActiveButton = 0;

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

    //======================================
  }
}