using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using Sokoban.GameManagement;

namespace Sokoban.UI
{
  public abstract class MenuUI : MonoBehaviour
  {
    [SerializeField] protected bool _menuCannotClosed = false;

    [SerializeField] protected List<Button> _listButtons;

    //--------------------------------------

    protected InputHandler inputHandler;

    protected PanelController panelController;

    protected AudioManager audioManager;

    private bool isSelectedButton;

    protected int indexActiveButton;

    protected readonly float timeMoveNextValue = 0.2f;
    protected float nextTimeMoveNextValue = 0.0f;

    //======================================

    public bool IsSelectedButton
    {
      get => isSelectedButton;
      set
      {
        isSelectedButton = value;
        if (isSelectedButton)
          OnSelected();
        else
          OnDeselected();
      }
    }

    //======================================

    protected virtual void Awake()
    {
      inputHandler = InputHandler.Instance;

      panelController = PanelController.Instance;

      audioManager = AudioManager.Instance;
    }

    protected virtual void OnEnable()
    {
      inputHandler.AI_Player.UI.Select.performed += Select_performed;
      inputHandler.AI_Player.UI.Pause.performed += OnCloseMenu;

      IsSelectedButton = true;
    }

    protected virtual void OnDisable()
    {
      inputHandler.AI_Player.UI.Select.performed -= Select_performed;
      inputHandler.AI_Player.UI.Pause.performed -= OnCloseMenu;

      IsSelectedButton = false;
    }

    protected virtual void Update()
    {
      MoveMenuVertically(1);
    }

    //======================================

    #region Закрыть меню

    protected virtual void CloseMenu()
    {
      if (_menuCannotClosed)
        return;

      Sound();
      panelController.ClosePanel();
    }

    protected void CloseMenuNoSound()
    {
      if (_menuCannotClosed)
        return;

      panelController.ClosePanel();
    }

    private void OnCloseMenu(InputAction.CallbackContext context)
    {
      CloseMenu();
    }

    #endregion

    #region Выбор кнопки

    protected virtual void ButtonClick()
    {
      if (_listButtons.Count == 0)
        return;

      _listButtons[indexActiveButton].onClick?.Invoke();
      Sound();
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
      ButtonClick();
    }

    #endregion

    #region Звуки

    protected void Sound()
    {
      audioManager.OnPlaySoundInterface?.Invoke();
    }

    #endregion

    protected virtual void MoveMenuVertically(int parValue)
    {
      if (_listButtons.Count == 0)
        return;

      if (Time.time > nextTimeMoveNextValue)
      {
        nextTimeMoveNextValue = Time.time + timeMoveNextValue;

        if (inputHandler.GetNavigationInput() > 0)
        {
          IsSelectedButton = false;

          indexActiveButton -= parValue;

          if (indexActiveButton < 0) indexActiveButton = _listButtons.Count - 1;

          Sound();
          IsSelectedButton = true;
        }

        if (inputHandler.GetNavigationInput() < 0)
        {
          IsSelectedButton = false;

          indexActiveButton += parValue;

          if (indexActiveButton > _listButtons.Count - 1) indexActiveButton = 0;

          Sound();
          IsSelectedButton = true;
        }
      }

      if (inputHandler.GetNavigationInput() == 0)
      {
        nextTimeMoveNextValue = Time.time;
      }
    }

    protected virtual void MoveMenuHorizontally()
    {
      if (_listButtons.Count == 0)
        return;

      if (Time.time > nextTimeMoveNextValue)
      {
        nextTimeMoveNextValue = Time.time + timeMoveNextValue;

        if (inputHandler.GetChangingValuesInput() > 0)
        {
          IsSelectedButton = false;

          indexActiveButton++;

          if (indexActiveButton > _listButtons.Count - 1) indexActiveButton = 0;

          Sound();
          IsSelectedButton = true;
        }

        if (inputHandler.GetChangingValuesInput() < 0)
        {
          IsSelectedButton = false;

          indexActiveButton--;

          if (indexActiveButton < 0) indexActiveButton = _listButtons.Count - 1;

          Sound();
          IsSelectedButton = true;
        }
      }

      if (inputHandler.GetChangingValuesInput() == 0)
      {
        nextTimeMoveNextValue = Time.time;
      }
    }

    //======================================

    protected virtual void OnSelected()
    {
      if (_listButtons.Count == 0)
        return;
      
      var listButtons = _listButtons[indexActiveButton];

      if (listButtons.TryGetComponent(out Animator parAnimator))
        parAnimator.SetTrigger("Play");
      /*var rectTransform = listButtons.GetComponent<RectTransform>();
      rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);*/

      /*var button = listButtons.GetComponentInChildren<TextMeshProUGUI>();
      if (button != null)
        button.color = ColorsGame.SELECTED_COLOR;*/
      var buttons = listButtons.GetComponentsInChildren<TextMeshProUGUI>(true);
      if (buttons != null)
      {
        foreach (var button in buttons)
        {
          button.color = ColorsGame.SELECTED_COLOR;
        }
      }

      /*if (listButtons.TryGetComponent<Image>(out var image))
        image.enabled = true;*/
    }

    protected virtual void OnDeselected()
    {
      if (_listButtons.Count == 0)
        return;

      var listButtons = _listButtons[indexActiveButton];

      /*var rectTransform = listButtons.GetComponent<RectTransform>();
      rectTransform.localScale = new Vector3(1, 1, 1);*/

      /*var button = listButtons.GetComponentInChildren<TextMeshProUGUI>();
      if (button != null)
        button.color = ColorsGame.STANDART_COLOR;*/
      var buttons = listButtons.GetComponentsInChildren<TextMeshProUGUI>(true);
      if (buttons != null)
      {
        foreach (var button in buttons)
        {
          button.color = ColorsGame.STANDART_COLOR;
        }
      }


      /*if (listButtons.TryGetComponent<Image>(out var image))
        image.enabled = false;*/
    }

    //======================================
  }
}