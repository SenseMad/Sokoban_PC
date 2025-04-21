using UnityEngine;

public abstract class SingletonInSceneNoInstance<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;

  //===========================================

  public static T Instance
  {
    get
    {
      return _instance ?? FindObjectOfType<T>();
    }
  }

  //===========================================

  protected void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this);
      return;
    }

    _instance = GetComponent<T>();
  }

  //===========================================
}