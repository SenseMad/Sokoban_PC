using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Sokoban.LevelManagement;

namespace Sokoban.UI
{
  public class UILevelSelectButton : MonoBehaviour
  {
    [SerializeField, Tooltip("Текст названия уровня")]
    private TextMeshProUGUI _textLevelName;

    [SerializeField, Tooltip("Image замка")]
    private Image _imageLock;

    [SerializeField] private Image _background;

    [SerializeField, Tooltip("Спрайт стандартный")]
    private Sprite _spriteStandart;
    [SerializeField, Tooltip("Спрайт выделения")]
    private Sprite _spriteSelected;

    [SerializeField] private GameObject _selectImage;

    //--------------------------------------

    [SerializeField] private Animator animator;

    //======================================

    public Button Button { get; set; }

    //======================================

    public void Initialize(LevelData levelData)
    {
      Button.name = $"{levelData.LevelNumber}";
      _textLevelName.text = $"{levelData.LevelNumber}";
    }

    public void ChangeColor()
    {
      _textLevelName.color = ColorsGame.STANDART_COLOR;
      //imageButton.color = ColorsGame.SELECTED_COLOR_BLACK;

      //IsLevelOpen = true;
      _imageLock.gameObject.SetActive(false);
      _textLevelName.gameObject.SetActive(true);
    }

    public void ChangeSprite(bool parValue)
    {
      animator.enabled = parValue;

      animator.Rebind();
      //_background.sprite = parValue ? _spriteSelected : _spriteStandart;

      //_selectImage.SetActive(parValue);

      //animator.enabled = parValue;
    }

    //======================================
  }
}