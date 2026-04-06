using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Dota1Warkey.Items;
using Dota1Warkey.ViewModels;

namespace Dota1Warkey.Pages;

public partial class WarkeyPage : Page
{
    private readonly WarkeyViewModel _vm;

    private static readonly SolidColorBrush CardDefault =
        new(Color.FromRgb(0x3A, 0x3A, 0x3C));
    private static readonly SolidColorBrush CardFocused =
        new(Color.FromArgb(0xFF, 0x1C, 0x2E, 0x45));
    private static readonly Thickness CardBorderDefault =
        new(0);
    private static readonly Thickness CardBorderFocused =
        new(1.5);
    private static readonly SolidColorBrush CardBorderFocusedBrush =
        new(Color.FromRgb(0x0A, 0x84, 0xFF));

    public WarkeyPage(WarkeyViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;
    }

    private void Slot_KeyDown(object sender, KeyEventArgs e)
    {
        var textBox = (TextBox)sender;
        var tag = textBox.Tag?.ToString() ?? "";
        var prop = typeof(WarkeyViewModel).GetProperty(tag);

        if (e.Key == Key.Escape)
        {
            prop?.SetValue(_vm, null);
            e.Handled = true;
            return;
        }

        bool alt = Keyboard.IsKeyDown(Key.LeftAlt);
        var key  = alt ? e.SystemKey : e.Key;

        prop?.SetValue(_vm, new HotkeyItem { Alt = alt, Key = key });
        e.Handled = true;
    }

    private void Slot_GotFocus(object sender, RoutedEventArgs e)
    {
        var card = GetCard((TextBox)sender);
        if (card is null) return;
        card.Background      = CardFocused;
        card.BorderBrush     = CardBorderFocusedBrush;
        card.BorderThickness = CardBorderFocused;
    }

    private void Slot_LostFocus(object sender, RoutedEventArgs e)
    {
        var card = GetCard((TextBox)sender);
        if (card is null) return;
        card.Background      = CardDefault;
        card.BorderBrush     = null;
        card.BorderThickness = CardBorderDefault;
    }

    private Border? GetCard(TextBox tb)
    {
        var tag = tb.Tag?.ToString();
        return tag switch
        {
            "Slot1" => Card1,
            "Slot2" => Card2,
            "Slot3" => Card3,
            "Slot4" => Card4,
            "Slot5" => Card5,
            "Slot6" => Card6,
            _       => null,
        };
    }
}
