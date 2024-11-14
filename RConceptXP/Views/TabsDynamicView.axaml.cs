using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace RConceptXP.Views;

public partial class TabsDynamicView : UserControl
{
    private ObservableCollection<TabsDynamicViewModel> _tabViewModels;

    public TabsDynamicView()
    {
        AvaloniaXamlLoader.Load(this);

        _tabViewModels = new ObservableCollection<TabsDynamicViewModel> {};
        DataContext = _tabViewModels;
    }

    public void AddNewTab()
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
            header = $"Boxplot{count}";
            count++;
        } while (headers.Contains(header));

        _tabViewModels.Add(new TabsDynamicViewModel(header, new BoxplotView()));
        tabControl.SelectedIndex = _tabViewModels.Count - 1;
    }
}
