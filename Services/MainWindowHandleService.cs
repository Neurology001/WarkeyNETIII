using System.Diagnostics;

namespace Dota1Warkey.Services;

public static class MainWindowHandleService
{
    private static readonly string[] ProcessNames = ["war3", "War3"];

    public static IntPtr? GetWar3Hwnd()
    {
        foreach (var name in ProcessNames)
        {
            var procs = Process.GetProcessesByName(name);
            if (procs.Length > 0)
                return procs[0].MainWindowHandle;
        }
        return null;
    }
}
