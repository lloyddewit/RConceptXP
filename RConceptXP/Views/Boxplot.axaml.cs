using Avalonia.Controls;
using RConceptXP.ViewModels;

namespace RConceptXP.Views;

public partial class Boxplot : Window
{
    public Boxplot()
    {
        InitializeComponent();
        DataContext = new BoxplotViewModel();
    }

    private void OnToScriptClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}