using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }

    [ObservableProperty]
    private object _firstTabContent;

    public MainViewModel()
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);

        FirstTabContent = new TextBlock { Text = "Empty tab" };
    }

    private void OpenBoxplot()
    {
        // todo: this line was suggested by copilot to try and solve issue that Android version closes when this function called.
        // On Android it stops the app from closing but the view is still not displayed.
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            FirstTabContent = new Boxplot2View();
        });
    }

}
