using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using System;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }
    public IRelayCommand OpenDataOptionsCommand { get; }

    private TabsDynamicView _dialogTabs;

    public MainViewModel(MainView mainView)
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);
        OpenDataOptionsCommand = new RelayCommand(OpenDataOptions);

        _dialogTabs = mainView.FindControl<TabsDynamicView>("tabsForDialogs") ??
            throw new Exception("Cannot find tabsForDialogs by name");
    }

    private void OpenBoxplot()
    {
        BoxplotView newBoxplotView = _dialogTabs.GetNewBoxplotView(null);
        _dialogTabs.AddNewTab(newBoxplotView, "Boxplot");
    }

    private void OpenDataOptions()
    {
        DataOptionsView newDataOptionsView = new DataOptionsView();
        _dialogTabs.AddNewTab(newDataOptionsView, "DataOptions", true);
    }
}
