using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RConceptXP.ViewModels;
using System;

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
        var boxplotMainTabView = this.FindControl<BoxplotMainTabView>("mainTab") ??
            throw new Exception("Cannot find boxplotMainTabView by name");
        DataContext = new BoxplotViewModel(boxplotMainTabView, boxplotToDuplicate);
    }    
}