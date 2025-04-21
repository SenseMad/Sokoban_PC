using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Sokoban.UI
{
  [CreateAssetMenu(fileName = "ButtonIcons", menuName = "ButtonIcons")]
  public class ButtonIcons : ScriptableObject
  {
    [SerializeField] private List<DeviceButtons> _buttons = new();

    //======================================

    public Sprite GetSprite(TypesDevices parTypeDevice, TypesButtons parTypeButton)
    {
      return _buttons.FirstOrDefault(button => button.TypeDevice == parTypeDevice)?.GetSprite(parTypeButton);
    }

    //======================================

    [System.Serializable]
    public class DeviceButtons
    {
      [SerializeField] private TypesDevices _typeDevice;

      [SerializeField] private List<Button> _buttons = new();

      public TypesDevices TypeDevice => _typeDevice;

      [System.Serializable]
      public struct Button
      {
        public TypesButtons TypeButton;
        public Sprite SpriteButton;
      }

      public Sprite GetSprite(TypesButtons parTypeButton)
      {
        return _buttons.FirstOrDefault(button => button.TypeButton == parTypeButton).SpriteButton;
      }
    }

    //======================================
  }

  public enum TypesDevices
  {
    Keyboard,
    DualShock,
    Xbox,
    Joystick
  }

  public enum TypesButtons
  {
    Select,
    Back,
    Reload,
    Q,
    E
  }

  //======================================
}