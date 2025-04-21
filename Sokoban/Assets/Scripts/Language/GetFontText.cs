using UnityEngine;
using TMPro;

using Sokoban.GameManagement;

namespace Sokoban.UI
{
  public class GetFontText : MonoBehaviour
  {
    private TextMeshProUGUI textField;

    //--------------------------------------

    private GameManager gameManager;

    //======================================

    private void Awake()
    {
      textField = GetComponent<TextMeshProUGUI>();

      gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
      ChangeFont();

      gameManager.SettingsData.ChangeLanguage.AddListener(parValue => ChangeFont());
    }

    private void OnDisable()
    {
      gameManager.SettingsData.ChangeLanguage.RemoveListener(parValue => ChangeFont());
    }

    //======================================

    private void ChangeFont()
    {
      textField.font = LocalisationSystem.Instance.GetFont();
    }

    //======================================
  }
}