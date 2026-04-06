using System.Runtime.InteropServices;

namespace Dota1Warkey.Services;

public static class ForegroundWindowService
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public static bool IsWar3Foreground(IntPtr hwnd) => GetForegroundWindow() == hwnd;
}
