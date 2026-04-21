using System.Collections;
using UnityEngine;

public sealed class MobileButtonsInput : MonoBehaviour
{
    public static MobileButtonsInput Instance { get; private set; }

    private Vector2 move;

    public Vector2 Move => move;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        move = Vector2.zero;
    }

    public void PressUp()
    {
        move = Vector2.up;
    }

    public void PressDown()
    {
        move = Vector2.down;
    }

    public void PressLeft()
    {
        move = Vector2.left;
    }

    public void PressRight()
    {
        move = Vector2.right;
    }

    public void Release()
    {
        move = Vector2.zero;
    }

    public bool HasInput()
    {
        return move != Vector2.zero;
    }
}