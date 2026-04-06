using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Dota1Warkey.Items;
using Dota1Warkey.ViewModels;

namespace Dota1Warkey.Services;

public static class MainService
{
    private const int HwndPollSeconds = 1;

    private static IntPtr _war3Hwnd;
    private static bool _war3WasRunning;

    public static bool IsWar3Foreground { get; private set; }

    public static WarkeyViewModel? WarkeyVm { get; set; }
    public static MainViewModel? StatusVm { get; set; }

    public static void Initialize()
    {
        PollWar3Hwnd();

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(HwndPollSeconds) };
        timer.Tick += (_, _) => PollWar3Hwnd();
        timer.Start();

        KeyboardHookService.Initialize();
    }

    public static void Dispose()
    {
        KeyboardHookService.Dispose();
    }

    /// <summary>
    /// 由键盘钩子直接调用。返回 true 表示已处理，钩子应抑制原始按键不透传给 War3。
    /// </summary>
    public static bool TryHandleKey(HotkeyItem e)
    {
        if (WarkeyVm is null) return false;

        HotkeyItem?[] slots =
        [
            WarkeyVm.Slot1, WarkeyVm.Slot2, WarkeyVm.Slot3,
            WarkeyVm.Slot4, WarkeyVm.Slot5, WarkeyVm.Slot6,
        ];

        bool matched = false;
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot is null) continue;
            if (slot.Key == e.Key && slot.Alt == e.Alt)
            {
                PostMessageService.PostItemKey(_war3Hwnd, i, e.Alt);
                matched = true;
            }
        }
        return matched;
    }

    private static void PollWar3Hwnd()
    {
        var hwnd = MainWindowHandleService.GetWar3Hwnd();

        if (hwnd is not null)
        {
            _war3Hwnd = hwnd.Value;
            _war3WasRunning = true;
            IsWar3Foreground = ForegroundWindowService.IsWar3Foreground(_war3Hwnd);
            if (StatusVm is not null) StatusVm.Status = "Running";
        }
        else
        {
            IsWar3Foreground = false;
            if (StatusVm is not null) StatusVm.Status = "";

            // 随 War3 关闭
            if (_war3WasRunning && Settings.IsAutoCloseWithWar3)
            {
                Application.Current.MainWindow?.Close();
                return;
            }

            // 随 War3 启动（只触发一次：War3 尚未运行过且 exe 存在）
            if (!_war3WasRunning && Settings.IsAutoStartWar3 && File.Exists("war3.exe"))
            {
                Process.Start("war3.exe");
            }
        }
    }
}
