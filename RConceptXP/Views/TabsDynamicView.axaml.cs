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

    public void AddNewTab(UserControl newView, string titleStem)
    {
        var tabControl = this.FindControl<TabControl>("tabs") ??
            throw new Exception("Cannot find tabs by name");

        var headers = new List<string>();
        foreach (var item in tabControl.Items)
        {
            if (item is TabsDynamicViewModel viewModel)
            {
                headers.Add(viewModel.Header);
            }
        }

        int count = 1;
        string header = "";
        do
        {
            header = $"{titleStem}{count}";
            count++;
        } while (headers.Contains(header));

        var newTab = new TabsDynamicViewModel(header, newView);
        newTab.TabCreated += OnTabCreated;
        newTab.TabDeleted += OnTabDeleted;
        newTab.TabDuplicated += OnTabDuplicated;
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
