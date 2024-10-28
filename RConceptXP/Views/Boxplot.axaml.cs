using Avalonia.Controls;
using Avalonia.Media;
using RConceptXP.ViewModels;
using System;
using System.Collections.Generic;

namespace RConceptXP.Views;

public partial class Boxplot : Window
{
    public Boxplot()
    {
        InitializeComponent();
        DataContext = new BoxplotViewModel(this);
    }
}