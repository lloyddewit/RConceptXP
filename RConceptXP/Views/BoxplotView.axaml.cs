using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;

namespace RConceptXP.Views;

public partial class BoxplotView : UserControl
{
    public BoxplotView()
    {
        InitializeComponent();
        DataContext = new BoxplotViewModel(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}