using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace RConceptXP.ViewModels;

//todo support empty list of columns (test that selector buttons enable/disable correctly and no crashes)
public partial class BoxplotViewModel : ObservableObject
{
    public RelayCommand<TextBox> OnReceiverGotFocusCommand { get; }
    public RelayCommand OnSelectorAddAllClickCommand { get; }
    public RelayCommand OnSelectorAddClickCommand { get; }

    public bool IsOkEnabled => GetIsOkEnabled();
    public bool IsWidthEnabled => GetIsWidthEnabled();
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
    private string _inputSummaries;

    [ObservableProperty]
    private string _inputWidth;

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
    private string _secondFactor;

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

    public BoxplotViewModel(Boxplot boxplot)
    {
        // initialize receiver controls
        OnReceiverGotFocusCommand = new RelayCommand<TextBox>(OnReceiverGotFocus);

        _columnsListBox = boxplot.FindControl<ListBox>("columns") ?? 
            throw new Exception("Cannot find columns ListBox by name");
        _addAllOption = boxplot.FindControl<MenuItem>("addAllOption") ?? 
            throw new Exception("Cannot find addAllOption MenuItem by name");
        _singleVariableTextBox = boxplot.FindControl<TextBox>("singleVariableTextBox") ?? 
            throw new Exception("Cannot find singleVariableTextBox by name");
        _multipleVariableTextBox = boxplot.FindControl<TextBox>("multipleVariableTextBox") ?? 
            throw new Exception("Cannot find singleVariableTextBox by name");
        _factorTextBox = boxplot.FindControl<TextBox>("factorTextBox") ?? 
            throw new Exception("Cannot find factor textBox by name");
        _secondFactorTextBox = boxplot.FindControl<TextBox>("secondFactorTextBox") ?? 
            throw new Exception("Cannot find singleVariableTextBox by name");
        _facetByTextBox = boxplot.FindControl<TextBox>("facetByTextBox") ?? 
            throw new Exception("Cannot find singleVariableTextBox by name");

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

        // intitialize graph name autocomplete box
        SaveName = "plot1"; // Initialize selected group to connect
        SaveNames = new List<string> { "box_plot", "jitter", "violin" }; // Initialize list of groups to connect

        // initialize other binding variables
        Factor = "";
        IsSaveGraph = true;
        IsSingle = true;
        SingleVariable = "";
        MultipleVariables = "";

        Comment = "Dialog: Boxplot Options";
        DataFrame = "survey"; // todo hard coded for testing
        FacetBy = "";
        FacetByType = "";
        InputSummaries = "";
        IsAddPoints = false;
        IsBoxPlot = true;
        IsBoxPlotExtra = false;
        IsComment = true;
        IsGroupToConnect = false;
        IsHorizontalBoxPlot = false;
        IsJitter = false;
        IsLegend = false;
        IsTufte = false;
        IsVarWidth = false;
        IsViolin = false;
        JitterExtra = "0.20";
        LegendPosition = "";
        SecondFactor = "";
        Transparency = "1.00";
        Width = "0.25";
        WidthExtra = "0.5";
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

    private bool GetIsWidthEnabled()
    {
        if (string.IsNullOrEmpty(Factor))
            return false;

        // todo hard-coded column names for testing
        if (Factor == "village" || Factor == "variety" || Factor == "fertgrp")
            return false;

        return true;
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
            _columnsListBox.SelectionMode |= SelectionMode.Single;
            _addAllOption.IsEnabled = false;
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
}
