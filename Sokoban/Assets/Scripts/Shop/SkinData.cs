using UnityEngine;

[CreateAssetMenu(fileName = "New Skin Data", menuName = "Data/Skin Data", order = 51)]
public class SkinData : ScriptableObject
{
  [SerializeField] private int _indexSkin;

  [SerializeField] private int _priceSkin;

  [SerializeField] private Sprite _skinSprite;

  [SerializeField] private GameObject _objectSkin;

  //======================================

  public int IndexSkin { get => _indexSkin; private set => _indexSkin = value; }

  public int PriceSkin { get => _priceSkin; private set => _priceSkin = value; }

  public Sprite SkinSprite { get => _skinSprite; private set => _skinSprite = value; }

  public GameObject ObjectSkin { get => _objectSkin; private set => _objectSkin = value; }

  //======================================
}