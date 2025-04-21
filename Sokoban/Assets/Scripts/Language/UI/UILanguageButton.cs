using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Sokoban.GameManagement;

namespace Sokoban.UI
{
  public class UILanguageButton : MonoBehaviour
  {
    [SerializeField] private Image _imageLanguage;

    [SerializeField] private TextMeshProUGUI _textLanguage;

    [SerializeField] private GameObject _objectSelectArrow;

    [SerializeField] private Image _imageArrowSelect;

    //--------------------------------------

    private GameManager gameManager;

    private Language language;

    //======================================

    public Button Button { get; private set; }

    //======================================

    private void Awake()
    {
      gameManager = GameManager.Instance;

      Button = GetComponent<Button>();
    }

    //======================================

    public void Initialize(LanguageData parLanguageData)
    {
      language = parLanguageData.Language;
      _imageLanguage.sprite = parLanguageData.LanguageSprite;
      _textLanguage.text = parLanguageData.LanguageName.ToLower();

      _textLanguage.font = LocalisationSystem.Instance.GetLocalizationFont(parLanguageData.Language);
    }

    public void ChangeLanguage()
    {
      if (gameManager.SettingsData.CurrentLanguage == language)
        return;

      gameManager.SettingsData.CurrentLanguage = language;
      gameManager.SaveData();
    }

    public void EnableDisableLanguageDisplay(bool parValue)
    {
      if (parValue)
      {
        _textLanguage.color = ColorsGame.SELECTED_COLOR;
        _objectSelectArrow.SetActive(true);
        _imageArrowSelect.color = ColorsGame.SELECTED_COLOR;
      }
      else
      {
        _textLanguage.color = ColorsGame.STANDART_COLOR;
        _objectSelectArrow.SetActive(false);
        _imageArrowSelect.color = ColorsGame.STANDART_COLOR;
      }
    }

    //======================================
  }
}