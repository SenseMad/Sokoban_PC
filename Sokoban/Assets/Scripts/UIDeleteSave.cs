using UnityEngine;
using UnityEngine.UI;

using Sokoban.GameManagement;

namespace Sokoban.UI
{
  public class UIDeleteSave : MenuUI
  {
    [SerializeField] private Button _yesButton;

    [SerializeField] private Button _noButton;

    //--------------------------------------

    private GameManager gameManager;

    private bool menuClosed = true;

    //======================================

    protected override void Awake()
    {
      base.Awake();

      gameManager = GameManager.Instance;
    }

    private void Start()
    {
      _listButtons.Add(_yesButton);
      _listButtons.Add(_noButton);

      indexActiveButton = 1;
      OnSelected();
    }

    protected override void OnEnable()
    {
      base.OnEnable();

      if (_listButtons != null)
      {
        OnDeselected();
        indexActiveButton = 1;
        OnSelected();
        menuClosed = false;
      }

      _yesButton.onClick.AddListener(() => OnDeleteSave());

      _noButton.onClick.AddListener(() => ClosePanel());
    }

    protected override void OnDisable()
    {
      base.OnDisable();

      _yesButton.onClick.RemoveListener(() => OnDeleteSave());

      _noButton.onClick.RemoveListener(() => ClosePanel());
    }

    protected override void Update()
    {
      MoveMenuHorizontally();
    }

    //======================================

    private void ClosePanel()
    {
      if (menuClosed == true)
        return;

      menuClosed = true;
      CloseMenu();
    }

    private void OnDeleteSave()
    {
      gameManager.ResetAndSaveFile();
      //panelController.ClosePanel();
      panelController.CloseAllPanels();
    }

    //======================================
  }
}