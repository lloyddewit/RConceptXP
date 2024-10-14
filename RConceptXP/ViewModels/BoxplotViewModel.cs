using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace RConceptXP.ViewModels;

public partial class BoxplotViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isGroupToConnectVisible; // Property to control ComboBox visibility

    [ObservableProperty]
    private List<string> savedGraphNames; // List of groups to connect

    [ObservableProperty]
    private string graphName; // Selected group to connect

    public BoxplotViewModel()
    {
        GraphName = "box_plot"; // Initialize selected group to connect
        IsGroupToConnectVisible = false; // Initialize visibility state
        SavedGraphNames = new List<string> { "box_plot", "jitter", "violin" }; // Initialize list of groups to connect
    }
}
