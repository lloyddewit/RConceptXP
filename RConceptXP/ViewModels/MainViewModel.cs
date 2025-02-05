using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using System;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }
    public IRelayCommand OpenDataOptionsCommand { get; }
    public IRelayCommand ResetDialogFromLogCommand { get; }

    private TabsDynamicView _dialogTabs;

    [ObservableProperty]
    private bool _dataOptionsTabExists;

    private TextBox _logTextBox;

    public MainViewModel(MainView mainView)
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);
        OpenDataOptionsCommand = new RelayCommand(OpenDataOptions);
        ResetDialogFromLogCommand = new RelayCommand(ResetDialogFromLog);

        _dialogTabs = mainView.FindControl<TabsDynamicView>("tabsForDialogs") ??
            throw new Exception("Cannot find tabsForDialogs by name");

        _dialogTabs.MainView = mainView;
        _dialogTabs.MainViewModel = this;

        _dialogTabs.TabDeleted += OnTabDeleted;

        _logTextBox = mainView.FindControl<TextBox>("logTextBox") ??
            throw new Exception("Cannot find logTextBox by name");

        DataOptionsTabExists = false;
    }

    private void OnDataOptionsOpened(object? sender, EventArgs e)
    {
        OpenDataOptions();        
    }

    private void OnTabDeleted(object? sender, EventArgs e)
    {
        if (sender is not TabsDynamicView tabDynamicView)
            return;

        if (e is TabDeletedEventArgs tabDeletedEventArgs 
                && tabDeletedEventArgs.Header == "DataOptions")
            DataOptionsTabExists = false;
    }

    private void OpenBoxplot()
    {
        BoxplotView newBoxplotView = _dialogTabs.GetNewBoxplotView(null);
        _dialogTabs.AddNewTab(newBoxplotView, "Boxplot");
        var boxplotViewModel = newBoxplotView.DataContext as BoxplotViewModel ??
            throw new Exception("Cannot find boxplotViewModel by name");
        boxplotViewModel.DataOptionsOpened += OnDataOptionsOpened;
    }

    private void OpenDataOptions()
    {
        if (DataOptionsTabExists)
            return;

        DataOptionsView newDataOptionsView = new DataOptionsView();
        _dialogTabs.AddNewTab(newDataOptionsView, "DataOptions", true);

        DataOptionsTabExists = true;
    }

    //todo
    private void ResetDialogFromLog()
    {
        var textPosition = _logTextBox.SelectionStart;
    }
}
