using Avalonia.Controls;
using RConceptXP.ViewModels;

namespace RConceptXP.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel(this);
    }
}
