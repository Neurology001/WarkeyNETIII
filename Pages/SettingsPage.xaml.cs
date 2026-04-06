using System.Windows.Controls;
using Dota1Warkey.ViewModels;

namespace Dota1Warkey.Pages;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
