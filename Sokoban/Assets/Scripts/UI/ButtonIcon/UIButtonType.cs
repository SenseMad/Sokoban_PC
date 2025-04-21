using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;

namespace Sokoban.UI
{
  public class UIButtonType : MonoBehaviour
  {
    [SerializeField] private TypesButtons _typeButton;

    [SerializeField] private ButtonIcons _buttonIcons;

    //--------------------------------------

    private Image image;

    private SpriteRenderer spriteRenderer;

    //======================================

    private void Awake()
    {
      image = GetComponent<Image>();

      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
      UpdateIconButton();
    }

    //======================================

    private void SetSprite(Sprite parSprite)
    {
      if (image != null)
        image.sprite = parSprite;
      else if (spriteRenderer != null)
        spriteRenderer.sprite = parSprite;
    }

    private void UpdateIconButton()
    {
      Sprite sprite = null;

      TypesDevices typeDevice = TypesDevices.Keyboard;
      TypesButtons typeButton = _typeButton;

#if UNITY_STANDALONE_WIN
      InputDevice lastDevice = InputSystem.devices[InputSystem.devices.Count - 1];

      switch (lastDevice)
      {
        case Keyboard:
        case Mouse:
          typeDevice = TypesDevices.Keyboard;
          break;
        case Gamepad:
          if (lastDevice is XInputController)
            typeDevice = TypesDevices.Xbox;
          else if (lastDevice is DualShockGamepad)
            typeDevice = TypesDevices.DualShock;
          break;
        case Joystick:
          typeDevice = TypesDevices.Joystick;
          break;
      }

#elif UNITY_PS4
      typeDevice = TypesDevices.DualShock;
      if (!InputHandler.Instance.EnterButtonCross)
      {
        switch (typeButton)
        {
          case TypesButtons.Select:
            typeButton = TypesButtons.Back;
            break;
          case TypesButtons.Back:
            typeButton = TypesButtons.Select;
            break;
        }
      }
#endif

      sprite = _buttonIcons.GetSprite(typeDevice, typeButton);
      SetSprite(sprite);
    }

    //======================================
  }
}