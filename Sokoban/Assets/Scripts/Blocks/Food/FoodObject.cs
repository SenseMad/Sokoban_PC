using System.Collections.Generic;
using UnityEngine;

using Sokoban.LevelManagement;
using Sokoban.GameManagement;

public class FoodObject : InteractiveObjects
{
  [SerializeField] private List<GameObject> _listFoods = new();

  //--------------------------------------

  private Transform meshTransform;

  //======================================

  protected override void Awake()
  {
    base.Awake();

    meshTransform = GetComponentInChildren<MeshFilter>().transform;

    RandomFood();
  }

  private void Update()
  {
    meshTransform.Rotate(new Vector3(0.0f, 30.0f * Time.deltaTime, 0.0f));
  }

  //======================================

  public bool IsFoodCollected { get; private set; }

  //======================================

  private void OnTriggerEnter(Collider other)
  {
    if (!other.GetComponent<PlayerObjects>())
      return;

    IsFoodCollected = true;

    if (Sound != null)
      AudioManager.Instance.OnPlaySound?.Invoke(Sound);

    LevelManager.Instance.IsFoodCollected();

    gameObject.SetActive(false);
  }

  //======================================

  private void RandomFood()
  {
    System.Random random = new();

    GameObject objectFood = _listFoods[random.Next(0, _listFoods.Count)];

    transform.GetComponentInChildren<MeshFilter>().sharedMesh = objectFood.GetComponentInChildren<MeshFilter>().sharedMesh;
    transform.GetComponentInChildren<MeshRenderer>().sharedMaterial = objectFood.GetComponentInChildren<Renderer>().sharedMaterial;
  }

  //======================================
}