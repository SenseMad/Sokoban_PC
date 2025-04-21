using UnityEngine;
using TMPro;

public class FontAsset : MonoBehaviour
{
  [SerializeField, Tooltip("Русский шрифт")]
  private TMP_FontAsset _russianFont;
  [SerializeField, Tooltip("Английский шрифт")]
  private TMP_FontAsset _englishFont;
  [SerializeField, Tooltip("Французский шрифт")]
  private TMP_FontAsset _frenchFont;
  [SerializeField, Tooltip("Японский шрифт")]
  private TMP_FontAsset _japanFont;
  [SerializeField, Tooltip("Немецкий шрифт")]
  private TMP_FontAsset _germanFont;
  [SerializeField, Tooltip("Испанский шрифт")]
  private TMP_FontAsset _spanishFont;
  [SerializeField, Tooltip("Португальский шрифт")]
  private TMP_FontAsset _portugueseFont;
  [SerializeField, Tooltip("Китайский шрифт")]
  private TMP_FontAsset _chineseFont;

  //======================================

  public TMP_FontAsset GetFont(Language language)
  {
    return language switch
    {
      Language.Russian => _russianFont,
      Language.English => _englishFont,
      Language.French => _frenchFont,
      Language.Japan => _japanFont,
      Language.German => _germanFont,
      Language.Spanish => _spanishFont,
      Language.Portuguese => _portugueseFont,
      Language.Chinese => _chineseFont,
      _ => _englishFont,
    };
  }

  //======================================
}
