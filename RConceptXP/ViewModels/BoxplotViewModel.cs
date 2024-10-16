using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RConceptXP.ViewModels;

public partial class BoxplotViewModel : ObservableObject
{
    [ObservableProperty]
    private List<string> columnNames; // List of data frame column names

    [ObservableProperty]
    private string graphName; // Selected group to connect

    [ObservableProperty]
    private List<string> graphNames; // List of groups to connect

    [ObservableProperty]
    private bool isGroupToConnectVisible; // Property to control ComboBox visibility

    public RelayCommand SetColumnNamesAllCommand { get; }
    public RelayCommand SetColumnNamesFactorCommand { get; }
    public RelayCommand SetColumnNamesNonFactorCommand { get; }

    // todo hardcoded column names for testing
    private static readonly List<string> ColumnNamesAll = new List<string> { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };
    private static readonly List<string> ColumnNamesFactor = new List<string> { "village", "variety", "fertgrp" };
    private static readonly List<string> ColumnNamesNonFactor = new List<string> { "field", "size", "fert", "yield" };


    public BoxplotViewModel()
    {
        ColumnNames = ColumnNamesNonFactor; // Initialize list of data frame column names
        GraphName = "plot1"; // Initialize selected group to connect
        GraphNames = new List<string> { "box_plot", "jitter", "violin" }; // Initialize list of groups to connect
        IsGroupToConnectVisible = false; // Initialize visibility state
        SetColumnNamesAllCommand = new RelayCommand(SetColumnNamesAll);
        SetColumnNamesFactorCommand = new RelayCommand(SetColumnNamesFactor);
        SetColumnNamesNonFactorCommand = new RelayCommand(SetColumnNamesNonFactor);
    }

    private void SetColumnNamesAll()
    {
        ColumnNames = new List<string>(ColumnNamesFactor);
        ColumnNames.AddRange(ColumnNamesNonFactor);
    }

    private void SetColumnNamesFactor()
    {
        ColumnNames = ColumnNamesFactor;
    }

    private void SetColumnNamesNonFactor()
    {
        ColumnNames = ColumnNamesNonFactor;
    }

}
