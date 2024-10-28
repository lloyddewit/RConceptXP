using CommunityToolkit.Mvvm.Input;
using RConceptXP.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RConceptXP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IRelayCommand OpenBoxplotCommand { get; }

    [ObservableProperty]
    private string toDoButton = "BoxPlot"; // Set the initial text for the button


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
