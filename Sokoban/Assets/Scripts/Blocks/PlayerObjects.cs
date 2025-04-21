using UnityEngine;
using UnityEngine.Events;

using Sokoban.LevelManagement;
using Sokoban.GridEditor;
using Sokoban.GameManagement;

public class PlayerObjects : Block
{
  [SerializeField] private float _speed = 2.0f;
  [SerializeField] private float _speedRotaion = 7.0f;

  [SerializeField] private Transform _meshTransform;

  [SerializeField] private AudioClip _soundMove;

  //--------------------------------------

  private Animator animator;

  private InputHandler inputHandler;

  private LevelManager levelManager;

  /// <summary>
  /// True, если можно двигаться
  /// </summary>
  private bool isPossibleMove = true;
  /// <summary>
  /// True, если игрок движется
  /// </summary>
  private bool isMoving = false;
  /// <summary>
  /// True, если игрок поворачивается
  /// </summary>
  private bool isPlayerRotation = false;

  /// <summary>
  /// Новая позиция
  /// </summary>
  private Vector3Int lastPosition;
  /// <summary>
  /// Направление движения
  /// </summary>
  private Vector3 direction;
  
  /// <summary>
  /// Сохраняет значение Time.time, когда была нажата кнопка движения в последний раз
  /// </summary>
  private float lastTime = 0;
  /// <summary>
  /// Время задержки перед следующим нажатием кнопки
  /// </summary>
  private float delayTimeNextButtonPress = 0f;

  #region Камера

  /// <summary>
  /// True, если камера поворачивается
  /// </summary>
  private bool isCameraRotation = false;
  private float targetRotation;

  #endregion

  //======================================

  public new Rigidbody rigidbody { get; private set; }

  //======================================

  /// <summary>
  /// Событие: Смерть игрока
  /// </summary>
  public UnityEvent OnPlayerDeathEvent { get; } = new UnityEvent();

  //======================================

  protected override void Awake()
  {
    base.Awake();

    //rigidbody = GetComponent<Rigidbody>();

    animator = GetComponent<Animator>();

    inputHandler = InputHandler.Instance;

    levelManager = LevelManager.Instance;
  }

  private void Start()
  {
    if (GridEditor.GridEditorEnabled)
      RemoveRigidbody();

    if (!GridEditor.GridEditorEnabled)
      levelManager.CinemachineVirtual.Follow = transform;

    lastPosition = Vector3Int.CeilToInt(transform.position);
    direction = transform.position;
  }

  private void OnEnable()
  {
    if (!isCameraRotation)
    {
      inputHandler.AI_Player.Camera.RotationLeft.performed += parValue => CameraRotation(90);
      inputHandler.AI_Player.Camera.RotationRight.performed += parValue => CameraRotation(-90);
    }
  }

  private void OnDisable()
  {
    if (!isCameraRotation)
    {
      inputHandler.AI_Player.Camera.RotationLeft.performed -= parValue => CameraRotation(90);
      inputHandler.AI_Player.Camera.RotationRight.performed -= parValue => CameraRotation(-90);
    }
  }

  private void Update()
  {
    if (GridEditor.GridEditorEnabled)
      return;

    PlayerMovement();

    SmoothCameraRotation();
    SmoothPlayerRotation();

    if (!isMoving)
      return;

    transform.position = Vector3.MoveTowards(transform.position, lastPosition + direction, _speed * Time.deltaTime);

    if (transform.position == lastPosition + direction)
      isMoving = false;
  }

  //======================================

  private void PlayerMovement()
  {
    Vector2 axisMovement = inputHandler.GetMove();
    axisMovement.Normalize();

    if (isMoving)
      return;

    if (axisMovement.sqrMagnitude > 0.5f)
    {
      if (Time.time > lastTime)
      {
        lastTime = Time.time + delayTimeNextButtonPress;
        var cinemachineVirtualDirection = levelManager.CinemachineVirtual.transform.TransformDirection(axisMovement);
        Vector3 direction = new(cinemachineVirtualDirection.x, 0.0f, cinemachineVirtualDirection.z);
        //Vector3 direction = new Vector3(axisMovement.x, 0.0f, axisMovement.y);
        Move(direction);
      }

      return;
    }

    lastTime = Time.time;
  }

  private bool Move(Vector3 parDirection)
  {
    if (levelManager.LevelCompleted || levelManager.IsPause || isMoving || levelManager.GridLevel.TryStatesLevel() || levelManager.IsLevelMenu)
      return false;

    if (Mathf.Abs(parDirection.x) < 0.5f)
      parDirection.x = 0;
    else
      parDirection.z = 0;

    parDirection.Normalize();

    if (IsBlocked(parDirection))
      return false;

    isMoving = true;
    isPlayerRotation = true;

    lastPosition = Vector3Int.CeilToInt(transform.position);
    direction = parDirection;

    levelManager.NumberMoves++;

    GameManager.Instance.ProgressData.TotalNumberMoves++;

    if (_soundMove != null)
      AudioManager.Instance.OnPlaySound?.Invoke(_soundMove);

    animator.SetTrigger("Run");
    return true;
  }

  /// <summary>
  /// Возвращает True, если перед игроком 2 и более ящика, блок который нельзя двигать, и т.д.
  /// </summary>
  /// <param name="parDirection">Направление движения</param>
  private bool IsBlocked(Vector3 parDirection)
  {
    if (!CheckGroundPlayer(parDirection))
      return true;

    if (CheckUnevenBlock(parDirection))
      return true;

    if (Physics.Raycast(transform.position, parDirection, out RaycastHit hit, 1))
    {
      if (hit.collider)
      {
        Block block = hit.collider.GetComponent<Block>();
        if (block.GetTypeObject() == TypeObject.staticObject)
          return true;

        if (block.GetTypeObject() == TypeObject.buttonDoorObject)
          return false;

        #region Проверка движущихся объектов

        if (hit.collider.TryGetComponent(out DynamicObjects dynamicObject))
          return !dynamicObject.ObjectMove(parDirection, _speed);

        #endregion

        #region Проверка шипов

        if (hit.collider.TryGetComponent(out SpikeObject spikeObject))
          return spikeObject.IsSpikeActivated;

        #endregion

        if (block.GetTypeObject() == TypeObject.foodObject)
          return false;
      }

      return true;
    }

    return false;
  }

  private bool CheckGroundPlayer(Vector3 direction)
  {
    // Проверяем позицию клетки впереди игрока
    if (direction.z > 0f && !Physics.Raycast(transform.position + transform.forward, Vector3.down, 1f))
      return false;
    // Проверяем позицию клетки слева от игрока
    else if (direction.x < 0f && !Physics.Raycast(transform.position - transform.right, Vector3.down, 1f))
      return false;
    // Проверяем позицию клетки справа от игрока
    else if (direction.x > 0f && !Physics.Raycast(transform.position + transform.right, Vector3.down, 1f))
      return false;
    // Проверяем позицию клетки позади игрока
    else if (direction.z < 0f && !Physics.Raycast(transform.position - transform.forward, Vector3.down, 1f))
      return false;

    return true;
  }

  /// <summary>
  /// Проверить неровный блок
  /// </summary>
  /// <param name="direction">Направление движения</param>
  private bool CheckUnevenBlock(Vector3 direction)
  {
    if (direction.z > 0f && IsUnevenBlock(transform.position + transform.forward))
      return true;
    else if (direction.x < 0f && IsUnevenBlock(transform.position - transform.right))
      return true;
    else if (direction.x > 0f && IsUnevenBlock(transform.position + transform.right))
      return true;
    else if (direction.z < 0f && IsUnevenBlock(transform.position - transform.forward))
      return true;

    return false;
  }

  /// <summary>
  /// True, если перед игроком неровный блок
  /// </summary>
  private bool IsUnevenBlock(Vector3 origin)
  {
    if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 1f))
    {
      if (hit.collider.TryGetComponent(out DynamicObjects dynamicObject))
      {
        return dynamicObject.IsFalling;
      }
    }

    return false;
  }

  #region Поворот камеры

  private void CameraRotation(float parValue)
  {
    if (levelManager.LevelCompleted || levelManager.IsPause || levelManager.GridLevel.TryStatesLevel() || levelManager.IsLevelMenu)
      return;

    targetRotation += parValue;
    isCameraRotation = true;

    /*if (!isCameraRotation)
    {
      // Вычисляем новый угол поворота камеры
      targetRotation = levelManager.CinemachineVirtual.transform.rotation.eulerAngles.y + parValue;
      isCameraRotation = true;
    }*/
  }

  private void SmoothCameraRotation()
  {
    if (!isCameraRotation)
      return;

    Quaternion currentRotation = levelManager.CinemachineVirtual.transform.rotation;
    Quaternion targetQuaternion = Quaternion.Euler(48.0f, targetRotation, 0.0f);
    Quaternion newRotation = Quaternion.Slerp(currentRotation, targetQuaternion, 3f * Time.deltaTime);

    levelManager.CinemachineVirtual.transform.rotation = newRotation;

    // Проверяем, достигли ли нужного угла поворота
    if (Quaternion.Angle(currentRotation, targetQuaternion) < 0.01f)
    {
      isCameraRotation = false;
      levelManager.CinemachineVirtual.transform.rotation = targetQuaternion;
    }
  }

  #endregion

  /// <summary>
  /// Плавный поворот игрока
  /// </summary>
  private void SmoothPlayerRotation()
  {
    if (!isPlayerRotation)
      return;

    Quaternion rotation = Quaternion.LookRotation(direction);
    _meshTransform.rotation = Quaternion.Lerp(_meshTransform.rotation, rotation, _speedRotaion * Time.deltaTime);

    if (_meshTransform.rotation == rotation)
      isPlayerRotation = false;
  }

  /// <summary>
  /// Можно ли двигаться
  /// </summary>
  private void PossibleMove(bool parValue)
  {
    isPossibleMove = !parValue;
  }

  //======================================

  public override void RemoveRigidbody()
  {
    //Destroy(rigidbody);
  }

  //======================================
}