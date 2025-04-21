using UnityEngine;

using Sokoban.GameManagement;
using Sokoban.LevelManagement;

namespace Sokoban.UI
{
  /// <summary>
  /// Главное меню
  /// </summary>
  public class MainMenu : MenuUI
  {
    [SerializeField, Tooltip("Панель главного меню")]
    private Panel _mainMenu;

    //======================================

    private GameManager gameManager;
    private LevelManager levelManager;

    //======================================

    protected override void Awake()
    {
      base.Awake();

      gameManager = GameManager.Instance;
      levelManager = LevelManager.Instance;
    }

    protected override void OnEnable()
    {
      indexActiveButton = 0;

      base.OnEnable();
    }

    //======================================

    public void ExitGame()
    {
      Application.Quit();
    }

    //======================================
  }
}