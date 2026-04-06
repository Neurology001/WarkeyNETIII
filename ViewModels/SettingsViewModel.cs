namespace Dota1Warkey.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private bool _isStartMinimized;
    private bool _isAutoStartWar3;
    private bool _isAutoCloseWithWar3;

    public bool IsStartMinimized
    {
        get => _isStartMinimized;
        set { _isStartMinimized = value; OnPropertyChanged(); Settings.IsStartMinimized = value; }
    }

    public bool IsAutoStartWar3
    {
        get => _isAutoStartWar3;
        set { _isAutoStartWar3 = value; OnPropertyChanged(); Settings.IsAutoStartWar3 = value; }
    }

    public bool IsAutoCloseWithWar3
    {
        get => _isAutoCloseWithWar3;
        set { _isAutoCloseWithWar3 = value; OnPropertyChanged(); Settings.IsAutoCloseWithWar3 = value; }
    }

    // Sync from static Settings after load
    public void LoadFromSettings()
    {
        _isStartMinimized    = Settings.IsStartMinimized;
        _isAutoStartWar3     = Settings.IsAutoStartWar3;
        _isAutoCloseWithWar3 = Settings.IsAutoCloseWithWar3;
        OnPropertyChanged(nameof(IsStartMinimized));
        OnPropertyChanged(nameof(IsAutoStartWar3));
        OnPropertyChanged(nameof(IsAutoCloseWithWar3));
    }
}
