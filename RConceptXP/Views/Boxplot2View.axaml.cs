using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;

namespace RConceptXP.Views;

public partial class Boxplot2View : UserControl
{
    public Boxplot2View()
    {
        InitializeComponent();
        DataContext = new Boxplot2ViewModel(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}