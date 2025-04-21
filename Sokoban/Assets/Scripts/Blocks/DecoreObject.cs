using UnityEngine;

public class DecoreObject : Block
{
  [SerializeField] private bool _isEnableBoxCollider;

  //======================================

  public bool IsEnableBoxCollider => _isEnableBoxCollider;

  //======================================
}