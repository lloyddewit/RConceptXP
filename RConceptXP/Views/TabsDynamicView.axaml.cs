using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RConceptXP.Views;

public partial class TabsDynamicView : UserControl
{
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

        // if the new tab is a singleton, check if it already exists
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

    public BoxplotView GetNewBoxplotView(BoxplotViewModel? boxplotToDuplicate = null)
    {
        var newBoxplotView = new BoxplotView(boxplotToDuplicate);
        var newBoxplotViewModel = newBoxplotView.DataContext as BoxplotViewModel;
        if (newBoxplotViewModel is null)
        {
            throw new Exception("DataContext of newBoxplotView is not BoxplotViewModel");
        }
        newBoxplotViewModel.DataOptionsOpened += OnDataOptionsOpened;
        return newBoxplotView;
    }

    private void OnDataOptionsOpened(object? sender, EventArgs e)
    {
        DataOptionsView newDataOptionsView = new DataOptionsView();
        AddNewTab(newDataOptionsView, "DataOptions", true);
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
