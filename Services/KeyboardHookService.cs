using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Dota1Warkey.Items;

namespace Dota1Warkey.Services;

public static class KeyboardHookService
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_KEYBOARD_LL  = 13;
    private const int WM_KEYDOWN      = 0x0100;
    private const int WM_SYSKEYDOWN   = 0x0104;
    private const int VK_LMENU        = 0xA4;
    private const int VK_RETURN       = 0x0D;  // Enter
    private const int VK_ESCAPE       = 0x1B;  // Esc

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static readonly LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    // 聊天框是否打开：Enter 开启，Enter/Esc 关闭
    private static bool _isChatting = false;

    public static void Initialize()
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule!;
        _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName!), 0);
    }

    public static void Dispose()
    {
        if (_hookID != IntPtr.Zero)
            UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (MainService.IsWar3Foreground && nCode >= 0)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                // 追踪聊天框开关状态
                if (vkCode == VK_RETURN)
                {
                    _isChatting = !_isChatting;
                    return CallNextHookEx(_hookID, nCode, wParam, lParam);
                }
                if (vkCode == VK_ESCAPE)
                {
                    _isChatting = false;
                    return CallNextHookEx(_hookID, nCode, wParam, lParam);
                }
            }

            // 聊天中不拦截任何按键
            if (!_isChatting)
            {
                bool handled = false;

                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    handled = MainService.TryHandleKey(new HotkeyItem
                    {
                        Alt = false,
                        Key = KeyInterop.KeyFromVirtualKey(vkCode)
                    });
                }
                else if (wParam == (IntPtr)WM_SYSKEYDOWN && vkCode != VK_LMENU)
                {
                    handled = MainService.TryHandleKey(new HotkeyItem
                    {
                        Alt = true,
                        Key = KeyInterop.KeyFromVirtualKey(vkCode)
                    });
                }

                if (handled)
                    return (IntPtr)1;
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }
}
