using System.Windows.Input;
using Dota1Warkey.ViewModels;

namespace Dota1Warkey.Items;

public class HotkeyItem : BaseViewModel
{
    private bool _alt;
    private Key _key;

    public bool Alt
    {
        get => _alt;
        set { _alt = value; OnPropertyChanged(); }
    }

    public Key Key
    {
        get => _key;
        set { _key = value; OnPropertyChanged(); }
    }

    public HotkeyItem()
    {
        Alt = false;
        Key = Key.None;
    }
}
