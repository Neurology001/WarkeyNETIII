using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dota1Warkey.Services;

public static class PostMessageService
{
    [DllImport("user32.dll", EntryPoint = "SendMessageA")]
    private static extern bool SendMsg(IntPtr hWnd, uint msg, int wParam, int lParam);

    private const uint WM_KEYDOWN   = 0x0100;
    private const uint WM_KEYUP     = 0x0101;
    private const uint WM_SYSKEYDOWN = 0x0104;
    private const int  VK_MENU      = 0x0012;

    // Dota1 item slots map to numpad: 7 8 / 4 5 / 1 2
    public static readonly Keys[] SlotVKeys =
    [
        Keys.NumPad7,
        Keys.NumPad8,
        Keys.NumPad4,
        Keys.NumPad5,
        Keys.NumPad1,
        Keys.NumPad2,
    ];

    public static void PostItemKey(IntPtr hwnd, int slotIndex, bool isAlt)
    {
        int vk = (int)SlotVKeys[slotIndex];

        if (isAlt)
            SendMsg(hwnd, WM_KEYUP, VK_MENU, 0);

        SendMsg(hwnd, WM_KEYDOWN, vk, 0);
        SendMsg(hwnd, WM_KEYUP,   vk, 0);

        if (isAlt)
            SendMsg(hwnd, WM_SYSKEYDOWN, VK_MENU, 0);
    }
}
