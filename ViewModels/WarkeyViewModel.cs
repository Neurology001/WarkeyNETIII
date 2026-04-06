using Dota1Warkey.Items;

namespace Dota1Warkey.ViewModels;

public class WarkeyViewModel : BaseViewModel
{
    private HotkeyItem? _slot1, _slot2, _slot3, _slot4, _slot5, _slot6;

    public HotkeyItem? Slot1 { get => _slot1; set { _slot1 = value; OnPropertyChanged(); } }
    public HotkeyItem? Slot2 { get => _slot2; set { _slot2 = value; OnPropertyChanged(); } }
    public HotkeyItem? Slot3 { get => _slot3; set { _slot3 = value; OnPropertyChanged(); } }
    public HotkeyItem? Slot4 { get => _slot4; set { _slot4 = value; OnPropertyChanged(); } }
    public HotkeyItem? Slot5 { get => _slot5; set { _slot5 = value; OnPropertyChanged(); } }
    public HotkeyItem? Slot6 { get => _slot6; set { _slot6 = value; OnPropertyChanged(); } }
}
