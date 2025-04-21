using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjects : Block
{
  [SerializeField] private AudioClip _sound;

  //======================================

  public AudioClip Sound => _sound;

  //======================================
}