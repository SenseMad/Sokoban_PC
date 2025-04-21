using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sokoban.UI
{
  public abstract class SpinBoxBase : MonoBehaviour
  {
    [SerializeField] protected TextMeshProUGUI _fieldNameText;
    [SerializeField] protected Image _leftArrow;
    [SerializeField] protected Image _rightArrow;

    [SerializeField] private bool _enableLeft;
    [SerializeField] private bool _enableRight;
    [SerializeField] private bool isSelected;

    //--------------------------------------

    private InputHandler inputHandler;

    private readonly float timeMoveNextValue = 0.2f;
    private float nextTimeMoveNextValue = 0.0f;

    //======================================

    public bool EnableLeft
    {
      get => _enableLeft;
      set
      {
        _enableLeft = value;
        _leftArrow.color = _enableLeft ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
      }
    }

    public bool EnableRight
    {
      get => _enableRight;
      set
      {
        _enableRight = value;
        _rightArrow.color = _enableRight ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
      }
    }

    public bool IsSelected
    {
      get => isSelected;
      set
      {
        isSelected = value;
        if (isSelected) { OnSelected(); }
        else { OnDeselected(); }
      }
    }

    //======================================

    protected virtual void Awake()
    {
      inputHandler = InputHandler.Instance;
    }

    protected virtual void OnDisable()
    {
      //IsSelected = false;
    }

    protected virtual void Update()
    {
      ChangeValue();
    }

    //======================================

    /// <summary>
    /// »зменить значение
    /// </summary>
    private void ChangeValue()
    {
      if (_leftArrow == null && _rightArrow == null) { return; }
      if (!IsSelected) { return; }

      if (Time.time > nextTimeMoveNextValue)
      {
        nextTimeMoveNextValue = Time.time + timeMoveNextValue;

        if (inputHandler.GetChangingValuesInput() < 0)
        {
          if (_enableLeft)
          {
            OnLeft();
          }
        }

        if (inputHandler.GetChangingValuesInput() > 0)
        {
          if (_enableRight)
          {
            OnRight();
          }
        }
      }

      if (inputHandler.GetChangingValuesInput() == 0)
      {
        nextTimeMoveNextValue = Time.time;
      }
    }

    //======================================

    protected abstract void OnLeft();
    protected abstract void OnRight();

    protected virtual void OnSelected()
    {
      _fieldNameText.color = ColorsGame.SELECTED_COLOR;

      if (_leftArrow == null && _rightArrow == null) { return; }
      _leftArrow.color = _enableLeft ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
      _rightArrow.color = _enableRight ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
    }

    protected virtual void OnDeselected()
    {
      _fieldNameText.color = ColorsGame.STANDART_COLOR;

      if (_leftArrow == null && _rightArrow == null) { return; }
      _leftArrow.color = _enableLeft ? ColorsGame.STANDART_COLOR : ColorsGame.DISABLE_COLOR;
      _rightArrow.color = _enableRight ? ColorsGame.STANDART_COLOR : ColorsGame.DISABLE_COLOR;
    }

    //======================================
  }
}