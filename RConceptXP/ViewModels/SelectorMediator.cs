using System.Collections.Generic;
using Avalonia.Controls;

namespace RConceptXP.ViewModels;

public class SelectorMediator
{
    public List<TextBlock> Receivers { get; set; } = new List<TextBlock>();

    public void Add(List<string> items)
    {
        foreach (var receiver in Receivers)
        {
            receiver.Text = string.Join(", ", items);
        }
    }

    public void GotFocus(TextBlock receiver)
    {
        receiver.FontWeight = Avalonia.Media.FontWeight.Bold;
    }

    public void LostFocus(TextBlock receiver)
    {
        receiver.FontWeight = Avalonia.Media.FontWeight.Normal;
    }
}
