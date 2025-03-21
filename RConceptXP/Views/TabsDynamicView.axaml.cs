using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RConceptXP.Views;

public partial class TabsDynamicView : UserControl
{
    public MainView? MainView = null;
    public MainViewModel? MainViewModel = null;

    public event EventHandler? TabDeleted;

    private ObservableCollection<TabsDynamicViewModel> _tabViewModels;    

    public TabsDynamicView()
    {
        AvaloniaXamlLoader.Load(this);

        _tabViewModels = new ObservableCollection<TabsDynamicViewModel> { };
        DataContext = _tabViewModels;
    }
    
    public void AddNewTab(UserControl newView, string titleStem, bool isSingleton = false)
    {
        var tabControl = this.FindControl<TabControl>("tabs") ??
            throw new Exception("Cannot find tabs by name");

        // create a list of headers to check for duplicates
        var headers = new List<string>();
        foreach (var item in tabControl.Items)
        {
            if (item is TabsDynamicViewModel viewModel)
            {
                headers.Add(viewModel.Header);
            }
        }

        // if the new tab is a singleton and it already exists, then do nothing
        if (isSingleton && headers.Contains(titleStem))
            return;

        // for non-singletons, create a unique header
        string header = titleStem;
        if (!isSingleton)
        {
            int count = 1;
            do
            {
                header = $"{titleStem}{count}";
                count++;
            } while (headers.Contains(header));
        }

        // create and add the new tab
        var newTab = new TabsDynamicViewModel(header, newView);

        newTab.IsNewCommandEnabled = !isSingleton;
        newTab.IsDuplicateCommandEnabled = !isSingleton;

        newTab.TabCreated += OnTabCreated;
        newTab.TabDuplicated += OnTabDuplicated;
        newTab.TabDeleted += OnTabDeleted;
        _tabViewModels.Add(newTab);
        tabControl.SelectedIndex = _tabViewModels.Count - 1;
    }

    public TabsDynamicViewModel? GetCurrentOpenTab()
    {
        var tabControl = this.FindControl<TabControl>("tabs") ??
            throw new Exception("Cannot find tabs by name");

        return tabControl.SelectedItem as TabsDynamicViewModel;
    }

    public BoxplotView GetNewBoxplotView(BoxplotViewModel? boxplotToDuplicate = null)
    {
        if (MainView is null)
        {
            throw new Exception("MainView is null");
        }

        if (MainViewModel is null)
        {
            throw new Exception("MainViewModel is null");
        }

        var newBoxplotView = new BoxplotView(MainView, MainViewModel, boxplotToDuplicate);
        var newBoxplotViewModel = newBoxplotView.DataContext as BoxplotViewModel;
        if (newBoxplotViewModel is null)
        {
            throw new Exception("DataContext of newBoxplotView is not BoxplotViewModel");
        }
        return newBoxplotView;
    }

    private void OnTabCreated(object? sender, EventArgs e)
    {
        if (sender is not TabsDynamicViewModel tabDynamicViewModel)
            return;

        if (tabDynamicViewModel.TabViewModel is BoxplotViewModel)
        {
            BoxplotView newBoxplotView = GetNewBoxplotView();
            AddNewTab(newBoxplotView, "Boxplot");
        }
    }

    private void OnTabDeleted(object? sender, EventArgs e)
    {
        if (sender is not TabsDynamicViewModel tabDynamicViewModel)
            return;

        _tabViewModels.Remove(tabDynamicViewModel);

        TabDeleted?.Invoke(this, new TabDeletedEventArgs(tabDynamicViewModel.Header));
    }

    private void OnTabDuplicated(object? sender, EventArgs e)
    {
        if (sender is not TabsDynamicViewModel tabDynamicViewModel)
            return;

        if (tabDynamicViewModel.TabViewModel is BoxplotViewModel)
        {
            UserControl newView = GetNewBoxplotView((BoxplotViewModel?)tabDynamicViewModel.TabViewModel);
            AddNewTab(newView, "Boxplot");
        }
    }
}
