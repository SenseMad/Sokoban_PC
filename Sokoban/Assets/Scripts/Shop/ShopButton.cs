using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sokoban.UI
{
  public class ShopButton : MonoBehaviour
  {
    [SerializeField] private Image _imageSkin;

    [SerializeField] private GameObject _priceObject;
    [SerializeField] private TextMeshProUGUI _textPrice;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI _textSelected;
    [SerializeField] private TextMeshProUGUI _textPurchased;

    [Space(10)]
    [SerializeField] private GameObject _selectedIcon;

    //======================================

    public Button Button { get; private set; }

    public int IndexSkin { get; private set; }

    //======================================

    private void Awake()
    {
      Button = GetComponent<Button>();
    }

    //======================================

    public void Initialize(SkinData parSkinData)
    {
      IndexSkin = parSkinData.IndexSkin;

      _imageSkin.sprite = parSkinData.SkinSprite;
      _textPrice.text = $"{parSkinData.PriceSkin}";
    }

    //======================================

    public void Select()
    {
      _imageSkin.color = new Color32(255, 255, 255, 255);

      _selectedIcon.SetActive(true);
      _textSelected.gameObject.SetActive(true);
      _textPurchased.gameObject.SetActive(false);
      _priceObject.SetActive(false);
    }

    public void UnSelect()
    {
      _imageSkin.color = new Color32(75, 75, 75, 255);

      _selectedIcon.SetActive(false);
      _textSelected.gameObject.SetActive(false);
      _textPurchased.gameObject.SetActive(false);
      _priceObject.SetActive(false);
    }

    public void NotPurchased()
    {
      _imageSkin.color = new Color32(0, 0, 0, 255);

      _selectedIcon.SetActive(false);
      _textSelected.gameObject.SetActive(false);
      _textPurchased.gameObject.SetActive(false);
      _priceObject.SetActive(true);
    }

    //======================================
  }
}