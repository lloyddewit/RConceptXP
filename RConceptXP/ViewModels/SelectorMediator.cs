using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;

namespace RConceptXP.ViewModels;

public class SelectorMediator
{
    public TextBox CurrentReceiver => _receivers[_textBoxWithFocus];

    private List<TextBox> _receivers { get; set; } = new List<TextBox>();
    private int _textBoxWithFocus = 0;
    private Brush? _defaultBackground;

    public SelectorMediator(List<TextBox> receivers)
    {
        if (receivers.Count == 0)
            throw new ArgumentException("receivers list is empty");

        _receivers.AddRange(receivers);
        _textBoxWithFocus = 0;
        _receivers[_textBoxWithFocus].Focus();
        if (_receivers[_textBoxWithFocus].Background is null)
            _defaultBackground = (Brush?)Brushes.White;
        else
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _defaultBackground = (Brush)_receivers[_textBoxWithFocus].Background;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    public void AddSelectedValueToReceiver(string text)
    {
        _receivers[_textBoxWithFocus].Text = text;
        _receivers[_textBoxWithFocus].Background = _defaultBackground;
        do
        {
            _textBoxWithFocus = (_textBoxWithFocus + 1) % _receivers.Count;
        } while (!_receivers[_textBoxWithFocus].IsVisible);
        
        _receivers[_textBoxWithFocus].Focus();
    }
}
