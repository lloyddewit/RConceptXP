using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using System;
using Avalonia.Controls.Selection;

namespace RConceptXP.ViewModels;

//todo support empty list of columns (test that selector buttons enable/disable correctly and no crashes)
public partial class BoxplotViewModel : ObservableObject
{
    [ObservableProperty]
    private List<string> _columnNames; // List of data frame column names

    [ObservableProperty]
    private string graphName; // Selected group to connect

    [ObservableProperty]
    private List<string> graphNames; // List of groups to connect

    [ObservableProperty]
    private bool isGroupToConnectVisible; // Property to control ComboBox visibility

    public RelayCommand<TextBox> SingleVariableGotFocusCommand { get; }
    public RelayCommand<TextBox> MultipleVariableGotFocusCommand { get; }
    public RelayCommand<TextBox> FactorGotFocusCommand { get; }
    public RelayCommand<TextBox> SecondFactorGotFocusCommand { get; }
    public RelayCommand<TextBox> FacetByGotFocusCommand { get; }
    public RelayCommand OnSelectorAddClickCommand { get; }
    public RelayCommand OnSelectorAddAllClickCommand { get; }

    public SelectionModel<string> Selection { get; }

    // todo hardcoded column names for testing
    private static readonly List<string> ColumnNamesAll = new List<string> { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };
    private static readonly List<string> ColumnNamesFactor = new List<string> { "village", "variety", "fertgrp" };
    private static readonly List<string> ColumnNamesNonFactor = new List<string> { "field", "size", "fert", "yield" };

    private SelectorMediator selectorMediator;

    private ListBox columnsListBox;
    private MenuItem addAllOption;

    public BoxplotViewModel(Boxplot boxplot)
    {
        ColumnNames = ColumnNamesNonFactor; // Initialize list of data frame column names
        GraphName = "plot1"; // Initialize selected group to connect
        GraphNames = new List<string> { "box_plot", "jitter", "violin" }; // Initialize list of groups to connect
        IsGroupToConnectVisible = false; // Initialize visibility state
        SingleVariableGotFocusCommand = new RelayCommand<TextBox>(SingleVariableGotFocus);
        MultipleVariableGotFocusCommand = new RelayCommand<TextBox>(MultipleVariableGotFocus);
        FactorGotFocusCommand = new RelayCommand<TextBox>(FactorGotFocus);
        SecondFactorGotFocusCommand = new RelayCommand<TextBox>(SecondFactorGotFocus);
        FacetByGotFocusCommand = new RelayCommand<TextBox>(FacetByGotFocus);


        List<TextBox> receivers = new List<TextBox>
        {
            boxplot.FindControl<TextBox>("singleVariableTextBox") ?? throw new Exception("Cannot find singleVariableTextBox by name"),
            boxplot.FindControl<TextBox>("multipleVariableTextBox") ?? throw new Exception("Cannot find multipleVariableTextBox by name"),
            boxplot.FindControl<TextBox>("factorTextBox") ?? throw new Exception("Cannot find factorTextBox by name"),
            boxplot.FindControl<TextBox>("secondFactorTextBox") ?? throw new Exception("Cannot find secondFactorTextBox by name"),
            boxplot.FindControl<TextBox>("facetByTextBox") ?? throw new Exception("Cannot find facetByTextBox by name")
        };

        selectorMediator = new SelectorMediator(receivers);

        OnSelectorAddClickCommand = new RelayCommand(OnSelectorAddClick);
        OnSelectorAddAllClickCommand = new RelayCommand(OnSelectorAddAllClick);

        Selection = new SelectionModel<string>();

        columnsListBox = boxplot.FindControl<ListBox>("columns") ?? throw new Exception("Cannot find columns ListBox by name");
        addAllOption = boxplot.FindControl<MenuItem>("addAllOption") ?? throw new Exception("Cannot find addAllOption MenuItem by name");

        //// ensure initial input focus is the single variable text box
        //TextBox singleVariableTextBox = boxplot.FindControl<TextBox>("singleVariableTextBox") ?? throw new Exception("Cannot find singleVariableTextBox by name");
        //singleVariableTextBox.AttachedToVisualTree += (s, e) => singleVariableTextBox.Focus();
    }

    private void SingleVariableGotFocus(TextBox receiver)
    {
        selectorMediator.SetFocus(receiver);
        SetColumnNamesNonFactor();
        columnsListBox.SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected | SelectionMode.Toggle;
        addAllOption.IsEnabled = false;
    }

    private void MultipleVariableGotFocus(TextBox receiver)
    {
        selectorMediator.SetFocus(receiver);
        SetColumnNamesNonFactor();
        columnsListBox.SelectionMode = SelectionMode.Multiple | SelectionMode.AlwaysSelected | SelectionMode.Toggle;
        addAllOption.IsEnabled = true;
    }

    private void FactorGotFocus(TextBox receiver)
    {
        selectorMediator.SetFocus(receiver);
        SetColumnNamesAll();
        columnsListBox.SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected | SelectionMode.Toggle;
        addAllOption.IsEnabled = false;
    }

    private void SecondFactorGotFocus(TextBox receiver)
    {
        selectorMediator.SetFocus(receiver);
        SetColumnNamesFactor();
        columnsListBox.SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected | SelectionMode.Toggle;
        addAllOption.IsEnabled = false;
    }

    private void FacetByGotFocus(TextBox receiver)
    {
        selectorMediator.SetFocus(receiver);
        SetColumnNamesFactor();
        columnsListBox.SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected | SelectionMode.Toggle;
        addAllOption.IsEnabled = false;
    }

    private void OnSelectorAddClick()
    {
        string selectedValue = Selection.SelectedItem ?? throw new Exception("Selected value in column selector list is null");
        IReadOnlyList<string?> selectedItems = Selection.SelectedItems;
        selectorMediator.AddSelectedValueToReceiver(selectedItems);
    }

    private void OnSelectorAddAllClick()
    {
        Selection.SelectAll();
        IReadOnlyList<string?> selectedItems = Selection.SelectedItems;
        selectorMediator.AddSelectedValueToReceiver(selectedItems);
    }

    private void SetColumnNamesAll()
    {
        ColumnNames = new List<string>(ColumnNamesFactor);
        ColumnNames.AddRange(ColumnNamesNonFactor);
        // Additional logic using textBlock if needed
    }

    private void SetColumnNamesFactor()
    {
        ColumnNames = ColumnNamesFactor;
        // Additional logic using textBlock if needed
    }

    private void SetColumnNamesNonFactor()
    {
        ColumnNames = ColumnNamesNonFactor;
        // Additional logic using textBlock if needed
    }
}
