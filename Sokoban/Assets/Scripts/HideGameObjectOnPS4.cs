using UnityEngine;

namespace Sokoban.GameManagement
{
  public sealed class HideGameObjectOnPS4 : MonoBehaviour
  {
#if UNITY_PS4
    private void OnEnable()
    {
      gameObject.SetActive(false);
    }
#endif
  }
}