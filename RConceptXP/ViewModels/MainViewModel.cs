using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }

    [ObservableProperty]
    private object _firstTabContent;

    [ObservableProperty]
    private TabControl _tabs;
    
    public MainViewModel()
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);

        FirstTabContent = new TextBlock { Text = "Empty tab" };

        Tabs = new TabControl();
    }

    private void OpenBoxplot()
    {
        var newTab = new TabItem
        {
            Header = "Boxplot",
            Content = new Boxplot2View()
        };
        Tabs.Items.Add(newTab);
    }

}
