using System.Collections.Generic;
using UnityEngine;

namespace Sokoban.UI
{
  public class PanelController : SingletonInSceneNoInstance<PanelController>
  {
    [SerializeField] private Panel _currentActivePanel;

    [SerializeField] private GameObject _topPanelObject;

    //--------------------------------------

    public List<Panel> listAllOpenPanels = new();

    //======================================

    public Panel GetCurrentActivePanel() => _currentActivePanel;

    //======================================

    public void ShowPanel(Panel parPanel)
    {
      _currentActivePanel = parPanel;

      if (_currentActivePanel == null)
        return;

      _topPanelObject.SetActive(true);
      _currentActivePanel.ShowPanel();
      listAllOpenPanels.Add(parPanel);
    }

    public void HidePanel(Panel parPanel)
    {
      if (parPanel == null)
        return;

      parPanel.HidePanel();
    }

    public void SetActivePanel(Panel parPanel)
    {
      HidePanel(_currentActivePanel);

      ShowPanel(parPanel);
    }

    //======================================

    public void ClosePanel()
    {
      if (listAllOpenPanels.Count == 0 || _currentActivePanel == null)
        return;

      if (_currentActivePanel.IsCloseWindow)
        return;

      HidePanel(_currentActivePanel);
      listAllOpenPanels.Remove(_currentActivePanel);

      if (listAllOpenPanels.Count == 0)
      {
        _currentActivePanel = null;
        _topPanelObject.SetActive(false);
        return;
      }

      _currentActivePanel = listAllOpenPanels[listAllOpenPanels.Count - 1];
      _currentActivePanel.ShowPanel();
    }

    public void CloseAllPanels()
    {
      for (int i = 0; i < listAllOpenPanels.Count; i++)
      {
        ClosePanel();
      }
    }

    /// <summary>
    /// Закрыть все панели
    /// </summary>
    public void CloseAllPanels1()
    {
      for (int i = 0; i < listAllOpenPanels.Count; i++)
      {
        HidePanel(listAllOpenPanels[i]);
        listAllOpenPanels.Remove(listAllOpenPanels[i]);
      }

      _topPanelObject.SetActive(false);

      _currentActivePanel = null;
      listAllOpenPanels.Clear();
    }

    //======================================
  }
}