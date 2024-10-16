using Avalonia.Controls;
using Avalonia.Media;
using RConceptXP.ViewModels;
using System;

namespace RConceptXP.Views;

public partial class Boxplot : Window
{
    public bool IsCheckedToDo = true;
    public Boxplot()
    {
        InitializeComponent();
        DataContext = new BoxplotViewModel();

        AutoCompleteBox saveGraphAutoComplete = this.FindControl<AutoCompleteBox>("SaveGraphAutoCompleteBox") ?? throw new Exception("Cannot find save graph auto-complete box by name");
        //todo saveGraphComboBox.SelectedIndex = 0;
    }

    private void OnToScriptClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}