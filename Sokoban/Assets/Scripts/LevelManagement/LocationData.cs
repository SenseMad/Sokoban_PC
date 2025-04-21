using UnityEngine;

namespace Sokoban.UI
{
  [CreateAssetMenu(fileName = "LocationData", menuName = "Data/LocationData", order = 51)]
  public class LocationData : ScriptableObject
  {
    [SerializeField] private Location _location;

    [SerializeField] private string _translationKey;

    [SerializeField] private Sprite _image;

    //======================================

    public Location Location => _location;

    public string TranslationKey => _translationKey;

    public Sprite Image => _image;

    //======================================
  }
}