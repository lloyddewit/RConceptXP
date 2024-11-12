using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;

namespace RConceptXP.Views;

public partial class TabControlDynamicView : UserControl
{
    private TabItemViewModel[] _tabItemViewModels;
    public TabControlDynamicView()
    {
        InitializeComponent();
        //DataContext = new TabControlDynamicViewModel();

        _tabItemViewModels = [
            new TabItemViewModel("One", "Some content on first tab"),
            new TabItemViewModel("Two", "Some content on second tab"),
        ];
        DataContext = _tabItemViewModels;
    }

    public void AddNewTab()
    {
        //DataContext = new TabItemViewModel("Three", "Some content on third tab");
        // add an extra element to the array
        Array.Resize(ref _tabItemViewModels, _tabItemViewModels.Length + 1);
        _tabItemViewModels[^1] = new TabItemViewModel("New", "Some content on new tab");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}
