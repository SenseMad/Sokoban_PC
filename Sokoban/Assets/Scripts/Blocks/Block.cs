using UnityEngine;

public abstract class Block : MonoBehaviour
{
  [SerializeField, Tooltip("Тип объекта")]
  private TypeObject _typeObject;

  [SerializeField, Tooltip("Индекс объекта")]
  private int _indexObject;

  [SerializeField, Tooltip("Позиция объекта")]
  private Vector3Int _objectPosition;

  [Header("UI")]
  [SerializeField, Tooltip("Название объекта")]
  private string _nameObject;
  [SerializeField, Tooltip("Спрайт объекта")]
  private Sprite _spriteObject;

  //--------------------------------------

  public BoxCollider BoxCollider { get; set; }

  //======================================

  protected virtual void Awake()
  {
    BoxCollider = GetComponent<BoxCollider>();
  }

  //======================================

  /// <summary>
  /// Получить тип объекта
  /// </summary>
  public TypeObject GetTypeObject() => _typeObject;

  /// <summary>
  /// Получить индекс объекта
  /// </summary>
  public int GetIndexObject() => _indexObject;

  /// <summary>
  /// Получить позицию объекта
  /// </summary>
  public Vector3Int GetObjectPosition() => _objectPosition;

  public string NameObject => _nameObject;

  /// <summary>
  /// Получить спрайт объекта
  /// </summary>
  public Sprite GetSpriteObject() => _spriteObject;

  /// <summary>
  /// Получить BoxCollider
  /// </summary>
  public BoxCollider GetBoxCollider() => BoxCollider;

  //======================================

  /// <summary>
  /// Установить позицию объекта
  /// </summary>
  public void SetPositionObject(Vector3Int parObjectPosition)
  {
    _objectPosition = parObjectPosition;
  }

  /// <summary>
  /// Удалить Rigidbody у объектов которые могут падать
  /// </summary>
  public virtual void RemoveRigidbody() { }

  //======================================
}