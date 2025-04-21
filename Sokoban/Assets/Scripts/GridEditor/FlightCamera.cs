using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Sokoban.GridEditor
{
  /// <summary>
  /// Полет камеры во время создания уровня
  /// </summary>
  public class FlightCamera : MonoBehaviour
  {
    [Header("СКОРОСТИ ДВИЖЕНИЯ")]
    [SerializeField, Tooltip("Замедленная скорость камеры")]
    private float _slowCameraSpeed = 5f;
    [SerializeField, Tooltip("Обычная скорость камеры")]
    private float _normalCameraSpeed = 10f;
    [SerializeField, Tooltip("Быстрая скорость камеры")]
    private float _fastCameraSpeed = 20f;

    [Header("ВРАЩЕНИЕ КАМЕРЫ")]
    [SerializeField, Tooltip("Скорость вращения камеры")]
    private float _rotationSpeed = 3f;

    //--------------------------------------

    private InputHandler inputHandler;

    private CinemachineCamera virtualCamera;
    private GridEditor gridEditor;

    private Vector2 axisMovement;
    private Vector2 axisLook;

    private float mouseX, mouseY;

    private bool slowCamera;
    private bool fastCamera;

    //======================================

    private void Awake()
    {
      inputHandler = InputHandler.Instance;

      virtualCamera = FindAnyObjectByType<CinemachineCamera>();
      //gridEditor = FindObjectOfType<GridEditor>();
    }

    private void Update()
    {
      Move();

      /*if (!gridEditor.EditMode)
        return;*/

      RotateCamera();

      slowCamera = inputHandler.GetButtonSlowCamera();
      fastCamera = inputHandler.GetButtonFastCamera();
    }

    //======================================

    private void Move()
    {
      axisMovement = inputHandler.GetMove();

      float speed = fastCamera ? _fastCameraSpeed : (!slowCamera ? _normalCameraSpeed : _slowCameraSpeed);
      Vector3 moveDirection = new Vector3(axisMovement.x, 0, axisMovement.y) * speed * Time.deltaTime;
      virtualCamera.transform.Translate(moveDirection, Space.Self);
    }

    /// <summary>
    /// Поворот камеры
    /// </summary>
    private void RotateCamera()
    {
      if (Mouse.current.rightButton.IsPressed())
      {
        axisLook = inputHandler.GetLook();

        mouseX += axisLook.x * _rotationSpeed;
        mouseY -= axisLook.y * _rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -89f, 89f);

        virtualCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
      }
    }

    //======================================
  }
}