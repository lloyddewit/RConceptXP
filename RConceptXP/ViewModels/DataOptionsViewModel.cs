using Avalonia.Controls.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace RConceptXP.ViewModels;

//todo support empty list of columns (test that selector buttons enable/disable correctly and no crashes)
public partial class DataOptionsViewModel : ObservableObject
{
    [ObservableProperty]
    private List<string> _columnNames = [];

    [ObservableProperty]
    private string _filter;

    [ObservableProperty]
    private bool _isColumns;
    [ObservableProperty]
    private bool _isFilterSelected;
    [ObservableProperty]
    private bool _isRows;

    public RelayCommand OnSelectorAddClickCommand { get; }
    public RelayCommand OnSelectorRemoveFilterClickCommand { get; }
    public SelectionModel<string> Selection { get; }

    public DataOptionsViewModel()
    {
        // todo hardcoded column names for testing
        List<string> ColumnNamesAll = new() { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };

        ColumnNames = ColumnNamesAll;
        Filter = "";
        IsColumns = false;
        IsFilterSelected = false;
        IsRows = true;
        OnSelectorAddClickCommand = new RelayCommand(OnSelectorAddClick);
        OnSelectorRemoveFilterClickCommand = new RelayCommand(OnSelectorRemoveFilterClick);
        Selection = new SelectionModel<string>();
    }

    private void OnSelectorAddClick()
    {
        string selectedValue = Selection.SelectedItem ?? throw new Exception("Selected value in column selector list is null");
        IReadOnlyList<string?> selectedItems = Selection.SelectedItems;
    }

    private void OnSelectorRemoveFilterClick()
    {
        //todo implement
    }

}
