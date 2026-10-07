// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MergeIt.Core.Configs;
using MergeIt.Core.Configs.Data;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Core.Schemes;
using MergeIt.Editor.Configs;
using MergeIt.Editor.LevelEditor.Commands;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using PopupWindow = UnityEngine.UIElements.PopupWindow;
using Task = System.Threading.Tasks.Task;
using VisualElement = UnityEngine.UIElements.VisualElement;

namespace MergeIt.Editor.LevelEditor
{
    public class LevelEditorWindow : EditorWindow
    {
        private static LevelEditorWindow _window;

        private Button _saveButton;
        private Button _loadButton;

        private VisualElement _grid;
        private VisualElement _itemFrameSettings;
        private VisualElement _itemFrameEvolutions;
        private StyleSheet _globalStyle;
        private ToolbarToggle[,] _toggles;
        private ToolbarToggle _selectedToggle;
        private GridPoint _selectedCell;
        private Toggle _isLockedToggle;
        private Button _applyButton;
        private Button _clearButton;
        private Image _itemImage;
        private Button _copyButton;
        private IntegerField _copyRow;
        private IntegerField _copyColumn;

        private Button _createNewButton;
        private IntegerField _createFieldWidth;
        private IntegerField _createFieldHeight;

        private Label _warnLabel;

        private SchemeObject _loadedScheme;
        private PopupField<EvolutionData> _evoPopup;
        private PopupField<ElementConfig> _evolutionChainElementsPopup;

        private Texture _lockTexture;

        private int _fieldWidth = 0;
        private int _fieldHeight = 0;

        private ObjectField _evolutionsField;
        private LevelConfig _levelConfig;
        private PopupWindow _createLevelPopup;
        private Dictionary<GridPoint, LevelElementData> _fieldElements = new();
        private ToolbarButton _createButton;
        private Rect _popupRect;
        private Button _createLevelButton;

        private GridPoint _copiedCell = GridPoint.Default;

        private IActionCommandManager _commandManager;

        [MenuItem("Window/Merge Toolkit/Level (Field)", false, 2)]
        public static void ShowWindow()
        {
            Show(null);
        }

        public static void Show(LevelConfig levelConfig)
        {
            _window = GetWindow<LevelEditorWindow>(true);
            _window.minSize = new Vector2(800, 600);
            _window.titleContent = new GUIContent("New level");

            if (levelConfig != null)
            {
                _window.Reload(levelConfig);
            }
        }

        public void CreateGUI()
        {
            _window = this;
            _commandManager = new ActionCommandManager();

            VisualElement root = rootVisualElement;
            root.focusable = true;
            root.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.commandKey || evt.ctrlKey)
                {
                    switch (evt.keyCode)
                    {
                        case KeyCode.Z:
                            _commandManager.Undo();
                            evt.StopPropagation();
                            break;

                        case KeyCode.Y:
                            _commandManager.Redo();
                            evt.StopPropagation();
                            break;
                    }
                }
            });

            _lockTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/MergeIt/Content/Images/Common/lock.png");

            var visualTree =
                (VisualTreeAsset)EditorGUIUtility.Load(Path.Combine(Constants.LevelEditorResourcesRoot,
                    "LevelEditorWindow.uxml"));
            VisualElement windowRoot = visualTree.CloneTree();
            root.Add(windowRoot);

            _globalStyle =
                (StyleSheet)EditorGUIUtility.Load(Path.Combine(Constants.LevelEditorResourcesRoot,
                    "LevelEditorWindow.uss"));
            root.styleSheets.Add(_globalStyle);
            root.RegisterCallback<KeyUpEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.S && (evt.commandKey || evt.ctrlKey))
                {
                    SaveButtonClicked();
                }
            });

            _evolutionsField = root.Q<ObjectField>("EvolutionsField");
            _evolutionsField.objectType = typeof(SchemeObject);
            _evolutionsField.RegisterValueChangedCallback(OnEvolutionsConfigChanged);

            ToolbarMenu toolbarMenu = windowRoot.Q<ToolbarMenu>("FileMenu");
            toolbarMenu.menu.AppendAction("Open...", _ => LoadButtonClicked());
            toolbarMenu.menu.AppendAction("Save", _ => SaveButtonClicked());
            toolbarMenu.menu.AppendAction("Save As...", _ => SaveAsButtonClicked());

            _createButton = root.Q<ToolbarButton>("CreateButton");
            _createButton.clicked += CreateButtonClicked;

            _createLevelButton = root.Q<Button>("CreateLevelButton");
            _createLevelButton.clicked += CreateLevelButtonClicked;

            _createLevelPopup = root.Q<PopupWindow>("CreateLevelPopup");
            _createLevelPopup.visible = false;

            _createFieldHeight = root.Q<IntegerField>("FieldHeight");
            _createFieldWidth = root.Q<IntegerField>("FieldWidth");

            _createFieldHeight.RegisterValueChangedCallback(OnFieldSizeChanged);
            _createFieldWidth.RegisterValueChangedCallback(OnFieldSizeChanged);

            _warnLabel = root.Q<Label>("WarningLabel");
            _warnLabel.visible = false;

            _grid = root.Q<VisualElement>("ItemsGrid");
            _grid.focusable = true;
            _grid.RegisterCallback<KeyUpEvent>(evt =>
            {
                if (evt.keyCode is KeyCode.Delete or KeyCode.Backspace)
                {
                    if (_selectedToggle != null)
                    {
                        ClearButtonClicked();
                    }
                }
                else if (evt.commandKey || evt.ctrlKey)
                {
                    switch (evt.keyCode)
                    {
                        case KeyCode.C:
                            if (_selectedCell != GridPoint.Default)
                            {
                                _copiedCell = _selectedCell;
                            }

                            break;

                        case KeyCode.V:
                            if (_selectedToggle != null &&
                                _copiedCell != GridPoint.Default &&
                                _selectedCell != _copiedCell)
                            {
                                CopyCell(_copiedCell.X + 1, _copiedCell.Y + 1);
                            }

                            break;
                    }
                }
            });

            _itemFrameSettings = root.Q<VisualElement>("ItemFrameSettings");
            SwitchSideBarVisibility(false);
            _isLockedToggle = _itemFrameSettings.Q<Toggle>("IsLocked");
            _isLockedToggle.RegisterValueChangedCallback(OnIsLockedChanged);

            _itemFrameEvolutions = _itemFrameSettings.Q<VisualElement>("EvoPanel");

            _itemImage = _itemFrameSettings.Q<Image>("ItemIcon");

            _applyButton = _itemFrameSettings.Q<Button>("ApplyButton");
            _applyButton.clicked += ApplyButtonClicked;

            _clearButton = _itemFrameSettings.Q<Button>("ClearButton");
            _clearButton.clicked += ClearButtonClicked;

            _copyButton = _itemFrameSettings.Q<Button>("CopyButton");
            _copyRow = _itemFrameSettings.Q<IntegerField>("CopyRow");
            _copyColumn = _itemFrameSettings.Q<IntegerField>("CopyColumn");

            _copyButton.clicked += CopyButtonClicked;

            CheckCreateButton();
        }
        
        public void ApplyCell(LevelElementData newData, bool wasChanged)
        {
            SetupElementCell(newData);
            CellSelected(true, newData.Position.X, newData.Position.Y);
            DrawElementCell(newData, _selectedToggle);

            if (!wasChanged)
            {
                SetItemData();
            }
        }

        public void UndoApplyCell(LevelElementData previousData, LevelElementData newData)
        {
            if (previousData != null)
            {
                CellSelected(true, previousData.Position.X, previousData.Position.Y);
                ApplyCell(previousData, true);
            }
            else
            {
                ClearCell(newData.Position);
            }
        }

        public void ClearCell(GridPoint point)
        {
            CellSelected(true, point.X, point.Y);
            _fieldElements.Remove(point);
            _selectedToggle.style.backgroundImage = null;

            ChangeVisualLock(false, _selectedToggle);
        }

        public void UndoClearCell(LevelElementData previousData)
        {
            SetupElementCell(previousData);
            CellSelected(true, previousData.Position.X, previousData.Position.Y);
            DrawElementCell(previousData, _selectedToggle);
        }

        private void OnDestroy()
        {
            rootVisualElement.UnregisterCallback<MouseUpEvent>(OnElementClicked);

            _isLockedToggle.UnregisterValueChangedCallback(OnIsLockedChanged);
            _createFieldHeight.UnregisterValueChangedCallback(OnFieldSizeChanged);
            _createFieldWidth.UnregisterValueChangedCallback(OnFieldSizeChanged);
            _createLevelButton.clicked -= CreateLevelButtonClicked;
            _createButton.clicked -= CreateButtonClicked;
            _copyButton.clicked -= CopyButtonClicked;
            _clearButton.clicked -= ClearButtonClicked;
            _applyButton.clicked -= ApplyButtonClicked;
        }

        private void OnFieldSizeChanged(ChangeEvent<int> evt)
        {
            CheckCreateButton();
        }

        private void CheckCreateButton()
        {
            if (_createFieldHeight.value <= 0 || _createFieldWidth.value <= 0)
            {
                _createLevelButton.SetEnabled(false);
            }
            else
            {
                _createLevelButton.SetEnabled(true);
            }
        }

        private void OnElementClicked(MouseUpEvent evt)
        {
            if (_createLevelPopup.visible && !_popupRect.Contains(evt.mousePosition))
            {
                ToggleCreatePopup();
                rootVisualElement.UnregisterCallback<MouseUpEvent>(OnElementClicked);
            }
        }

        private void SaveButtonClicked()
        {
            PackLevel(_levelConfig);

            EditorUtility.SetDirty(_levelConfig);

            Debug.Log($"Level saved successfully: {AssetDatabase.GetAssetPath(_levelConfig)}");
        }

        private void SaveAsButtonClicked()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save level", "New_level", "asset", "Save level to file");
            if (path.Length != 0)
            {
                var levelConfig = CreateInstance<LevelConfig>();

                PackLevel(levelConfig);

                AssetDatabase.CreateAsset(levelConfig, path);

                Debug.Log($"New level saved successfully: {path}");

                _levelConfig = levelConfig;
                _window.titleContent = new GUIContent(path);
            }
        }

        private void LoadButtonClicked()
        {
            var path = EditorUtility.OpenFilePanelWithFilters("Load level", "Assets",
                new[] { "Saved levels", "asset" });
            if (path.Length != 0)
            {
                try
                {
                    string relativePath = null;
                    if (path.StartsWith(Application.dataPath))
                    {
                        relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                    }

                    var levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfig>(relativePath);
                    Debug.Log($"Start loading config with elements: {levelConfig?.FieldElementsData?.Count}");

                    Reload(levelConfig);

                    Debug.Log($"Level loaded successfully: {relativePath}");
                }
                catch (Exception e)
                {
                    Debug.Log($"{e}");
                }
            }
        }

        private async void Reload(LevelConfig levelConfig)
        {
            UnpackLevel(levelConfig);

            while (EvolutionSelection.ElementData == null)
            {
                await Task.Yield();
            }

            DrawLevel();

            var path = AssetDatabase.GetAssetPath(levelConfig);

            _window.titleContent = new GUIContent(path);
        }

        private void CreateButtonClicked()
        {
            ToggleCreatePopup();
            rootVisualElement.RegisterCallback<MouseUpEvent>(OnElementClicked, TrickleDown.TrickleDown);
        }

        private void CreateLevelButtonClicked()
        {
            var path = EditorUtility.SaveFilePanelInProject("Create level", "New_level", "asset", "Create new level");
            if (path.Length != 0)
            {
                _fieldWidth = _createFieldWidth.value;
                _fieldHeight = _createFieldHeight.value;

                _levelConfig = CreateInstance<LevelConfig>();
                _levelConfig.FieldWidth = _fieldWidth;
                _levelConfig.FieldHeight = _fieldHeight;
                _levelConfig.EvolutionsScheme = _evolutionsField.value as SchemeObject;

                AssetDatabase.CreateAsset(_levelConfig, path);

                Debug.Log($"Level created successfully: {path}");

                _fieldElements = new Dictionary<GridPoint, LevelElementData>();

                DrawLevel();

                _window.titleContent = new GUIContent(path);
                _createLevelPopup.style.display = StylesConstants.DisplayNone;
            }
        }

        private void ToggleCreatePopup()
        {
            if (_createLevelPopup.visible)
            {
                _createLevelPopup.visible = false;
            }
            else
            {
                _createLevelPopup.visible = true;
                Rect worldBound = _createButton.worldBound;
                var buttonPosition = new Vector2(worldBound.xMin, worldBound.yMax);
                _createLevelPopup.style.top = buttonPosition.y;
                _createLevelPopup.style.left = buttonPosition.x;
                _popupRect = new Rect(buttonPosition.x, buttonPosition.y, _createLevelPopup.worldBound.width,
                    _createLevelPopup.worldBound.height);
            }
        }

        private void PackLevel(LevelConfig levelConfig)
        {
            List<LevelElementData> elements = _fieldElements.Values.ToList();
            levelConfig.EvolutionsScheme = _evolutionsField.value as SchemeObject;
            levelConfig.FieldElementsData = elements;
            levelConfig.FieldHeight = _fieldHeight;
            levelConfig.FieldWidth = _fieldWidth;
        }

        private void UnpackLevel(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            if (levelConfig.FieldElementsData?.Count != 0)
            {
                _fieldElements = new();
                if (levelConfig.FieldElementsData != null)
                {
                    foreach (var levelElementData in levelConfig.FieldElementsData)
                    {
                        _fieldElements[levelElementData.Position] = levelElementData.GetClone();
                    }
                }
            }
            else
            {
                _fieldElements = new Dictionary<GridPoint, LevelElementData>();
            }

            _fieldHeight = levelConfig.FieldHeight;
            _fieldWidth = levelConfig.FieldWidth;

            var evolution = _levelConfig.EvolutionsScheme;
            _evolutionsField.SetValueWithoutNotify(evolution);
            SetupEvolution(evolution);
        }

        private void CopyButtonClicked()
        {
            int row = _copyRow.value;
            int column = _copyColumn.value;

            CopyCell(row, column);
        }

        private void CopyCell(int row, int column)
        {
            if (row < 1 || row > _fieldHeight)
            {
                Debug.Log($"Row number {row} is out of bounds");
                return;
            }

            if (column < 1 || column > _fieldWidth)
            {
                Debug.Log($"Column number {row} is out of bounds");
                return;
            }

            _fieldElements.TryGetValue(GridPoint.Create(row - 1, column - 1), out LevelElementData data);

            if (data != null)
            {
                EvolutionSelection.UpdateCell(data, copyPosition: false);

                PerformApplyCellCommand();
            }
            else
            {
                PerformClearCellCommand(EvolutionSelection.ElementData.Position);
            }
        }

        private void ApplyButtonClicked()
        {
            PerformApplyCellCommand();
        }

        private void ClearButtonClicked()
        {
            PerformClearCellCommand(EvolutionSelection.ElementData.Position);
        }

        private void PerformApplyCellCommand()
        {
            var newData = EvolutionSelection.ElementData.GetClone();

            if (_fieldElements.TryGetValue(newData.Position, out var data))
            {
                var changeCommand = new ChangeCellCommand(this, data, newData);
                _commandManager.ExecuteCommand(changeCommand);
            }
            else
            {
                var applyCommand = new ApplyCellCommand(this, newData);
                _commandManager.ExecuteCommand(applyCommand);
            }
        }

        private void PerformClearCellCommand(GridPoint point)
        {
            var existingData = EvolutionSelection.ElementData.GetClone();
            var command = new ClearCellCommand(this, existingData);
            _commandManager.ExecuteCommand(command);
        }

        private void OnIsLockedChanged(ChangeEvent<bool> evt)
        {
            SetLock(evt.newValue);
        }

        private void OnEvolutionsConfigChanged(ChangeEvent<Object> evt)
        {
            ClearAll();

            SetupEvolution(evt.newValue as SchemeObject);
        }

        private void SetupEvolution(SchemeObject config)
        {
            if (config)
            {
                _loadedScheme = config;

                if (_loadedScheme != null)
                {
                    Debug.Log($"Evolutions config changed to: {_loadedScheme.name}");

                    EvolutionSelection.Init(_loadedScheme.Evolution);
                    SwitchFieldActiveState(true);
                }
            }
        }

        private void SwitchFieldActiveState(bool active)
        {
            _warnLabel.visible = !active;

            if (_toggles != null)
            {
                for (int i = 0; i < _toggles.GetLength(0); i++)
                {
                    for (int j = 0; j < _toggles.GetLength(1); j++)
                    {
                        _toggles[i, j].SetEnabled(active);
                    }
                }
            }
        }

        private void SwitchSideBarVisibility(bool active)
        {
            _itemFrameSettings.visible = active;
        }

        private void ClearAll()
        {
            _selectedToggle = null;
            _selectedCell = GridPoint.Default;

            SwitchFieldActiveState(false);
            SwitchSideBarVisibility(false);
            ClearField();
            EvolutionSelection.ClearSelection();
        }

        private void ClearField()
        {
            foreach (KeyValuePair<GridPoint, LevelElementData> fieldElement in _fieldElements)
            {
                GridPoint point = fieldElement.Key;
                ToolbarToggle toggle = _toggles[point.X, point.Y];
                toggle.style.backgroundImage = null;
                ChangeVisualLock(false, toggle);
                UnselectToggle(toggle, true);
            }

            _fieldElements.Clear();
        }

        private void DrawLevel()
        {
            _grid.Clear();
            _toggles = new ToolbarToggle[_fieldHeight, _fieldWidth];

            for (int i = 0; i < _fieldHeight; i++)
            {
                if (i == 0)
                {
                    var visualElementHeader = new VisualElement();
                    visualElementHeader.AddToClassList("itemsGridRowHeader");
                    for (int j = 0; j < _fieldWidth; j++)
                    {
                        var labelColumn = new Label($"{j + 1}");
                        labelColumn.AddToClassList("gridHeaderLabel");
                        visualElementHeader.Add(labelColumn);
                    }

                    _grid.Add(visualElementHeader);
                }

                var visualElement = new VisualElement();
                visualElement.AddToClassList("itemsGridRow");

                var labelRow = new Label($"{i + 1}");
                labelRow.AddToClassList("gridLabelColumn");

                visualElement.Add(labelRow);

                for (int j = 0; j < _fieldWidth; j++)
                {
                    var button = new ToolbarToggle();
                    var elementPosition = GridPoint.Create(i, j);
                    if (_fieldElements.TryGetValue(elementPosition, out LevelElementData data))
                    {
                        DrawElementCell(data, button);
                    }

                    _toggles[i, j] = button;

                    int row = i;
                    int column = j;

                    button.RegisterCallback<MouseUpEvent>(evt =>
                    {
                        if (button.value)
                        {
                            SelectToggle(button, false);
                        }
                        else
                        {
                            UnselectToggle(button, false);
                        }
                        
                        CellSelected(button.value, row, column);
                        OnElementClicked(evt);
                    });

                    button.AddToClassList("gridButton");
                    visualElement.Add(button);
                }

                _grid.Add(visualElement);
            }

            if (_loadedScheme == null)
            {
                SwitchFieldActiveState(false);
            }
        }

        private void CellSelected(bool selected, int row, int column)
        {
            if (selected)
            {
                for (int i = 0; i < _fieldHeight; i++)
                {
                    for (int j = 0; j < _fieldWidth; j++)
                    {
                        var toggle = _toggles[i, j];

                        if (i == row && j == column)
                        {
                            GridPoint elementPosition = GridPoint.Create(row, column);

                            _fieldElements.TryGetValue(elementPosition, out LevelElementData elementData);
                            _selectedToggle = toggle;
                            _selectedCell = new GridPoint(row, column);
                            _selectedToggle.SetValueWithoutNotify(true);

                            SelectToggle(_selectedToggle, true);

                            SwitchSideBarVisibility(true);

                            EvolutionSelection.SelectCell(row, column, elementData);

                            SetItemData();

                            continue;
                        }

                        UnselectToggle(toggle, true);
                    }
                }
            }
            else
            {
                EvolutionSelection.ClearSelection();
                SwitchSideBarVisibility(false);
            }
        }

        private void SetLock(bool isLocked)
        {
            EvolutionSelection.LockCell(isLocked);
        }

        private void ChangeVisualLock(bool isLocked, Toggle toggle)
        {
            var locker = toggle.Q<Image>("Locker");
            if (isLocked)
            {
                if (locker == null)
                {
                    locker = CreateLocker();
                    toggle.Add(locker);
                }
            }
            else
            {
                if (locker != null)
                {
                    toggle.Remove(locker);
                }
            }
        }

        private void SetupElementCell(LevelElementData itemData)
        {
            if (_fieldElements.TryGetValue(itemData.Position, out LevelElementData fieldElement))
            {
                fieldElement.CopyFrom(itemData);
            }
            else
            {
                _fieldElements[itemData.Position] = itemData;
            }
        }

        private void DrawElementCell(LevelElementData elementData, Toggle toggle)
        {
            ElementConfig elementConfig = elementData.Element;

            Sprite icon = GetIcon(elementConfig.CommonSettings.Icon);

            toggle.style.backgroundImage = icon != null ? icon.texture : null;

            ChangeVisualLock(elementData.IsBlocked, toggle);
        }

        private Sprite GetIcon(FieldElementIconComponent placeholder)
        {
            Sprite sprite = null;
            if (placeholder)
            {
                sprite = placeholder.GetImage()?.sprite;
            }

            return sprite;
        }

        private Image CreateLocker()
        {
            var locker = new Image
            {
                name = "Locker",
                image = _lockTexture,
                style =
                {
                    left = StylesConstants.Length0,
                    right = StylesConstants.Length0,
                    top = StylesConstants.Length0,
                    bottom = StylesConstants.Length0,
                    position = StylesConstants.AbsolutePosition,
                }
            };

            return locker;
        }

        private void SelectToggle(ToolbarToggle toggle, bool setValue)
        {
            if (setValue)
            {
                toggle.SetValueWithoutNotify(true);
            }
            toggle.AddToClassList("gridSelectedButton");
            toggle.RemoveFromClassList("gridButton");
        }

        private void UnselectToggle(ToolbarToggle toggle, bool setValue)
        {
            if (setValue)
            {
                toggle.SetValueWithoutNotify(false);
            }
            toggle.AddToClassList("gridButton");
            toggle.RemoveFromClassList("gridSelectedButton");
        }

        private void SetItemData()
        {
            if (_evoPopup == null)
            {
                _evoPopup = new PopupField<EvolutionData>("Evolution:", _loadedScheme.Evolution,
                    EvolutionSelection.Evolution);

                _evoPopup.formatListItemCallback = FormatEvolutionCallback;
                _evoPopup.formatSelectedValueCallback = FormatEvolutionCallback;

                _evoPopup.AddToClassList("evoList");
                _evoPopup.RegisterValueChangedCallback(OnEvolutionChanged);
                _itemFrameEvolutions.Add(_evoPopup);
            }

            int evoIndex = _loadedScheme.Evolution.IndexOf(EvolutionSelection.Evolution);

            if (evoIndex == _evoPopup.index)
            {
                SendChangedEvent(_evoPopup, _evoPopup.value, EvolutionSelection.Evolution);
            }
            else
            {
                _evoPopup.index = _loadedScheme.Evolution.IndexOf(EvolutionSelection.Evolution);
            }

            _isLockedToggle.SetValueWithoutNotify(EvolutionSelection.ElementData.IsBlocked);
        }

        private void OnEvolutionChanged(ChangeEvent<EvolutionData> evt)
        {
            EvolutionSelection.SelectEvolution(evt.newValue);

            _evoPopup.index = _loadedScheme.Evolution.IndexOf(EvolutionSelection.Evolution);

            if (_evolutionChainElementsPopup != null)
            {
                _evolutionChainElementsPopup.UnregisterValueChangedCallback(OnEvolutionChainElementChanged);
                _itemFrameEvolutions.Remove(_evolutionChainElementsPopup);
            }

            List<ElementConfig> evolutionItems = EvolutionSelection.Evolution.Chain;

            _evolutionChainElementsPopup =
                new PopupField<ElementConfig>("Element:", evolutionItems, EvolutionSelection.EvoItem);

            _evolutionChainElementsPopup.RegisterValueChangedCallback(OnEvolutionChainElementChanged);

            _evolutionChainElementsPopup.formatSelectedValueCallback = OnFormatEvolutionChainItemCallback;
            _evolutionChainElementsPopup.formatListItemCallback = OnFormatEvolutionChainItemCallback;

            _evolutionChainElementsPopup.AddToClassList("evoList");
            _itemFrameEvolutions.Add(_evolutionChainElementsPopup);

            SendChangedEvent(_evolutionChainElementsPopup, _evolutionChainElementsPopup.value,
                EvolutionSelection.EvoItem);
        }

        private void OnEvolutionChainElementChanged(ChangeEvent<ElementConfig> evt)
        {
            EvolutionSelection.SelectElement(evt.newValue);

            var elementConfig = evt.newValue;
            Sprite icon = GetIcon(elementConfig.CommonSettings.Icon);

            _itemImage.image = icon != null ? icon.texture : null;
        }

        private void SendChangedEvent<T>(PopupField<T> field, T prevValue, T newValue)
        {
            var changeEvent = ChangeEvent<T>.GetPooled(prevValue, newValue);
            changeEvent.target = field;
            field.SendEvent(changeEvent);
        }

        private string FormatEvolutionCallback(EvolutionData arg)
        {
            return arg.Name;
        }

        private string OnFormatEvolutionChainItemCallback(ElementConfig item)
        {
            var evoIndex = _evoPopup.index;
            var evolution = _loadedScheme.Evolution[evoIndex];
            var itemIndex = evolution.Chain.IndexOf(item);

            var elementConfig = item;
            string itemName = $"Level {itemIndex + 1}: {elementConfig.CommonSettings.Name}";

            return itemName;
        }
    }
}