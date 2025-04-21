using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban.LevelManagement
{
  /// <summary>
  /// Поведение на уровне
  /// </summary>
  public abstract class LevelBehaviour : SingletonInSceneNoInstance<LevelBehaviour>
  {
    #region UNITY

    protected virtual new void Awake()
    {
      base.Awake();
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void LateUpdate() { }

    #endregion

    //======================================

    /// <summary>
    /// Возвращает данные уровня
    /// </summary>
    public abstract LevelData GetCurrentLevelData();

    /// <summary>
    /// Возвращает прогресс на уровне
    /// </summary>
    public abstract LevelProgressData GetCurrentLevelProgressData();

    //======================================

    public abstract bool IsFoodCollected();

    /// <summary>
    /// Добавить коробку в список
    /// </summary>
    /// <param name="box"></param>
   // public abstract void AddBoxToList(Box box);

    //======================================
  }
}