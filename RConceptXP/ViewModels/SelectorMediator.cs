using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using RConceptXP.Views;

namespace RConceptXP.ViewModels;

public class SelectorMediator
{
    public TextBox CurrentReceiver => _receivers[_textBoxWithFocusIndex];

    private List<TextBox> _receivers { get; set; } = new List<TextBox>();
    private int _textBoxWithFocusIndex = 0;
    private ImmutableSolidColorBrush? _defaultBackground;

    public SelectorMediator(List<TextBox> receivers)
    {
        if (receivers.Count == 0)
            throw new ArgumentException("receivers list is empty");

        _receivers.AddRange(receivers);
        _textBoxWithFocusIndex = 0;
        _receivers[_textBoxWithFocusIndex].Focus();
        if (_receivers[_textBoxWithFocusIndex].Background is null)
            _defaultBackground = (ImmutableSolidColorBrush?)Brushes.White;
        else
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _defaultBackground = (ImmutableSolidColorBrush)_receivers[_textBoxWithFocusIndex].Background;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // set input focus to first receiver
        // Note: this statement uses a lambda expression to define an event handler for when the
        // first receiver has been added to the visual tree (calling the Focus method before this
        // event has no affect). Ask GitHub Copilot for more explanation.
        _receivers[_textBoxWithFocusIndex].AttachedToVisualTree += (s, e) => _receivers[_textBoxWithFocusIndex].Focus();
    }

    public void AddSelectedValueToReceiver(IReadOnlyList<string?> items)
    {
        //convert items to newline separated string
        string text = string.Join(Environment.NewLine, items);
        _receivers[_textBoxWithFocusIndex].Text = text;

        int newTextBoxIndex = (_textBoxWithFocusIndex + 1) % _receivers.Count; 
        while (!_receivers[newTextBoxIndex].IsVisible || !_receivers[newTextBoxIndex].IsEnabled) //todo handle case where no receivers are visible
        {
            newTextBoxIndex = (newTextBoxIndex + 1) % _receivers.Count;
        }

        _receivers[newTextBoxIndex].Focus();
    }

    public void SetFocus(TextBox receiver)
    {
        _receivers[_textBoxWithFocusIndex].Background = _defaultBackground;
        receiver.Background = Brushes.LightYellow;
        _textBoxWithFocusIndex = _receivers.IndexOf(receiver);
    }
}
