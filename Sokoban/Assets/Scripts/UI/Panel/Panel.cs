using UnityEngine;

namespace Sokoban.UI
{
  public class Panel : MonoBehaviour
  {
    [SerializeField] private GameObject _panelObject;

    [SerializeField] private bool _panelIsShow;

    [Space(10)]
    [SerializeField] private bool _isCloseWindow = false;

    //======================================

    private void Awake()
    {
      if (_panelObject == null)
        _panelObject = gameObject;
    }

    //======================================

    public bool PanelIsShow
    {
      get => _panelIsShow;
      set
      {
        _panelIsShow = value;
        _panelObject.SetActive(value);
      }
    }

    public bool IsCloseWindow => _isCloseWindow;

    //======================================

    public void ShowPanel()
    {
      PanelIsShow = true;
    }

    public void HidePanel()
    {
      PanelIsShow = false;
    }

    //======================================
  }
}