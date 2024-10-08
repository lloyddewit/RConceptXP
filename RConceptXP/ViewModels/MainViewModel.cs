using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }

    public MainViewModel()
    {
        OpenBoxplotCommand = new RelayCommand(OpenBoxplot);
    }

    private async void OpenBoxplot()
    {
        // Check if Application or Application.Current is null
        if (Application.Current == null || Application.Current.ApplicationLifetime == null)
        {
            return;
        }

        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            var mainWindow = desktopLifetime.MainWindow;
            if (mainWindow != null)
            {
                var dialog = new Boxplot();
                await dialog.ShowDialog(mainWindow);
            }
        }
    }

}
