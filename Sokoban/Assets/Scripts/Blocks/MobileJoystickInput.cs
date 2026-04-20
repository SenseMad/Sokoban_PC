using UnityEngine;

public sealed class MobileJoystickInput : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    public static MobileJoystickInput Instance { get; private set; }

    public Vector2 Move => _joystick != null ? _joystick.Direction : Vector2.zero;

    private void Awake()
    {
        Instance = this;
    }

    public bool HasInput(float threshold = 0.5f)
    {
        return Move.sqrMagnitude >= threshold * threshold;
    }
}