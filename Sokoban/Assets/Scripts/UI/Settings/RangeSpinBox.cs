using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Sokoban.UI
{
  public class RangeSpinBox : SpinBoxBase
  {
    [System.Serializable]
    private class RangeSpinBoxEvent : UnityEvent<int> { }

    [SerializeField] private TextMeshProUGUI _valueText;

    [SerializeField] private int _value;
    [SerializeField] private int _minValue;
    [SerializeField] private int _maxValue;

    [Space, SerializeField]
    private RangeSpinBoxEvent _onValueChanged;

    //======================================

    public int Value
    {
      get => _value;
      set
      {
        SetValue(value, true);
      }
    }

    public event UnityAction<int> OnValueChanged
    {
      add { _onValueChanged.AddListener(value); }
      remove { _onValueChanged.RemoveListener(value); }
    }

    //======================================

    protected override void Awake()
    {
      base.Awake();

      _valueText.text = $"{_value}";

      EnableLeft = _value > _minValue;
      EnableRight = _value < _maxValue;
    }

    //======================================

    protected override void OnSelected()
    {
      base.OnSelected();
      _valueText.color = ColorsGame.SELECTED_COLOR;
    }

    protected override void OnDeselected()
    {
      base.OnDeselected();
      _valueText.color = ColorsGame.STANDART_COLOR;
    }

    private void SetValue(int parValue, bool parNotify)
    {
      parValue = Mathf.Clamp(parValue, _minValue, _maxValue);
      if (parValue == _value) { return; }
      _value = parValue;

      _valueText.text = $"{_value}";

      EnableLeft = _value > _minValue;
      EnableRight = _value < _maxValue;

      if (parNotify)
      {
        _onValueChanged?.Invoke(_value);
      }
    }

    public void SetValueWithoutNotify(int parValue)
    {
      SetValue(parValue, false);
    }

    protected override void OnLeft()
    {
      SetValue(_value - 5, true);
    }

    protected override void OnRight()
    {
      SetValue(_value + 5, true);
    }

    //======================================
  }
}