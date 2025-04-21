using UnityEngine;

using Sokoban.GameManagement;
using Sokoban.LevelManagement;

public class DynamicObjects : Block
{
  private bool isMoving = false;

  private float speed = 2.0f;

  private Vector3Int lastPosition;
  private Vector3 direction;
  
  private Vector3Int positionAfterFall;

  //--------------------------------------

  public Rigidbody Rigidbody { get; private set; }

  public bool IsFalling { get; private set; }

  //======================================

  protected override void Awake()
  {
    base.Awake();

    Rigidbody = GetComponent<Rigidbody>();
  }

  private void Start()
  {
    lastPosition = Vector3Int.CeilToInt(transform.position);
    direction = transform.position;

    if (Sokoban.GridEditor.GridEditor.GridEditorEnabled)
      RemoveRigidbody();
  }

  private void Update()
  {
    if (IsFalling)
    {
      if (transform.position == positionAfterFall)
      {
        Rigidbody.useGravity = false;
        IsFalling = false;
      }
    }

    if (!isMoving)
      return;

    transform.position = Vector3.MoveTowards(transform.position, lastPosition + direction, speed * Time.deltaTime);

    if (transform.position == lastPosition + direction)
      isMoving = false;
  }

  //======================================

  public bool ObjectMove(Vector3 parDirection, float parSpeed)
  {
    if (IsBlocked(parDirection))
      return false;

    if (isMoving)
      return false;

    isMoving = true;
    lastPosition = Vector3Int.CeilToInt(transform.position);
    direction = parDirection;
    speed = parSpeed;
    GameManager.Instance.ProgressData.TotalNumberMovesBox++;
    IsEmptiness();
    return true;
  }

  /// <summary>
  /// True, если движение объекта вперед заблокировано
  /// </summary>
  /// <param name="direction">Направление движения</param>
  private bool IsBlocked(Vector3 direction)
  {
    if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1))
    {
      if (hit.collider)
      {
        if (hit.collider.TryGetComponent<DecoreObject>(out var decoreObject))
          return decoreObject.IsEnableBoxCollider;

        if (hit.collider.GetComponent<DynamicObjects>() || hit.collider.GetComponent<StaticObjects>())
          return true;

        if (hit.collider.TryGetComponent(out SpikeObject spikeObject))
          return spikeObject.IsSpikeActivated;

        if (hit.collider.GetComponent<ButtonDoorObject>())
          return false;
      }
    }

    return false;
  }

  private void IsEmptiness()
  {
    var gridLevel = LevelManager.Instance.GridLevel;
    Vector3Int tempNextPosition = new((int)(lastPosition + direction).x, (int)(lastPosition + direction).y - 1, (int)(lastPosition + direction).z);

    if (!gridLevel.listAllAvailableCells.Contains(tempNextPosition))
    {
      gridLevel.listEmptyCells.Add(tempNextPosition);
      gridLevel.listAllAvailableCells.Add(tempNextPosition);
    }

    if (gridLevel.listEmptyCells.Contains(tempNextPosition))
    {
      Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;

      IsFalling = true;
      Rigidbody.useGravity = true;
      positionAfterFall = tempNextPosition;

      gridLevel.listEmptyCells.Remove(tempNextPosition);
    }
  }

  /// <summary>
  /// True, если объект падает
  /// </summary>
  /*public bool IsObjectFalling()
  {
    return rigidbody.velocity.y < -0.1f;
  }*/

  //======================================

  public override void RemoveRigidbody()
  {
    Destroy(Rigidbody);
  }

  //======================================
}