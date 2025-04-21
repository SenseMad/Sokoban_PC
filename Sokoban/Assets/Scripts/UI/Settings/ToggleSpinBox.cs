using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Sokoban.UI
{
  public class ToggleSpinBox : SpinBoxBase
  {
    [System.Serializable]
    private class ToggleSpinBoxEvent : UnityEvent<bool> { }

    [SerializeField] private TextMeshProUGUI _yesText;
    [SerializeField] private TextMeshProUGUI _noText;

    [SerializeField] private bool _value;

    [Space, SerializeField]
    private ToggleSpinBoxEvent _onValueChanged;

    //======================================

    public event UnityAction<bool> OnValueChanged
    {
      add { _onValueChanged.AddListener(value); }
      remove { _onValueChanged.RemoveListener(value); }
    }

    //======================================

    protected override void Awake()
    {
      base.Awake();

      _yesText.color = _value ? ColorsGame.STANDART_COLOR : ColorsGame.DISABLE_COLOR;
      _noText.color = _value ? ColorsGame.DISABLE_COLOR : ColorsGame.STANDART_COLOR;

      EnableLeft = true;
      EnableRight = true;
    }

    //======================================

    protected override void OnSelected()
    {
      base.OnSelected();
      _yesText.color = _value ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
      _noText.color = _value ? ColorsGame.DISABLE_COLOR : ColorsGame.SELECTED_COLOR;
    }

    protected override void OnDeselected()
    {
      base.OnDeselected();
      _yesText.color = _value ? ColorsGame.STANDART_COLOR : ColorsGame.DISABLE_COLOR;
      _noText.color = _value ? ColorsGame.DISABLE_COLOR : ColorsGame.STANDART_COLOR;
    }

    private void SetValue(bool parValue, bool parNotify)
    {
      if (parValue == _value) { return; }
      _value = parValue;

      _yesText.color = _value ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
      _noText.color = _value ? ColorsGame.DISABLE_COLOR : ColorsGame.SELECTED_COLOR;

      if (parNotify)
      {
        _onValueChanged?.Invoke(_value);
      }
    }

    public void SetValueWithoutNotify(bool parValue)
    {
      SetValue(parValue, false);
    }

    protected override void OnLeft()
    {
      SetValue(!_value, true);
    }

    protected override void OnRight()
    {
      SetValue(!_value, true);
    }

    //======================================
  }
}