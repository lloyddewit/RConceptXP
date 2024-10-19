using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using System;

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

    public RelayCommand<TextBox> SingleVariableGotFocusCommand { get; }
    public RelayCommand<TextBox> MultipleVariableGotFocusCommand { get; }
    public RelayCommand<TextBox> FactorGotFocusCommand { get; }
    public RelayCommand<TextBox> SecondFactorGotFocusCommand { get; }
    public RelayCommand<TextBox> FacetByGotFocusCommand { get; }

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
        SingleVariableGotFocusCommand = new RelayCommand<TextBox>(SingleVariableGotFocus);
        MultipleVariableGotFocusCommand = new RelayCommand<TextBox>(MultipleVariableGotFocus);
        FactorGotFocusCommand = new RelayCommand<TextBox>(FactorGotFocus);
        SecondFactorGotFocusCommand = new RelayCommand<TextBox>(SecondFactorGotFocus);
        FacetByGotFocusCommand = new RelayCommand<TextBox>(FacetByGotFocus);
    }

    private void SingleVariableGotFocus(TextBox? receiver)
    {
        //if receiver is null then raise exception
        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        receiver.FontWeight = Avalonia.Media.FontWeight.Bold;
        receiver.Background = Avalonia.Media.Brushes.LightYellow;
        SetColumnNamesNonFactor();
    }

    private void MultipleVariableGotFocus(TextBox? receiver)
    {
        //if receiver is null then raise exception
        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        receiver.FontWeight = Avalonia.Media.FontWeight.Bold;
        receiver.Background = Avalonia.Media.Brushes.LightYellow;
        SetColumnNamesNonFactor();
    }

    private void FactorGotFocus(TextBox? receiver)
    {
        //if receiver is null then raise exception
        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        receiver.FontWeight = Avalonia.Media.FontWeight.Bold;
        receiver.Background = Avalonia.Media.Brushes.LightYellow;
        SetColumnNamesAll();
    }

    private void SecondFactorGotFocus(TextBox? receiver)
    {
        //if receiver is null then raise exception
        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        receiver.FontWeight = Avalonia.Media.FontWeight.Bold;
        receiver.Background = Avalonia.Media.Brushes.LightYellow;
        SetColumnNamesFactor();
    }

    private void FacetByGotFocus(TextBox? receiver)
    {
        //if receiver is null then raise exception
        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }
        receiver.FontWeight = Avalonia.Media.FontWeight.Bold;
        receiver.Background = Avalonia.Media.Brushes.LightYellow;
        SetColumnNamesFactor();
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
