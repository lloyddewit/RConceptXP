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

    public RelayCommand OnFactorTextBoxGotFocusCommand { get; }

    public BoxplotViewModel()
    {
        ColumnNames = new List<string> { "field", "size", "fert", "yield" }; // Initialize list of data frame column names
        GraphName = "plot1"; // Initialize selected group to connect
        GraphNames = new List<string> { "box_plot", "jitter", "violin" }; // Initialize list of groups to connect
        IsGroupToConnectVisible = false; // Initialize visibility state
        OnFactorTextBoxGotFocusCommand = new RelayCommand(OnFactorTextBoxGotFocus);
    }

    private void OnFactorTextBoxGotFocus()
    {
        // Update the ColumnNames list
        ColumnNames = new List<string> { "village", "field", "size", "fert", "variety", "yield", "fertgrp" };
    }
    
}
