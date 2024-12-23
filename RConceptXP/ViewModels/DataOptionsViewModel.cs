using CommunityToolkit.Mvvm.ComponentModel;

namespace RConceptXP.ViewModels;

//todo support empty list of columns (test that selector buttons enable/disable correctly and no crashes)
public partial class DataOptionsViewModel : ObservableObject
{
    [ObservableProperty]
    private int _selectedTabIndex;


    public DataOptionsViewModel()
    {
    }
}
