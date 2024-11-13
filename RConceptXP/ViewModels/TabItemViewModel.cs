using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RConceptXP.ViewModels;

public class TabItemViewModel
{
    public string Header { get; set; }
    public string Content { get; set; }
    public TabItemViewModel(string header, string content)
    {
        Header = header;
        Content = content;
    }
}
