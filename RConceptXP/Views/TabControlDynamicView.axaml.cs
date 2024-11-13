using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace RConceptXP.Views;

public partial class TabControlDynamicView : UserControl
{
    private ObservableCollection<TabItemViewModel> _tabItemViewModels;

    public TabControlDynamicView()
    {
        InitializeComponent();
        //DataContext = new TabControlDynamicViewModel();

        _tabItemViewModels = new ObservableCollection<TabItemViewModel>
        {
            new TabItemViewModel("One", "Some content on first tab"),
            new TabItemViewModel("Two", "Some content on second tab"),
        };
        DataContext = _tabItemViewModels;
    }

    public void AddNewTab()
    {
        //DataContext = new TabItemViewModel("Three", "Some content on third tab");
        // add an extra element to the array
        //Array.Resize(ref _tabItemViewModels, _tabItemViewModels.Length + 1);
        //_tabItemViewModels[^1] = new TabItemViewModel("New", "Some content on new tab");
        //_tabItemViewModels[0].Content = "new content";
        //_tabItemViewModels[0].Header = "New";
        var newTab = new TabItemViewModel("New", "Some content on new tab");
        _tabItemViewModels.Add(newTab);
        var tabControl = this.FindControl<TabControl>("TabControl");
        tabControl.SelectedIndex = _tabItemViewModels.Count - 1; // Set the newly added tab as the selected tab
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}
