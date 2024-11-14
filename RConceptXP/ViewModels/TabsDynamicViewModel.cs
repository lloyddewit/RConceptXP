using CommunityToolkit.Mvvm.ComponentModel;
using RConceptXP.Views;

namespace RConceptXP.ViewModels;

public partial class TabsDynamicViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private BoxplotView? _content;

    public TabsDynamicViewModel(string header, BoxplotView? content)
    {
        Header = header;
        Content = content;
    }
}

