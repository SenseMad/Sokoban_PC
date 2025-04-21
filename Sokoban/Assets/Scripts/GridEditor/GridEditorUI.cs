using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using Sokoban.LevelManagement;

namespace Sokoban.GridEditor
{
  public class GridEditorUI : MonoBehaviour
  {
    [Header("ВЫБОР ТИПОВ БЛОКОВ")]
    [SerializeField, Tooltip("Фильтр типа объекта")]
    private TMP_Dropdown _blockTypeFilter;
    [SerializeField, Tooltip("Префаб кнопки выбора блока")]
    private UIBlockTypeSelectButton _prefabButtonTypeObject;
    [SerializeField, Tooltip("Панель выбора типов блоков")]
    private RectTransform _blockTypeSelectionPanel;

    [Header("ВЫБОР ЛОКАЦИИ")]
    [SerializeField, Tooltip("Выбор локации")]
    private TMP_Dropdown _locationDropdown;

    [Header("НОМЕР УРОВНЯ")]
    [SerializeField, Tooltip("Номера уровней")]
    private TMP_Dropdown _levelNumbersDropdown;

    [Header("НАСТРОЙКИ УРОВНЯ (X Y Z)")]
    [SerializeField, Tooltip("X")]
    private TMP_InputField _inputFieldX;
    [SerializeField, Tooltip("Y")]
    private TMP_InputField _inputFieldY;
    [SerializeField, Tooltip("Z")]
    private TMP_InputField _inputFieldZ;
    [SerializeField, Tooltip("Кнопка подтвердить")]
    private Button _buttonConfirm;

    [Header("УРОВЕНЬ ПОЛЯ")]
    [SerializeField, Tooltip("Кнопка понижения уровня поля")]
    private Button _gridLevelDownButton;
    [SerializeField, Tooltip("Текст уровня поля")]
    private TextMeshProUGUI _gridLevelText;
    [SerializeField, Tooltip("Кнопка повышения уровня поля")]
    private Button _gridLevelUpButton;

    [Header("КНОПКИ")]
    [SerializeField, Tooltip("Кнопка создать")]
    private Button _buttonCreate;
    [SerializeField, Tooltip("Кнопка сохранить")]
    private Button _buttonSave;
    [SerializeField, Tooltip("Кнопка загрузить")]
    private Button _buttonLoad;
    [SerializeField, Tooltip("Кнопка очистить")]
    private Button _buttonClear;

    [Header("TOGGLE")]
    [SerializeField, Tooltip("Режим редактирования")]
    private Toggle _toggleEditMode;
    [SerializeField, Tooltip("Режим удаления")]
    private Toggle _toggleDeleteMode;

    [Header("ТЕКСТ")]
    [SerializeField, Tooltip("Текст локации/номера уровня")]
    private TextMeshProUGUI _textLocationName;

    //--------------------------------------

    private GridEditor gridEditor;

    /// <summary>
    /// Список всех кнопок
    /// </summary>
    private List<UIBlockTypeSelectButton> listBlocks = new();

    //======================================

    /// <summary>
    /// Количество нажатий на кнопку создать
    /// </summary>
    private int numberClickCreateButton;
    /// <summary>
    /// Количество нажатий на кнопку подтвердить
    /// </summary>
    private int numberClickConfirmButton;
    /// <summary>
    /// Количество нажатий на кнопку загрузить
    /// </summary>
    private int numberClickLoadButton;
    /// <summary>
    /// Количество нажатий на кнопку очистить
    /// </summary>
    private int numberClickClearButton;

    //======================================

    private void Awake()
    {
      gridEditor = FindObjectOfType<GridEditor>();
    }

    private void Start()
    {
      Levels.GetFullNumberLevelsLocation();

      AddObjectTypesDropdown();
      AddLocationDropdown();
      AddNumberLevelDropDown(gridEditor.SelectedLocation);

      AddListBlocks();

      UpdateTextFieldSize(gridEditor.GetFieldSize());
      UpdateTextGridLevel(gridEditor.GetGridLevel());

      _textLocationName.text = $"Локация: <color=#10FF00>{gridEditor.SelectedLocation}</color>\nУровень: <color=#10FF00>{gridEditor.SelectedLevelNumber}</color>";

      #region InputField

      _inputFieldX.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar); };
      _inputFieldY.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar); };
      _inputFieldZ.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar); };

      #endregion
    }

    private void OnEnable()
    {
      #region Dropdown

      _blockTypeFilter.onValueChanged.AddListener(GetTypeObjectStringDropdown);
      _locationDropdown.onValueChanged.AddListener(GetLocationStringDropdown);
      _levelNumbersDropdown.onValueChanged.AddListener(GetNumberLevelDropdown);

      #endregion

      #region InputField

      _buttonConfirm.onClick.AddListener(() => UIConfrirm());

      #endregion

      #region Button

      _buttonCreate.onClick.AddListener(() => UICreateLevelData());
      _buttonSave.onClick.AddListener(() => gridEditor.SaveLevelData());
      _buttonLoad.onClick.AddListener(() => UILoadLevelData());
      _buttonClear.onClick.AddListener(() => UIClearLevelData());

      _gridLevelDownButton.onClick.AddListener(() => ChangeGridLevel(false));
      _gridLevelUpButton.onClick.AddListener(() => ChangeGridLevel(true));

      #endregion

      #region Toggle

      _toggleEditMode.onValueChanged.AddListener(ChangeToggleEditMode);
      _toggleDeleteMode.onValueChanged.AddListener(ChangeToggleDeleteMode);

      #endregion
    }

    private void OnDisable()
    {
      #region Dropdown

      _blockTypeFilter.onValueChanged.RemoveListener(GetTypeObjectStringDropdown);
      _locationDropdown.onValueChanged.RemoveListener(GetLocationStringDropdown);
      _levelNumbersDropdown.onValueChanged.RemoveListener(GetNumberLevelDropdown);

      #endregion

      #region InputField

      _buttonConfirm.onClick.AddListener(() => UIConfrirm());

      #endregion

      #region Button

      _buttonCreate.onClick.RemoveListener(() => UICreateLevelData());
      _buttonSave.onClick.RemoveListener(() => gridEditor.SaveLevelData());
      _buttonLoad.onClick.RemoveListener(() => UILoadLevelData());
      _buttonClear.onClick.RemoveListener(() => UIClearLevelData());

      _gridLevelDownButton.onClick.RemoveListener(() => ChangeGridLevel(false));
      _gridLevelUpButton.onClick.RemoveListener(() => ChangeGridLevel(true));

      #endregion

      #region Toggle

      _toggleEditMode.onValueChanged.RemoveListener(ChangeToggleEditMode);
      _toggleDeleteMode.onValueChanged.RemoveListener(ChangeToggleDeleteMode);

      #endregion
    }

    private void OnDestroy()
    {
      #region InputField

      _inputFieldX.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar); };
      _inputFieldY.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar); };
      _inputFieldZ.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar); };

      #endregion
    }

    //======================================

    #region (Dropdown) Фильтр типа объекта

    /// <summary>
    /// Получить русский текст из типов объектов
    /// </summary>
    /// <param name="parTypeObject">Тип объекта</param>
    /// <returns></returns>
    private string GetRussianTextObjectTypes(TypeObject parTypeObject)
    {
      switch (parTypeObject)
      {
        case TypeObject.allObject:
          return "Все объекты";
        case TypeObject.playerObject:
          return "Объекты игрока";
        case TypeObject.dynamicObject:
          return "Объекты движения";
        case TypeObject.staticObject:
          return "Объекты статистические";
        case TypeObject.foodObject:
          return "Объекты еды";
        case TypeObject.spikeObject:
          return "Объекты шипов";
        case TypeObject.doorObject:
          return "Объекты дверей";
        case TypeObject.buttonDoorObject:
          return "Объекты кнопок дверей";
        case TypeObject.decorObject:
          return "Объекты декора";
      }

      return "";
    }

    /// <summary>
    /// Получить типы объектов из русского текста
    /// </summary>
    private TypeObject GetTypeObjectsRussianText(string parText)
    {
      switch(parText)
      {
        case "Все объекты":
          return TypeObject.allObject;
        case "Объекты игрока":
          return TypeObject.playerObject;
        case "Объекты движения":
          return TypeObject.dynamicObject;
        case "Объекты статистические":
          return TypeObject.staticObject;
        case "Объекты еды":
          return TypeObject.foodObject;
        case "Объекты шипов":
          return TypeObject.spikeObject;
        case "Объекты дверей":
          return TypeObject.doorObject;
        case "Объекты кнопок дверей":
          return TypeObject.buttonDoorObject;
        case "Объекты декора":
          return TypeObject.decorObject;
      }

      return TypeObject.allObject;
    }

    /// <summary>
    /// Добавить типы объектов в Dropdown
    /// </summary>
    private void AddObjectTypesDropdown()
    {
      var typeObjects = (TypeObject[])Enum.GetValues(typeof(TypeObject));
      var listTypeObjects = new List<string>();

      foreach (var typeObject in typeObjects)
      {
        listTypeObjects.Add($"{GetRussianTextObjectTypes(typeObject)}");
      }

      _blockTypeFilter.ClearOptions();
      _blockTypeFilter.AddOptions(listTypeObjects);
    }

    /// <summary>
    /// Получить тип объекта из Dropdown
    /// </summary>
    private void GetTypeObjectStringDropdown(int parValue)
    {
      string textTypeObject = _blockTypeFilter.options[parValue].text;

      foreach (var block in listBlocks)
      {
        if (GetTypeObjectsRussianText(textTypeObject) == TypeObject.allObject)
        {
          block.gameObject.SetActive(true);
          continue;
        }

        if (GetTypeObjectsRussianText(textTypeObject) != block.TypeObject)
        {
          block.gameObject.SetActive(false);
          continue;
        }

        block.gameObject.SetActive(true);
      }
    }

    #endregion

    #region Создание кнопок выбора объектов

    private void AddListBlocks()
    {
      foreach (var block in gridEditor.GetListBlockObjectsTypes().GetListAllBlock())
      {
        UIBlockTypeSelectButton button = Instantiate(_prefabButtonTypeObject, _blockTypeSelectionPanel);
        button.Button = button.GetComponent<Button>();

        button.Button.onClick.AddListener(() => SelectObject(block, button));
        listBlocks.Add(button);

        button.InitializeButton(block.GetTypeObject(), block.GetSpriteObject(), $"{block.GetIndexObject()}");
      }
    }

    /// <summary>
    /// Выбрать объект
    /// </summary>
    /// <param name="parTypeObject">Тип объекта</param>
    /// <param name="parIndexObject">Индекс объекта</param>
    private void SelectObject(Block parBlock, UIBlockTypeSelectButton parUIBlockTypeSelectButton)
    {
      gridEditor.TypeSelectedObject = parBlock.GetTypeObject();
      gridEditor.IndexSelectedObject = parBlock.GetIndexObject();

      parUIBlockTypeSelectButton.ChangeColor(true);
      foreach (var block in listBlocks)
      {
        if (block == parUIBlockTypeSelectButton)
          continue;

        block.ChangeColor(false);
      }
    }

    #endregion

    #region (Dropdown) Выбор локации

    /// <summary>
    /// Добавить Локации в Dropdown
    /// </summary>
    private void AddLocationDropdown()
    {
      var locations = GetLocation.GetNamesAllLocation();
      var listLocation = new List<string>();

      foreach (var location in locations)
      {
        listLocation.Add($"{location}");
      }

      _locationDropdown.ClearOptions();
      _locationDropdown.AddOptions(listLocation);
    }

    /// <summary>
    /// Получить название локации из Dropdown
    /// </summary>
    private void GetLocationStringDropdown(int parValue)
    {
      string textLocation = _locationDropdown.options[parValue].text;

      if (!Enum.TryParse(textLocation, out Location location))
        return;

      gridEditor.SelectedLocation = location;

      /*if (!Levels.GetLocationTable(location))
        return;*/

      AddNumberLevelDropDown(location);

      _textLocationName.text = $"Локация: <color=#10FF00>{gridEditor.SelectedLocation}</color>\nУровень: <color=#10FF00>{gridEditor.SelectedLevelNumber}</color>";
    }

    #endregion

    #region (Dropdown) Выбор номер уровня

    /// <summary>
    /// Добавить номера уровней в Dropdown
    /// </summary>
    private void AddNumberLevelDropDown(Location parLocation)
    {
      int num = Levels.GetNumberLevelsLocation(parLocation);
      var listNumbers = new List<string>();

      for (int i = 0; i < num; i++)
      {
        listNumbers.Add($"{i + 1}");
      }

      gridEditor.SelectedLevelNumber = 1;
      _levelNumbersDropdown.ClearOptions();
      _levelNumbersDropdown.AddOptions(listNumbers);
    }

    /// <summary>
    /// Получить номер уровня из Dropdown
    /// </summary>
    private void GetNumberLevelDropdown(int parValue)
    {
      string textNumberLevel = _levelNumbersDropdown.options[parValue].text;

      gridEditor.SelectedLevelNumber = int.Parse(textNumberLevel);

      _textLocationName.text = $"Локация: <color=#10FF00>{gridEditor.SelectedLocation}</color>\nУровень: <color=#10FF00>{gridEditor.SelectedLevelNumber}</color>";
    }

    #endregion

    #region Размеры поля

    /// <summary>
    /// Проверка на символы (Цифры)
    /// </summary>
    private char ValidateInput(char addedChar)
    {
      if (char.IsDigit(addedChar))
        return addedChar;
      else
        return '\0';
    }

    /// <summary>
    /// Изменить размер поля
    /// </summary>
    private void ChangeFieldSize()
    {
      if (_inputFieldX.text == "" || _inputFieldX.text == $"{0}")
        _inputFieldX.text = $"{1}";
      if (_inputFieldY.text == "" || _inputFieldY.text == $"{0}")
        _inputFieldY.text = $"{1}";
      if (_inputFieldZ.text == "" || _inputFieldZ.text == $"{0}")
        _inputFieldZ.text = $"{1}";

      if (int.TryParse(_inputFieldX.text, out int X) && int.TryParse(_inputFieldY.text, out int Y) && int.TryParse(_inputFieldZ.text, out int Z))
      {
        gridEditor.ChangeFieldSize(new Vector3Int(X, Y, Z));
      }
    }

    /// <summary>
    /// Обновить текст размера поля
    /// </summary>
    private void UpdateTextFieldSize(Vector3Int fieldSize)
    {
      _inputFieldX.text = $"{fieldSize.x}";
      _inputFieldY.text = $"{fieldSize.y}";
      _inputFieldZ.text = $"{fieldSize.z}";
    }

    #endregion

    #region Высота уровня

    /// <summary>
    /// Изменить уровень поля
    /// </summary>
    private void ChangeGridLevel(bool parValue)
    {
      gridEditor.ChangeGridLevel(parValue);

      UpdateTextGridLevel(gridEditor.GetGridLevel());
    }

    /// <summary>
    /// Обновить текст уровня поля
    /// </summary>
    private void UpdateTextGridLevel(int gridLevel)
    {
      _gridLevelText.text = $"{gridLevel + 1}";
    }

    #endregion

    #region Toggle

    /// <summary>
    /// Изменить значение ToggleEditMode
    /// </summary>
    private void ChangeToggleEditMode(bool parValue)
    {
      if (parValue)
      {
        _toggleDeleteMode.isOn = false;

        gridEditor.EditMode = parValue;
        gridEditor.DeleteMode = !parValue;
      }
    }

    /// <summary>
    /// Изменить значение ToggleDeleteMode
    /// </summary>
    private void ChangeToggleDeleteMode(bool parValue)
    {
      if (parValue)
      {
        _toggleEditMode.isOn = false;

        gridEditor.EditMode = !parValue;
        gridEditor.DeleteMode = parValue;
      }
    }

    #endregion

    /// <summary>
    /// UI Создание данных уровня
    /// </summary>
    private void UICreateLevelData()
    {
      numberClickCreateButton++;

      numberClickConfirmButton = 0;
      numberClickLoadButton = 0;
      numberClickClearButton = 0;

      if (numberClickCreateButton < 2)
        return;

      numberClickCreateButton = 0;

      int tempNum = 0;
      foreach (var blockObject in gridEditor.GetBlockObjects())
      {
        if (blockObject == null)
          continue;

        tempNum++;
      }

      if (tempNum == 0)
        return;

      gridEditor.CreateLevelData();
      AddNumberLevelDropDown(gridEditor.SelectedLocation);

      gridEditor.ClearLevelObjects();
      gridEditor.CurrentLevelData = null;
    }

    private void UIConfrirm()
    {
      numberClickConfirmButton++;

      numberClickCreateButton = 0;
      numberClickLoadButton = 0;
      numberClickClearButton = 0;

      if (numberClickConfirmButton < 2)
        return;

      numberClickConfirmButton = 0;

      ChangeFieldSize();
    }

    /// <summary>
    /// UI Загрузка данных уровня
    /// </summary>
    private void UILoadLevelData()
    {
      numberClickLoadButton++;

      numberClickCreateButton = 0;
      numberClickConfirmButton = 0;
      numberClickClearButton = 0;

      if (numberClickLoadButton < 2)
        return;

      numberClickLoadButton = 0;

      gridEditor.LoadLevelData();
    }

    /// <summary>
    /// UI Очистка данных уровня
    /// </summary>
    private void UIClearLevelData()
    {
      numberClickClearButton++;

      numberClickCreateButton = 0;
      numberClickLoadButton = 0;
      numberClickConfirmButton = 0;

      if (numberClickClearButton < 2)
        return;

      numberClickClearButton = 0;

      gridEditor.ClearLevelObjects();
      gridEditor.CurrentLevelData = null;
    }

    //======================================
  }
}