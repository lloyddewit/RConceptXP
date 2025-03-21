using Avalonia.Controls;
using RConceptXP.ViewModels;
using System;

namespace RConceptXP.Views;

public partial class BoxplotView : UserControl
{
    public BoxplotViewModel? BoxplotViewModel => DataContext as BoxplotViewModel;

    // The axaml file needs a parameterless constructor
    public BoxplotView() : this(new MainView(), new MainViewModel(new MainView()), null)
    {
    }

    public BoxplotView(MainView mainView, MainViewModel mainViewModel, BoxplotViewModel? boxplotToDuplicate)
    {
        InitializeComponent();
        var boxplotMainTabView = this.FindControl<BoxplotMainTabView>("mainTab") ??
            throw new Exception("Cannot find boxplotMainTabView by name");
        DataContext = new BoxplotViewModel(mainView, mainViewModel, boxplotMainTabView, boxplotToDuplicate);
    }    
}