using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sokoban.LevelManagement;
using Sokoban.GameManagement;

namespace Sokoban.UI
{
  public class UILocationSelectMenu : MenuUI
  {
    [SerializeField] private RectTransform _locationSelectPanel;

    [Space(10)]
    [SerializeField] private UILocationSelectButton _prefabButtonLocationSelect;

    [Space(10)]
    [SerializeField] private Panel _panelLevelSelection;

    [Space(10)]
    [SerializeField] private List<LocationData> _locationDatas;

    //--------------------------------------

    private GameManager gameManager;

    private UILevelSelectMenu uILevelSelectMenu;

    private List<UILocationSelectButton> listUILocationSelectButton = new List<UILocationSelectButton>();

    //======================================

    protected override void Awake()
    {
      base.Awake();

      gameManager = GameManager.Instance;

      uILevelSelectMenu = _panelLevelSelection.GetComponent<UILevelSelectMenu>();
    }

    protected override void OnEnable()
    {
      DisplayLocationSelectionButtonsUI();

      base.OnEnable();
    }

    protected override void OnDisable()
    {
      base.OnDisable();

      ClearButtonsUI();
    }

    protected override void Update()
    {
      MoveMenuHorizontally();
    }

    //======================================

    protected override void MoveMenuHorizontally()
    {
      if (_listButtons.Count == 0)
        return;

      if (Time.time > nextTimeMoveNextValue)
      {
        nextTimeMoveNextValue = Time.time + timeMoveNextValue;

        if (inputHandler.GetChangingValuesInput() > 0)
        {
          IsSelectedButton = false;

          listUILocationSelectButton[indexActiveButton].ChangeSprite(false);

          indexActiveButton++;

          if (indexActiveButton > _listButtons.Count - 1) indexActiveButton = 0;
          if (!gameManager.ProgressData.IsLocationOpen(listUILocationSelectButton[indexActiveButton].Location))
            indexActiveButton = 0;

          listUILocationSelectButton[indexActiveButton].ChangeSprite(true);

          Sound();
          IsSelectedButton = true;
        }

        if (inputHandler.GetChangingValuesInput() < 0)
        {
          IsSelectedButton = false;

          listUILocationSelectButton[indexActiveButton].ChangeSprite(false);

          indexActiveButton--;

          if (indexActiveButton < 0) indexActiveButton = _listButtons.Count - 1;
          while (!gameManager.ProgressData.IsLocationOpen(listUILocationSelectButton[indexActiveButton].Location))
            indexActiveButton--;

          listUILocationSelectButton[indexActiveButton].ChangeSprite(true);

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

    /*private void DisplayLocationSelectionButtonsUI()
    {
      if (_listButtons.Count != 0)
        return;

      indexActiveButton = -1;

      foreach (var location in Levels.GetListLocation())
      {
        if (!Levels.GetLocationTable(location))
          continue;

        UILocationSelectButton button = Instantiate(_prefabButtonLocationSelect, _locationSelectPanel);

        button.Button = button.GetComponent<Button>();

        if (gameManager.ProgressData.IsLocationOpen(location))
        {
          button.ChangeColor();
          button.Button.interactable = true;
          button.Button.onClick.AddListener(() => SelectLocation(location));
          button.ChangeTextNumberLevels($"{gameManager.ProgressData.GetNumberLevelsCompleted(location)}/{Levels.GetNumberLevelsLocation(location)}");
          indexActiveButton++;
        }

        listUILocationSelectButton.Add(button);
        _listButtons.Add(button.GetComponent<Button>());
        button.Initialize(location);
      }

      listUILocationSelectButton[indexActiveButton].ChangeSprite(true);
    }*/

    private void DisplayLocationSelectionButtonsUI()
    {
      if (_listButtons.Count != 0)
        return;

      indexActiveButton = -1;

      foreach (var location in _locationDatas)
      {
        if (!Levels.GetLocationTable(location.Location))
          continue;

        UILocationSelectButton button = Instantiate(_prefabButtonLocationSelect, _locationSelectPanel);

        button.Button = button.GetComponent<Button>();

        if (gameManager.ProgressData.IsLocationOpen(location.Location))
        {
          button.ChangeColor();
          button.Button.interactable = true;
          button.Button.onClick.AddListener(() => SelectLocation(location.Location));
          button.ChangeTextNumberLevels($"{gameManager.ProgressData.GetNumberLevelsCompleted(location.Location)}/{Levels.GetNumberLevelsLocation(location.Location)}");
          indexActiveButton++;
        }

        listUILocationSelectButton.Add(button);
        _listButtons.Add(button.GetComponent<Button>());
        button.Initialize(location);
      }

      listUILocationSelectButton[indexActiveButton].ChangeSprite(true);
    }

    public void ClearButtonsUI()
    {
      for (int i = 0; i < listUILocationSelectButton.Count; i++)
      {
        Destroy(listUILocationSelectButton[i].gameObject);
      }

      listUILocationSelectButton = new List<UILocationSelectButton>();
      _listButtons = new List<Button>();
    }

    protected override void OnSelected()
    {
      if (_listButtons.Count == 0)
        return;

      var listButtons = _listButtons[indexActiveButton];

      var rectTransform = listButtons.GetComponent<RectTransform>();
      rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);
    }

    protected override void OnDeselected()
    {
      if (_listButtons.Count == 0)
        return;

      var listButtons = _listButtons[indexActiveButton];

      var rectTransform = listButtons.GetComponent<RectTransform>();
      rectTransform.localScale = new Vector3(1, 1, 1);
    }

    //======================================

    private void SelectLocation(Location parLocation)
    {
      panelController.SetActivePanel(_panelLevelSelection);

      uILevelSelectMenu.DisplayLevelSelectionButtonsUI(parLocation);
    }

    //======================================
  }
}