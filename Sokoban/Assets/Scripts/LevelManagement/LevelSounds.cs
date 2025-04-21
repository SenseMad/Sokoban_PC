using UnityEngine;

namespace Sokoban.LevelManagement
{
  public class LevelSounds : MonoBehaviour
  {
    [SerializeField] private AudioClip _levelComplete;

    //======================================

    public AudioClip LevelComplete => _levelComplete;

    //======================================

    private void PlaySound(AudioClip parAudioClip)
    {
      //PlaySound(parAudioClip);
    }

    //======================================
  }
}