using UnityEngine;
using TMPro;

using Sokoban.GameManagement;

public class LanguageKey : MonoBehaviour
{
  [SerializeField, Tooltip("Ключ перевода")]
  private string _key;

  //--------------------------------------

  private TextMeshProUGUI textField;

  private GameManager gameManager;

  //======================================

  private void Awake()
  {
    textField = GetComponent<TextMeshProUGUI>();

    gameManager = GameManager.Instance;
  }

  private void OnEnable()
  {
    UpdateText();

    gameManager.SettingsData.ChangeLanguage.AddListener(ChangeLanguage);
  }

  private void OnDisable()
  {
    gameManager.SettingsData.ChangeLanguage.RemoveListener(ChangeLanguage);
  }

  //======================================

  private void ChangeLanguage(Language language)
  {
    UpdateText();
  }

  private void UpdateText()
  {
    if (_key == "") { return; }

    var localisationSystem = LocalisationSystem.Instance;
    string value = localisationSystem.GetLocalisedValue(_key);

    value = value.TrimStart(' ', '"');
    value = value.Replace("\"", "");
    textField.text = value;
    textField.font = localisationSystem.GetFont();
  }

  //======================================
}