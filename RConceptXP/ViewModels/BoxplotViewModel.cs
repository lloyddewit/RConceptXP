using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using RConceptXP.Services;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Collections;

namespace RConceptXP.ViewModels;

//todo support empty list of columns (test that selector buttons enable/disable correctly and no crashes)
public partial class BoxplotViewModel : ObservableObject
{
    public event EventHandler? DataOptionsOpened;

    public RelayCommand OnDataOptionsClickCommand { get; }
    public RelayCommand<int> OnMainTabRightClickCommand { get; }
    public RelayCommand<TextBox> OnReceiverGotFocusCommand { get; }
    public RelayCommand OnRedoClickCommand { get; }
    public RelayCommand OnResetClickCommand { get; }
    public RelayCommand OnSelectorAddAllClickCommand { get; }
    public RelayCommand OnSelectorAddClickCommand { get; }
    public RelayCommand OnToScriptClickCommand { get; }
    public RelayCommand OnUndoClickCommand { get; }

    public bool IsOkEnabled => GetIsOkEnabled();
    public bool IsWidthEnabled => GetIsFactorNumeric();
    public MainViewModel MainViewModel => _mainViewModel;
    public SelectionModel<string> Selection { get; }

    [ObservableProperty]
    private List<string> _columnNames = [];

    [ObservableProperty]
    private string _comment;

    [ObservableProperty]
    private string _dataFrame;

    [ObservableProperty]
    private string _facetBy;

    [ObservableProperty]
    private string _facetByType;

    [ObservableProperty]
    private List<string> _facetByTypes;

    [ObservableProperty]
    private string _factor;

    [ObservableProperty]
    private List<string> _groupToConnectSummaries;

    [ObservableProperty]
    private string _groupToConnectSummary;

    [ObservableProperty]
    private bool _isAddPoints;

    [ObservableProperty]
    private bool _isBoxPlot;

    [ObservableProperty]
    private bool _isBoxPlotExtra;

    [ObservableProperty]
    private bool _isComment;

    [ObservableProperty]
    private bool _isGroupToConnect;

    [ObservableProperty]
    private bool _isHorizontalBoxPlot;

    [ObservableProperty]
    private bool _isJitter;

    [ObservableProperty]
    private bool _isLegend;

    [ObservableProperty]
    private bool _isSaveGraph;

    [ObservableProperty]
    private bool _isSingle;

    [ObservableProperty]
    private bool _isTufte;

    [ObservableProperty]
    private bool _isVarWidth;

    [ObservableProperty]
    private bool _isViolin;

    [ObservableProperty]
    private bool _isWidth;

    [ObservableProperty]
    private string _jitterExtra;

    [ObservableProperty]
    private string _legendPosition;

    [ObservableProperty]
    private List<string> _legendPositions;

    [ObservableProperty]
    private string _multipleVariables;

    [ObservableProperty]
    private string _saveName;

    [ObservableProperty]
    private List<string> _saveNames;

    [ObservableProperty]
    private string _secondFactor;

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private string _singleVariable;

    [ObservableProperty]
    private string _transparency;

    [ObservableProperty]
    private string _width;

    [ObservableProperty]
    private string _widthExtra;

    partial void OnColumnNamesChanged(List<string> value) => OnPropertyChangedAction();
    partial void OnCommentChanged(string value) => OnPropertyChangedAction();
    partial void OnDataFrameChanged(string value) => OnPropertyChangedAction();
    partial void OnFacetByChanged(string value) => OnPropertyChangedAction();
    partial void OnFacetByTypeChanged(string value) => OnPropertyChangedAction();
    partial void OnFacetByTypesChanged(List<string> value) => OnPropertyChangedAction();
    partial void OnFactorChanged(string value)
    {
        OnPropertyChangedAction();
        OnPropertyChanged(nameof(IsWidthEnabled));
    }
    partial void OnGroupToConnectSummariesChanged(List<string> value) => OnPropertyChangedAction();
    partial void OnGroupToConnectSummaryChanged(string value) => OnPropertyChangedAction();
    partial void OnIsAddPointsChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsBoxPlotChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsBoxPlotExtraChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsCommentChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsGroupToConnectChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsHorizontalBoxPlotChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsJitterChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsLegendChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsSaveGraphChanged(bool value)
    {
        OnPropertyChangedAction();
        OnPropertyChanged(nameof(IsOkEnabled));
    }
    partial void OnIsSingleChanged(bool value)
    {
        OnPropertyChangedAction();
        OnPropertyChanged(nameof(IsOkEnabled));
    }
    partial void OnIsTufteChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsVarWidthChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsViolinChanged(bool value) => OnPropertyChangedAction();
    partial void OnIsWidthChanged(bool value) => OnPropertyChangedAction();
    partial void OnJitterExtraChanged(string value) => OnPropertyChangedAction();
    partial void OnLegendPositionChanged(string value) => OnPropertyChangedAction();
    partial void OnLegendPositionsChanged(List<string> value) => OnPropertyChangedAction();
    partial void OnMultipleVariablesChanged(string value)
    {
        OnPropertyChangedAction();
        OnPropertyChanged(nameof(IsOkEnabled));
    }
    partial void OnSaveNameChanged(string value)
    {
        OnPropertyChangedAction();
        OnPropertyChanged(nameof(IsOkEnabled));
    }
    partial void OnSaveNamesChanged(List<string> value) => OnPropertyChangedAction();
    partial void OnSecondFactorChanged(string value) => OnPropertyChangedAction();
    partial void OnSelectedTabIndexChanged(int value) => OnPropertyChangedAction();
    partial void OnSingleVariableChanged(string value)
    {
        OnPropertyChangedAction();
        OnPropertyChanged(nameof(IsOkEnabled));
    }
    partial void OnTransparencyChanged(string value) => OnPropertyChangedAction();
    partial void OnWidthChanged(string value) => OnPropertyChangedAction();
    partial void OnWidthExtraChanged(string value) => OnPropertyChangedAction();

    private MenuItem _addAllOption;
    private ListBox _columnsListBox;
    private TextBox _facetByTextBox;
    private TextBox _factorTextBox;
    private MainViewModel _mainViewModel;
    private TextBox _multipleVariableTextBox;
    private TextBox _secondFactorTextBox;
    private SelectorMediator _selectorMediator;
    private TextBox _singleVariableTextBox;

    // Disable the warning 'non-nullable field must contain a non-null value when exiting
    // constructor'. We need to disable because otherwise many incorrect warnings are generated
    // for the observable properties that are initialised in the constructor via the
    // 'OnResetClick' method.
#pragma warning disable CS8618
    public BoxplotViewModel(MainViewModel mainViewModel, BoxplotMainTabView boxplotMainTabView, BoxplotViewModel? boxplotToDuplicate)
#pragma warning restore CS8618
    {
        _mainViewModel = mainViewModel;

        // initialize receiver controls
        OnReceiverGotFocusCommand = new RelayCommand<TextBox>(OnReceiverGotFocus);

        var columnSelectorView = boxplotMainTabView.FindControl<ColumnSelectorView>("columnSelectorView") ??
            throw new Exception("Cannot find columnSelectorView by name");
        _columnsListBox = columnSelectorView.FindControl<ListBox>("columns") ??
            throw new Exception("Cannot find columns ListBox by name");
        _addAllOption = columnSelectorView.FindControl<MenuItem>("addAllOption") ??
            throw new Exception("Cannot find addAllOption MenuItem by name");

        var singleMultipleReceiverView = boxplotMainTabView.FindControl<SingleMultipleReceiverView>("singleMultipleReceiverView") ??
            throw new Exception("Cannot find singleMultipleReceiverView by name");
        _singleVariableTextBox = singleMultipleReceiverView.FindControl<TextBox>("singleVariableTextBox") ??
            throw new Exception("Cannot find singleVariableTextBox by name");
        _multipleVariableTextBox = singleMultipleReceiverView.FindControl<TextBox>("multipleVariableTextBox") ??
            throw new Exception("Cannot find multipleVariableTextBox by name");

        _factorTextBox = boxplotMainTabView.FindControl<TextBox>("factorTextBox") ??
            throw new Exception("Cannot find factor textBox by name");
        _secondFactorTextBox = boxplotMainTabView.FindControl<TextBox>("secondFactorTextBox") ??
            throw new Exception("Cannot find secondFactorTextBox by name");

        var graphLegendFacetSaveView = boxplotMainTabView.FindControl<GraphLegendFacetSaveView>("graphLegendFacetSaveView") ??
            throw new Exception("Cannot find graphLegendFacetSaveView by name");
        _facetByTextBox = graphLegendFacetSaveView.FindControl<TextBox>("facetByTextBox") ??
            throw new Exception("Cannot find facetByTextBox by name");

        // Note: We need to catch delete and backspace key presses in receivers so that the user
        // can clear the receiver. There are simpler ways to catch most key events in Avalonia,
        // but delete and backspace are a special case and require a Tunnel routing strategy.
        _singleVariableTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown,
                                          RoutingStrategies.Tunnel);
        _multipleVariableTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown,
                                          RoutingStrategies.Tunnel);
        _factorTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown,
                                          RoutingStrategies.Tunnel);
        _secondFactorTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown,
                                          RoutingStrategies.Tunnel);
        _facetByTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown,
                                          RoutingStrategies.Tunnel);


        // initialize selector controls
        OnSelectorAddClickCommand = new RelayCommand(OnSelectorAddClick);
        OnSelectorAddAllClickCommand = new RelayCommand(OnSelectorAddAllClick);
        Selection = new SelectionModel<string>();

        // initialize selector model
        List<TextBox> receivers = new List<TextBox>
        {
            _singleVariableTextBox,
            _multipleVariableTextBox,
            _factorTextBox,
            _secondFactorTextBox,
            _facetByTextBox
        };
        _selectorMediator = new SelectorMediator(receivers);

        // initialize other command controls
        OnDataOptionsClickCommand = new RelayCommand(OnDataOptionsClick);
        OnMainTabRightClickCommand = new RelayCommand<int>(OnMainTabRightClick);
        OnRedoClickCommand = new RelayCommand(OnRedoClick);
        OnResetClickCommand = new RelayCommand(OnResetClick);
        OnToScriptClickCommand = new RelayCommand(OnToScriptClick);
        OnUndoClickCommand = new RelayCommand(OnUndoClick);

        // initialize one-way data bindings (set only once here and never changed)
        FacetByTypes = new List<string> { "Facet Wrap", "Facet Row", "Facet Column", "None" };
        GroupToConnectSummaries = new List<string> { "mean", "median" };
        LegendPositions = new List<string> { "None", "Left", "Right", "Top", "Bottom" };
        SaveNames = new List<string> { "box_plot", "jitter", "tufte_boxplot", "violin" };

        if (boxplotToDuplicate is null)
            OnResetClick();
        else
            DuplicateBoxplot(boxplotToDuplicate);
    }

    private string BoolToUpperCaseString(bool value)
    {
        return value ? "TRUE" : "FALSE";
    }

    private void DuplicateBoxplot(BoxplotViewModel boxplotToDuplicate)
    {
        // suspend data snapshots during duplication (snapshots are used for undo and redo)
        isSnapshotActive = false;

        Comment = boxplotToDuplicate.Comment;
        DataFrame = boxplotToDuplicate.DataFrame;
        FacetBy = boxplotToDuplicate.FacetBy;
        FacetByType = boxplotToDuplicate.FacetByType;
        Factor = boxplotToDuplicate.Factor;
        GroupToConnectSummary = boxplotToDuplicate.GroupToConnectSummary;
        IsAddPoints = boxplotToDuplicate.IsAddPoints;
        IsBoxPlot = boxplotToDuplicate.IsBoxPlot;
        IsBoxPlotExtra = boxplotToDuplicate.IsBoxPlotExtra;
        IsComment = boxplotToDuplicate.IsComment;
        IsGroupToConnect = boxplotToDuplicate.IsGroupToConnect;
        IsHorizontalBoxPlot = boxplotToDuplicate.IsHorizontalBoxPlot;
        IsJitter = boxplotToDuplicate.IsJitter;
        IsLegend = boxplotToDuplicate.IsLegend;
        IsSaveGraph = boxplotToDuplicate.IsSaveGraph;
        IsSingle = boxplotToDuplicate.IsSingle;
        IsTufte = boxplotToDuplicate.IsTufte;
        IsVarWidth = boxplotToDuplicate.IsVarWidth;
        IsViolin = boxplotToDuplicate.IsViolin;
        IsWidth = boxplotToDuplicate.IsWidth;
        JitterExtra = boxplotToDuplicate.JitterExtra;
        LegendPosition = boxplotToDuplicate.LegendPosition;
        MultipleVariables = boxplotToDuplicate.MultipleVariables;
        SaveName = boxplotToDuplicate.SaveName;
        SecondFactor = boxplotToDuplicate.SecondFactor;
        SelectedTabIndex = boxplotToDuplicate.SelectedTabIndex;
        SingleVariable = boxplotToDuplicate.SingleVariable;
        Transparency = boxplotToDuplicate.Transparency;
        Width = boxplotToDuplicate.Width;
        WidthExtra = boxplotToDuplicate.WidthExtra;

        // resume data snapshots and ensure this duplication is stored in a snaphot
        isSnapshotActive = true;
        OnPropertyChangedAction();
    }

    private bool GetIsFactorFactor()
    {
        if (string.IsNullOrEmpty(Factor))
            return false;

        // todo hard-coded column names for testing
        List<string> ColumnNamesFactor = new() { "village", "variety", "fertgrp" };
        return ColumnNamesFactor.Contains(Factor);
    }

    private bool GetIsFactorNumeric()
    {
        if (string.IsNullOrEmpty(Factor))
            return false;

        // todo hard-coded column names for testing
        List<string> ColumnNamesNonFactor = new() { "field", "size", "fert", "yield" };
        return ColumnNamesNonFactor.Contains(Factor);
    }

    private bool GetIsOkEnabled()
    {
        if (IsSaveGraph && string.IsNullOrEmpty(SaveName))
            return false;

        if (IsSingle)
            return !string.IsNullOrEmpty(SingleVariable);
        else
            return !string.IsNullOrEmpty(MultipleVariables);
    }

    private void OnDataOptionsClick()
    {
        DataOptionsOpened?.Invoke(this, EventArgs.Empty);
    }

    private void OnMainTabRightClick(int tabIndex)
    {
        SelectedTabIndex = tabIndex;
    }

    //todo implement undo/redo
    private bool isSnapshotActive = false;
    private Stack<BoxplotDataTransfer> undoStack = new();
    private Stack<BoxplotDataTransfer> redoStack = new();

    private void OnPropertyChangedAction()
    {
        // if undo snapshots suspended, then do nothing
        if (!isSnapshotActive)
            return;

        // create a new boxplot data transfer object
        BoxplotDataTransfer boxplotData = new BoxplotDataTransfer(this);

        // if current state is the same as the last snapshot, then do nothing
        if (undoStack.Count > 0 && boxplotData.Equals(undoStack.Peek()))
            return;

        // if we are halfway through a radio button change, then do nothing
        // (radio button changes trigger 2 events: one to set the current radio button to false
        //  and the second to set the new radio button to true)
        if (!IsBoxPlot && !IsJitter && !IsViolin)
            return;

        // if the undo stack is at its maximum size, remove the oldest snapshot
        // todo use config item for max undo stack size
        if (undoStack.Count >= 20)
        {
            // use a temporary stack to reverse the order
            Stack<BoxplotDataTransfer> tempStack = new Stack<BoxplotDataTransfer>();

            // transfer all but the undo items (except the oldest item) to the temporary stack
            while (undoStack.Count > 1)
            {
                tempStack.Push(undoStack.Pop());
            }

            // remove the oldest undo item
            undoStack.Pop();

            // transfer the remaining items back to the original undo stack
            while (tempStack.Count > 0)
            {
                undoStack.Push(tempStack.Pop());
            }
        }            

        // add the new object to the undo stack
        undoStack.Push(boxplotData);

        // clear the redo stack
        redoStack.Clear();
    }

    private void OnReceiverGotFocus(TextBox? receiver)
    {
        ArgumentNullException.ThrowIfNull(receiver, nameof(receiver));

        // todo hardcoded column names for testing
        List<string> ColumnNamesAll= new() { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };
        List<string> ColumnNamesFactor = new() { "village", "variety", "fertgrp" };
        List<string> ColumnNamesNonFactor = new() { "field", "size", "fert", "yield" };

        // note: We only change ColumnNames when it is essential.
        //       This is to avoid triggering unnecessary undo/redo snapshots
        _selectorMediator.SetFocus(receiver);
        if (receiver == _factorTextBox)
        {
            if (!ColumnNames.SequenceEqual(ColumnNamesAll))
                ColumnNames = ColumnNamesAll;
        }
        else if (receiver == _secondFactorTextBox || receiver == _facetByTextBox)
        {
            if (!ColumnNames.SequenceEqual(ColumnNamesFactor))
                ColumnNames = ColumnNamesFactor;
        }
        else if (!ColumnNames.SequenceEqual(ColumnNamesNonFactor))
            ColumnNames = ColumnNamesNonFactor;

        _columnsListBox.SelectionMode = SelectionMode.AlwaysSelected | SelectionMode.Toggle;
        if (receiver == _multipleVariableTextBox)
        {
            _addAllOption.IsEnabled = true;
            _columnsListBox.SelectionMode |= SelectionMode.Multiple;
        }
        else
        {
            _addAllOption.IsEnabled = false;
            _columnsListBox.SelectionMode |= SelectionMode.Single;
        }
    }

    private void OnReceiverKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                textBox.Clear();
            }
        }
    }

    //todo
    private void OnRedoClick()
    {
        if (redoStack.Count < 1)
            return;

        // find the state that we want to redo to
        BoxplotDataTransfer boxplotData = redoStack.Peek();

        // move this state to the undo stack (so we can undo it later)
        undoStack.Push(redoStack.Pop());

        // suspend data snapshots during redo
        isSnapshotActive = false;

        SetStateFromTransferObject(boxplotData);

        isSnapshotActive = true;
    }

    private void OnResetClick()
    {
        // suspend data snapshots during reset (snapshots are used for undo and redo)
        isSnapshotActive = false;

        BoxplotDataTransfer boxplotData = new BoxplotDataTransfer();
        boxplotData.GroupToConnectSummary = GroupToConnectSummaries[0];
        boxplotData.FacetByType = FacetByTypes[0];
        boxplotData.LegendPosition = LegendPositions[0];
        SetStateFromTransferObject(boxplotData);

        // resume data snapshots and ensure this reset is stored in a snaphot
        isSnapshotActive = true;
        OnPropertyChangedAction();
    }

    private void OnSelectorAddAllClick()
    {
        Selection.SelectAll();
        IReadOnlyList<string?> selectedItems = Selection.SelectedItems;
        _selectorMediator.AddSelectedValueToReceiver(selectedItems);
    }

    private void OnSelectorAddClick()
    {
        string selectedValue = Selection.SelectedItem ?? throw new Exception("Selected value in column selector list is null");
        IReadOnlyList<string?> selectedItems = Selection.SelectedItems;
        _selectorMediator.AddSelectedValueToReceiver(selectedItems);
    }

    private void OnToScriptClick()
    {
        Dictionary<string, string> dataBindings = new Dictionary<string, string>
        {
            {"comment", "# " + Comment + "\n\n"},
            {"dataFrame", DataFrame},
            {"facetBy", FacetBy},
            {"facetByType", FacetByType},
            {"inputSummaries", GroupToConnectSummary},
            {"inputWidth", Width},
            {"isAddPoints", BoolToUpperCaseString(IsAddPoints)},
            {"isBoxPlot", BoolToUpperCaseString(IsBoxPlot)},
            {"isBoxPlotExtra", BoolToUpperCaseString(IsBoxPlotExtra)},
            {"isComment", BoolToUpperCaseString(IsComment)},
            {"isFactorFactor", BoolToUpperCaseString(GetIsFactorFactor())},
            {"isGroupToConnect", BoolToUpperCaseString(IsGroupToConnect)},
            {"isHorizontalBoxPlot", BoolToUpperCaseString(IsHorizontalBoxPlot)},
            {"isJitter", BoolToUpperCaseString(IsJitter)},
            {"isLegend", BoolToUpperCaseString(IsLegend)},
            {"isSaveGraph", BoolToUpperCaseString(IsSaveGraph)},
            {"isSingleVariable", BoolToUpperCaseString(IsSingle)},
            {"isTufte", BoolToUpperCaseString(IsTufte)},
            {"isVarWidth", BoolToUpperCaseString(IsVarWidth)},
            {"isViolin", BoolToUpperCaseString(IsViolin)},
            {"isWidth", BoolToUpperCaseString(GetIsFactorNumeric() && IsWidth)},
            {"jitterExtra", JitterExtra},
            {"legendPosition", $"\"{LegendPosition.ToLower()}\""},
            {"saveName", SaveName},
            {"secondFactor", SecondFactor},
            {"transparency", Transparency},
            {"variableNames", MultipleVariables},
            {"widthExtra", WidthExtra},
            {"x", Factor},
            {"y", SingleVariable}
        };

        string rScript = TransformationUtilities.GetRScript("BoxPlot", dataBindings);

        //todo write dict and script to file for debugging -------
        string dataBindingsSummary = "";
        foreach (var kvp in dataBindings)
        {
            dataBindingsSummary += $"Key: {kvp.Key}, Value: {kvp.Value}\n";
        }

        string strDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string strFilePath = Path.Combine(strDesktopPath, "tmp", "dictActual.txt");
        dataBindingsSummary += "\n\n\n";

        if (File.Exists(strFilePath))
        {
            File.AppendAllText(strFilePath, dataBindingsSummary);
        }
        else
        {
            string? directory = Path.GetDirectoryName(strFilePath);
            if (directory != null)
                Directory.CreateDirectory(directory);

            File.WriteAllText(strFilePath, dataBindingsSummary);
        }

        //write the actual script (script built from the R model)
        strFilePath = Path.Combine(strDesktopPath, "tmp", "actual.R");
        rScript = $"\n\n\n {rScript}";
        if (File.Exists(strFilePath))
        {
            File.AppendAllText(strFilePath, rScript);
        }
        else
        {
            string? directory = Path.GetDirectoryName(strFilePath);
            if (directory != null)
                Directory.CreateDirectory(directory);

            File.WriteAllText(strFilePath, rScript);
        }
        //todo end -------
    }

    //todo
    private void OnUndoClick()
    {
        if (undoStack.Count < 2)
            return;

        // move the current state from the undo stack to the redo stack
        redoStack.Push(undoStack.Pop());

        // find the state that we want to undo to
        BoxplotDataTransfer boxplotData = undoStack.Peek();

        // suspend data snapshots during undo
        isSnapshotActive = false;

        SetStateFromTransferObject(boxplotData);

        isSnapshotActive = true;
    }

    private void SetStateFromTransferObject(BoxplotDataTransfer boxplotData)
    {
        Comment = boxplotData.Comment;
        DataFrame = boxplotData.DataFrame;
        FacetBy = boxplotData.FacetBy;
        FacetByType = boxplotData.FacetByType;
        Factor = boxplotData.Factor;
        GroupToConnectSummary = boxplotData.GroupToConnectSummary;
        IsAddPoints = boxplotData.IsAddPoints;
        IsBoxPlot = boxplotData.IsBoxPlot;
        IsBoxPlotExtra = boxplotData.IsBoxPlotExtra;
        IsComment = boxplotData.IsComment;
        IsGroupToConnect = boxplotData.IsGroupToConnect;
        IsHorizontalBoxPlot = boxplotData.IsHorizontalBoxPlot;
        IsJitter = boxplotData.IsJitter;
        IsLegend = boxplotData.IsLegend;
        IsSaveGraph = boxplotData.IsSaveGraph;
        IsSingle = boxplotData.IsSingle;
        IsTufte = boxplotData.IsTufte;
        IsVarWidth = boxplotData.IsVarWidth;
        IsViolin = boxplotData.IsViolin;
        IsWidth = boxplotData.IsWidth;
        JitterExtra = boxplotData.JitterExtra;
        LegendPosition = boxplotData.LegendPosition;
        MultipleVariables = boxplotData.MultipleVariables;
        SaveName = boxplotData.SaveName;
        SecondFactor = boxplotData.SecondFactor;
        SelectedTabIndex = boxplotData.SelectedTabIndex;
        SingleVariable = boxplotData.SingleVariable;
        Transparency = boxplotData.Transparency;
        Width = boxplotData.Width;
        WidthExtra = boxplotData.WidthExtra;
    }


}
