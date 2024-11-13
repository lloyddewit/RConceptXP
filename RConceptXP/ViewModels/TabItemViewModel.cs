using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RConceptXP.ViewModels;

public partial class TabItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private string _content;
    
    public TabItemViewModel(string header, string content)
    {
        Header = header;
        Content = content;
    }
}
