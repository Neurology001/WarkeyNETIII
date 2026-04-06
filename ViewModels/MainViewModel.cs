namespace Dota1Warkey.ViewModels;

public class MainViewModel : BaseViewModel
{
    private string _status = "";

    // "Running" when War3 process is detected, empty otherwise
    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }
}
