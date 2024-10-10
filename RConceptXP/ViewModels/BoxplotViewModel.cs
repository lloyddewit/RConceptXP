using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RConceptXP.ViewModels;

public partial class BoxplotViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isGroupToConnectVisible; // Property to control ComboBox visibility

    public BoxplotViewModel()
    {
        IsGroupToConnectVisible = false; // Initialize visibility state
    }
}
