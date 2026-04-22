using UnityEngine;
using UnityEngine.EventSystems;

public sealed class MobileInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private Direction _direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MobileButtonsInput.Instance == null)
            return;

        switch (_direction)
        {
            case Direction.Up:
                MobileButtonsInput.Instance.PressUp();
                break;

            case Direction.Down:
                MobileButtonsInput.Instance.PressDown();
                break;

            case Direction.Left:
                MobileButtonsInput.Instance.PressLeft();
                break;

            case Direction.Right:
                MobileButtonsInput.Instance.PressRight();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (MobileButtonsInput.Instance == null)
            return;

        MobileButtonsInput.Instance.Release();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MobileButtonsInput.Instance == null)
            return;

        MobileButtonsInput.Instance.Release();
    }
}