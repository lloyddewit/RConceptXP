using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;

namespace RConceptXP.Views;

public partial class BoxplotView : UserControl
{
    public BoxplotViewModel? BoxplotViewModel => DataContext as BoxplotViewModel;

    // The axaml file needs a parameterless constructor
    public BoxplotView() : this(null)
    {
    }

    public BoxplotView(BoxplotViewModel? boxplotToDuplicate)
    {
        InitializeComponent();
        DataContext = new BoxplotViewModel(this, boxplotToDuplicate);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}