using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using RInsightF461;
using System;
using System.Collections.Generic;
using System.Linq;

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
    private ScrollViewer _logScrollViewer;

    // note: considered using an OrderedDictionary but on balance chose a Dictionary
    private Dictionary<int, BoxplotDataTransfer> dialogStates;

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
        _logScrollViewer = mainView.FindControl<ScrollViewer>("logScrollViewer") ??
            throw new Exception("Cannot find factor logScrollViewer by name");

        dialogStates = new Dictionary<int, BoxplotDataTransfer>();

        DataOptionsTabExists = false;
    }

    internal void WriteToLog(string message, BoxplotDataTransfer boxplotData)
    {
        int textLength = _logTextBox.Text == null ? 0 : _logTextBox.Text.Length;
        dialogStates[textLength] = boxplotData;

        _logTextBox.Text += message
            + Environment.NewLine + Environment.NewLine + Environment.NewLine;
        _logScrollViewer.ScrollToEnd();
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

    private void ResetDialogFromLog()
    {
        int textPosition = _logTextBox.SelectionStart;

        // Find the largest key that is less than or equal to textPosition
        int closestKey = dialogStates.Keys.Where(key => key <= textPosition).DefaultIfEmpty(-1).Max();
        if (closestKey == -1)
            return;

        TabsDynamicViewModel? openTabViewModel = _dialogTabs.GetCurrentOpenTab();
        BoxplotViewModel? boxplotViewModel = openTabViewModel?.GetBoxplotViewModel();
        boxplotViewModel?.SetStateFromTransferObject(dialogStates[closestKey]);
    }
}
