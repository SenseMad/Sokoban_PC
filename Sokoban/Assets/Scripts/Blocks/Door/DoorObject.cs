using UnityEngine;

public class DoorObject : StaticObjects
{
  [SerializeField] private DoorColor _doorColor;

  [SerializeField] private GameObject _meshGameObject;

  //======================================

  public DoorColor GetDoorColor() => _doorColor;

  public GameObject MeshGameObject { get => _meshGameObject; private set => _meshGameObject = value; }

  //======================================

  protected override void Awake()
  {
    base.Awake();

    MeshGameObject.SetActive(true);
  }

  //======================================
}