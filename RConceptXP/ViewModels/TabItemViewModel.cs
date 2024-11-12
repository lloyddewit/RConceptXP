using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RConceptXP.ViewModels;

public class TabItemViewModel
{
    public string Header { get; }
    public string Content { get; }
    public TabItemViewModel(string header, string content)
    {
        Header = header;
        Content = content;
    }
}
