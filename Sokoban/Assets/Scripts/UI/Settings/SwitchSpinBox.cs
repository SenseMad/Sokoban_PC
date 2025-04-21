using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Sokoban.UI
{
  public class SwitchSpinBox : SpinBoxBase
  {
    [System.Serializable]
    private class SwitchSpinBoxEvent : UnityEvent<int> { }

    [SerializeField] private TextMeshProUGUI _valueText;

    [SerializeField] private int _value;

    [Space, SerializeField]
    private SwitchSpinBoxEvent _onValueChanged;

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

      EnableLeft = true;
      EnableRight = true;
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
      //if (parValue == _value) { return; }
      _value = parValue;

      if (parNotify)
      {
        _onValueChanged?.Invoke(_value);
      }
    }

    public void UpdateText(string parValue)
    {
      _valueText.text = $"{parValue}";
    }

    public void SetValueWithoutNotify(int parValue)
    {
      SetValue(parValue, false);
    }

    //======================================

    protected override void OnLeft()
    {
      SetValue(_value - 1, true);
    }

    protected override void OnRight()
    {
      SetValue(_value + 1, true);
    }

    //======================================
  }
}