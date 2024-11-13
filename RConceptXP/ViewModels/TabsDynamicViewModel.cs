using CommunityToolkit.Mvvm.ComponentModel;

namespace RConceptXP.ViewModels;

public partial class TabsDynamicViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private string _content;

    public TabsDynamicViewModel(string header, string content)
    {
        Header = header;
        Content = content;
    }
}

