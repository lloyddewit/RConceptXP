using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }

    private TabControlDynamicView _dialogTabs;

    public MainViewModel(MainView mainView)
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);

        _dialogTabs = mainView.FindControl<TabControlDynamicView>("dialogTabs") ??
            throw new Exception("Cannot find dialogTabs by name");

    }

    private void OpenBoxplot()
    {
        _dialogTabs.AddNewTab();
    }
}
