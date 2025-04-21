using UnityEngine;

public abstract class SingletonInGame<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;

  //===========================================

  public static T Instance
  {
    get
    {
      if (_instance == null)
      {
        var singletonObject = new GameObject($"{typeof(T)}");
        _instance = singletonObject.AddComponent<T>();
        DontDestroyOnLoad(singletonObject);
      }

      return _instance;
    }
  }

  //===========================================

  protected virtual void Awake()
  {
    if (_instance == null)
    {
      _instance = GetComponent<T>();
      //DontDestroyOnLoad(gameObject);

      if (transform.parent == null)
      {
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Debug.LogWarning($"{typeof(T)}: объект не является корневым, DontDestroyOnLoad может не сработать корректно.");
      }
      return;
    }

    Destroy(this);
  }

  //===========================================
}