using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace RConceptXP.Views;

public partial class TabsDynamicView : UserControl
{
    private ObservableCollection<TabsDynamicViewModel> _tabViewModels;

    public TabsDynamicView()
    {
        AvaloniaXamlLoader.Load(this);

        _tabViewModels = new ObservableCollection<TabsDynamicViewModel>
        {
            new TabsDynamicViewModel("<empty>", null)
        };
        DataContext = _tabViewModels;
    }

    public void AddNewTab()
    {
        var newTab = new TabsDynamicViewModel("New", new BoxplotView());
        _tabViewModels.Add(newTab);

        var tabControl = this.FindControl<TabControl>("tabs") ??
            throw new Exception("Cannot find tabs by name");

        tabControl.SelectedIndex = _tabViewModels.Count - 1;
    }
}
