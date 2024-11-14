using CommunityToolkit.Mvvm.ComponentModel;
using RConceptXP.Views;

namespace RConceptXP.ViewModels;

public partial class TabsDynamicViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private Boxplot2View? _content;

    public TabsDynamicViewModel(string header, Boxplot2View? content)
    {
        Header = header;
        Content = content;
    }
}

