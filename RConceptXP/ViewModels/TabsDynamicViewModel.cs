using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using System;

namespace RConceptXP.ViewModels;

public partial class TabsDynamicViewModel : ObservableObject
{
    public event EventHandler? TabCreated;
    public event EventHandler? TabDeleted;
    public event EventHandler? TabDuplicated;

    public RelayCommand OnCreateTabCommand { get; }
    public RelayCommand OnDeleteTabCommand { get; }
    public RelayCommand OnDuplicateTabCommand { get; }

    public ObservableObject? TabViewModel { get; }

    [ObservableProperty]
    private UserControl? _content;

    [ObservableProperty]
    private string _header;

    public TabsDynamicViewModel(string header, UserControl? content)
    {
        Header = header;
        Content = content;
        TabViewModel = (ObservableObject?)(content?.DataContext);
        OnCreateTabCommand = new RelayCommand(CreateTab);
        OnDeleteTabCommand = new RelayCommand(DeleteTab);
        OnDuplicateTabCommand = new RelayCommand(DuplicateTab);
    }

    private void CreateTab()
    {
        TabCreated?.Invoke(this, EventArgs.Empty);
    }

    private void DeleteTab()
    {
        TabDeleted?.Invoke(this, EventArgs.Empty);
    }

    private void DuplicateTab()
    {
        TabDuplicated?.Invoke(this, EventArgs.Empty);
    }
}
