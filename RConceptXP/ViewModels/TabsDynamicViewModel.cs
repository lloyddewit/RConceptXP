using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using System;
using System.Collections.ObjectModel;

namespace RConceptXP.ViewModels;

public partial class TabsDynamicViewModel : ObservableObject
{
    public RelayCommand OnDeleteTabCommand { get; }

    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private BoxplotView? _content;

    public TabsDynamicViewModel(string header, BoxplotView? content)
    {
        Header = header;
        Content = content;
        OnDeleteTabCommand = new RelayCommand(DeleteTab);

    }

    private void DeleteTab()
    {
    }
}
