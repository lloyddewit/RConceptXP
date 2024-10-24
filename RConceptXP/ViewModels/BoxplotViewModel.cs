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

    public SelectionModel<string> Selection { get; }

    [ObservableProperty]
    private List<string> _columnNames; // List of data frame column names

    [ObservableProperty]
    private string _graphName; // Selected group to connect

    [ObservableProperty]
    private List<string> _graphNames; // List of groups to connect

    // todo hardcoded column names for testing
    private static readonly List<string> ColumnNamesAll = new List<string> { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };
    private static readonly List<string> ColumnNamesFactor = new List<string> { "village", "variety", "fertgrp" };
    private static readonly List<string> ColumnNamesNonFactor = new List<string> { "field", "size", "fert", "yield" };

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

        _columnsListBox = boxplot.FindControl<ListBox>("columns") ?? throw new Exception("Cannot find columns ListBox by name");
        _addAllOption = boxplot.FindControl<MenuItem>("addAllOption") ?? throw new Exception("Cannot find addAllOption MenuItem by name");
        _singleVariableTextBox = boxplot.FindControl<TextBox>("singleVariableTextBox") ?? throw new Exception("Cannot find singleVariableTextBox by name");
        _multipleVariableTextBox = boxplot.FindControl<TextBox>("multipleVariableTextBox") ?? throw new Exception("Cannot find singleVariableTextBox by name");
        _factorTextBox = boxplot.FindControl<TextBox>("factorTextBox") ?? throw new Exception("Cannot find factor textBox by name");
        _secondFactorTextBox = boxplot.FindControl<TextBox>("secondFactorTextBox") ?? throw new Exception("Cannot find singleVariableTextBox by name");
        _facetByTextBox = boxplot.FindControl<TextBox>("facetByTextBox") ?? throw new Exception("Cannot find singleVariableTextBox by name");

        // Note: We need to catch delete and backspace key presses in receivers so that the user
        // can clear the receiver. There are simpler ways to catch most key events in Avalonia,
        // but delete and backspace are a special case and require a Tunnel routing strategy.
        _singleVariableTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown, RoutingStrategies.Tunnel);
        _multipleVariableTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown, RoutingStrategies.Tunnel);
        _factorTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown, RoutingStrategies.Tunnel);
        _secondFactorTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown, RoutingStrategies.Tunnel);
        _facetByTextBox.AddHandler(InputElement.KeyDownEvent, OnReceiverKeyDown, RoutingStrategies.Tunnel);


        // initialize selector controls
        ColumnNames = ColumnNamesNonFactor;
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
        GraphName = "plot1"; // Initialize selected group to connect
        GraphNames = new List<string> { "box_plot", "jitter", "violin" }; // Initialize list of groups to connect
    }

    private void OnReceiverGotFocus(TextBox? receiver)
    {
        ArgumentNullException.ThrowIfNull(receiver, nameof(receiver));

        _selectorMediator.SetFocus(receiver);
        if (receiver == _factorTextBox)
        {
            ColumnNames = new List<string>(ColumnNamesFactor);
            ColumnNames.AddRange(ColumnNamesNonFactor);
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
