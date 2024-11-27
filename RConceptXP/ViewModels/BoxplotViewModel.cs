using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.TextFormatting.Unicode;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using RConceptXP.Views;
using RInsightF461;
using RConceptXP.Services;
using System.Collections.Generic;
using System.IO;
using System;

namespace RConceptXP.ViewModels;

//todo support empty list of columns (test that selector buttons enable/disable correctly and no crashes)
public partial class BoxplotViewModel : ObservableObject
{
    public RelayCommand<int> OnMainTabRightClickCommand { get; }
    public RelayCommand<TextBox> OnReceiverGotFocusCommand { get; }
    public RelayCommand OnResetClickCommand { get; }
    public RelayCommand OnSelectorAddAllClickCommand { get; }
    public RelayCommand OnSelectorAddClickCommand { get; }
    public RelayCommand OnToScriptClickCommand { get; }

    public bool IsOkEnabled => GetIsOkEnabled();
    public bool IsWidthEnabled => GetIsFactorNumeric();
    public SelectionModel<string> Selection { get; }


    [ObservableProperty]
    private List<string> _columnNames = [];

    [ObservableProperty]
    private string _factor;
    partial void OnFactorChanged(string value) => OnPropertyChanged(nameof(IsWidthEnabled));

    [ObservableProperty]
    private string _saveName;
    partial void OnSaveNameChanged(string value) => OnPropertyChanged(nameof(IsOkEnabled));

    [ObservableProperty]
    private List<string> _saveNames;

    [ObservableProperty]
    private bool _isSaveGraph;
    partial void OnIsSaveGraphChanged(bool value) => OnPropertyChanged(nameof(IsOkEnabled));

    [ObservableProperty]
    private bool _isSingle;
    partial void OnIsSingleChanged(bool value) => OnPropertyChanged(nameof(IsOkEnabled));

    [ObservableProperty]
    private string _singleVariable;
    partial void OnSingleVariableChanged(string value) => OnPropertyChanged(nameof(IsOkEnabled));

    [ObservableProperty]
    private string _multipleVariables;
    partial void OnMultipleVariablesChanged(string value) => OnPropertyChanged(nameof(IsOkEnabled));


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
    private string _secondFactor;

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private string _transparency;

    [ObservableProperty]
    private string _width;

    [ObservableProperty]
    private string _widthExtra;


    private SelectorMediator _selectorMediator;

    private ListBox _columnsListBox;
    private MenuItem _addAllOption;

    private TextBox _singleVariableTextBox;
    private TextBox _multipleVariableTextBox;
    private TextBox _factorTextBox;
    private TextBox _secondFactorTextBox;
    private TextBox _facetByTextBox;

    // Disable the warning 'non-nullable field must contain a non-null value when exiting
    // constructor'. We need to disable because otherwise many incorrect warnings are generated
    // for the observable properties that are initialised in the constructor via the
    // 'OnResetClick' method.
#pragma warning disable CS8618
    public BoxplotViewModel(BoxplotView boxplotView, BoxplotViewModel? boxplotToDuplicate)
#pragma warning restore CS8618
    {
        // initialize receiver controls
        OnReceiverGotFocusCommand = new RelayCommand<TextBox>(OnReceiverGotFocus);

        _columnsListBox = boxplotView.FindControl<ListBox>("columns") ??
            throw new Exception("Cannot find columns ListBox by name");
        _addAllOption = boxplotView.FindControl<MenuItem>("addAllOption") ??
            throw new Exception("Cannot find addAllOption MenuItem by name");
        _singleVariableTextBox = boxplotView.FindControl<TextBox>("singleVariableTextBox") ??
            throw new Exception("Cannot find singleVariableTextBox by name");
        _multipleVariableTextBox = boxplotView.FindControl<TextBox>("multipleVariableTextBox") ??
            throw new Exception("Cannot find multipleVariableTextBox by name");
        _factorTextBox = boxplotView.FindControl<TextBox>("factorTextBox") ??
            throw new Exception("Cannot find factor textBox by name");
        _secondFactorTextBox = boxplotView.FindControl<TextBox>("secondFactorTextBox") ??
            throw new Exception("Cannot find secondFactorTextBox by name");
        _facetByTextBox = boxplotView.FindControl<TextBox>("facetByTextBox") ??
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
        OnMainTabRightClickCommand = new RelayCommand<int>(OnMainTabRightClick);
        OnToScriptClickCommand = new RelayCommand(OnToScriptClick);
        OnResetClickCommand = new RelayCommand(OnResetClick);

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

    private void OnMainTabRightClick(int tabIndex)
    {
        SelectedTabIndex = tabIndex;
    }

    private void OnReceiverGotFocus(TextBox? receiver)
    {
        ArgumentNullException.ThrowIfNull(receiver, nameof(receiver));

        // todo hardcoded column names for testing
        List<string> ColumnNamesAll= new() { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };
        List<string> ColumnNamesFactor = new() { "village", "variety", "fertgrp" };
        List<string> ColumnNamesNonFactor = new() { "field", "size", "fert", "yield" };

        _selectorMediator.SetFocus(receiver);
        if (receiver == _factorTextBox)
        {
            ColumnNames = ColumnNamesAll;
        }
        else if (receiver == _secondFactorTextBox || receiver == _facetByTextBox)
            ColumnNames = ColumnNamesFactor;
        else
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

    private void OnResetClick()
    {
        Comment = "Dialog: Boxplot Options";
        DataFrame = "survey"; // todo hard coded for testing
        FacetBy = "";
        FacetByType = FacetByTypes[0];
        Factor = "";
        GroupToConnectSummary = GroupToConnectSummaries[0];
        IsAddPoints = false;
        IsBoxPlot = true;
        IsBoxPlotExtra = false;
        IsComment = true;
        IsGroupToConnect = false;
        IsHorizontalBoxPlot = false;
        IsJitter = false;
        IsLegend = false;
        IsSaveGraph = false;
        IsSingle = true;
        IsTufte = false;
        IsVarWidth = false;
        IsViolin = false;
        IsWidth = false;
        JitterExtra = "0.20";
        LegendPosition = LegendPositions[0];
        MultipleVariables = "";
        SaveName = "plot1";
        SecondFactor = "";
        SelectedTabIndex = 0;
        SingleVariable = "";
        Transparency = "1.00";
        Width = "0.25";
        WidthExtra = "0.5";
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
}
