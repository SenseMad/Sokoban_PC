using System.Collections.Generic;
using UnityEngine;

public class ShopData : SingletonInSceneNoInstance<ShopData>
{
  [SerializeField] private List<SkinData> _skinDatas = new();

  //======================================

  public List<SkinData> SkinDatas { get => _skinDatas; private set => _skinDatas = value; }

  //======================================
}