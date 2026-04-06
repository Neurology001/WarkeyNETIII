using System.Windows.Controls;
using System.Windows.Media;
using Dota1Warkey.Pages;
using Dota1Warkey.Services;
using Dota1Warkey.ViewModels;

namespace Dota1Warkey;

public partial class MainWindow : Window
{
    private readonly MainViewModel _mainVm = new();
    private readonly WarkeyViewModel _warkeyVm = new();
    private readonly SettingsViewModel _settingsVm = new();

    private static readonly SolidColorBrush ActiveNavBg =
        new(Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF));

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _mainVm;
        _mainVm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainViewModel.Status))
                UpdateStatusIndicator(_mainVm.Status);
        };
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await Settings.LoadAsync(_warkeyVm);

        if (Settings.IsStartMinimized)
            WindowState = WindowState.Minimized;

        _settingsVm.LoadFromSettings();

        MainService.WarkeyVm = _warkeyVm;
        MainService.StatusVm = _mainVm;
        MainService.Initialize();

        NavFrame.Navigate(new WarkeyPage(_warkeyVm));
        SetActiveNav(BtnWarkey);
    }

    private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await Settings.SaveAsync(_warkeyVm);
        MainService.Dispose();
    }

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => Close();

    private void Nav_Click(object sender, RoutedEventArgs e)
    {
        var btn = (Button)sender;
        SetActiveNav(btn);

        switch (btn.Tag?.ToString())
        {
            case "Warkey":
                NavFrame.Navigate(new WarkeyPage(_warkeyVm));
                break;
            case "Settings":
                NavFrame.Navigate(new SettingsPage(_settingsVm));
                break;
        }
    }

    private void SetActiveNav(Button active)
    {
        foreach (var child in ((StackPanel)active.Parent).Children)
        {
            if (child is Button b)
                b.Background = Brushes.Transparent;
        }
        active.Background = ActiveNavBg;

        // Active nav item uses white foreground
        foreach (var child in ((StackPanel)active.Parent).Children)
        {
            if (child is Button b)
            {
                var tb = b.Content as TextBlock;
                if (tb is not null)
                    tb.Foreground = b == active
                        ? (SolidColorBrush)FindResource("TextPrimary")
                        : (SolidColorBrush)FindResource("TextSecondary");
            }
        }
    }

    private void UpdateStatusIndicator(string status)
    {
        if (status == "Running")
        {
            StatusDot.Fill  = new SolidColorBrush(Color.FromRgb(0x30, 0xD1, 0x58)); // green
            StatusText.Text = "War3 运行中";
        }
        else
        {
            StatusDot.Fill  = new SolidColorBrush(Color.FromRgb(0xFF, 0x3B, 0x30)); // red
            StatusText.Text = "未检测到 War3";
        }
    }
}
