using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using System;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }

    private TabsDynamicView _dialogTabs;

    public MainViewModel(MainView mainView)
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);

        _dialogTabs = mainView.FindControl<TabsDynamicView>("tabsForDialogs") ??
            throw new Exception("Cannot find tabsForDialogs by name");
    }

    private void OpenBoxplot()
    {
        _dialogTabs.AddNewTab();
    }
}
