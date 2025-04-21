using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sokoban.UI
{
  public class UILocationSelectButton : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textLocationName;

    [SerializeField] private TextMeshProUGUI _textNumberLevels;

    [Space(10)]
    [SerializeField] private Image _backgroundImage;

    [SerializeField] private Image _imageLock;

    //======================================

    public Button Button { get; set; }

    public Location Location { get; private set; }

    //======================================

    public void Initialize(LocationData parLocationData)
    {
      Button.name = $"{parLocationData.Location}";
      UpdateText(parLocationData);

      _backgroundImage.sprite = parLocationData.Image;

      Location = parLocationData.Location;
    }

    private void UpdateText(LocationData parLocationData)
    {
      if (parLocationData.TranslationKey == "") { return; }

      var localisationSystem = LocalisationSystem.Instance;
      string value = localisationSystem.GetLocalisedValue(parLocationData.TranslationKey);

      value = value.TrimStart(' ', '"');
      value = value.Replace("\"", "");
      _textLocationName.text = value;
      _textLocationName.font = localisationSystem.GetFont();
    }

    public void ChangeColor()
    {
      _imageLock.gameObject.SetActive(false);
      _textNumberLevels.gameObject.SetActive(true);

      //_textLocationName.color = ColorsGame.STANDART_COLOR;
      //imageButton.color = ColorsGame.SELECTED_COLOR_BLACK;
      //imageButton.color = ColorsGame.STANDART_COLOR;
    }

    public void ChangeTextNumberLevels(string parText)
    {
      _textNumberLevels.text = parText;
    }

    public void ChangeSprite(bool parValue)
    {
      if (parValue)
      {
        _textLocationName.color = ColorsGame.SELECTED_COLOR;
        _textNumberLevels.color = ColorsGame.SELECTED_COLOR;
      }
      else
      {
        _textLocationName.color = ColorsGame.STANDART_COLOR;
        _textNumberLevels.color = ColorsGame.STANDART_COLOR;
      }
      //imageButton.sprite = parValue ? _spriteSelected : _spriteStandart;
    }

    //======================================
  }
}