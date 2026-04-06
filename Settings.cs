using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using Dota1Warkey.Items;
using Dota1Warkey.ViewModels;

namespace Dota1Warkey;

// Plain serializable DTO — avoids serializing INotifyPropertyChanged internals
file class SettingsData
{
    public HotkeyData?[] Slots { get; set; } = new HotkeyData?[6];
    public bool IsStartMinimized { get; set; }
    public bool IsAutoStartWar3 { get; set; }
    public bool IsAutoCloseWithWar3 { get; set; }
}

file class HotkeyData
{
    public bool Alt { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Key Key { get; set; }
}

public static class Settings
{
    private const string FileName = "Dota1Warkey.json";
    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

    public static bool IsStartMinimized { get; set; }
    public static bool IsAutoStartWar3 { get; set; }
    public static bool IsAutoCloseWithWar3 { get; set; }

    public static async Task LoadAsync(WarkeyViewModel warkeyVm)
    {
        if (!File.Exists(FileName)) return;

        try
        {
            using var fs = File.OpenRead(FileName);
            var data = await JsonSerializer.DeserializeAsync<SettingsData>(fs, JsonOpts);
            if (data is null) return;

            IsStartMinimized    = data.IsStartMinimized;
            IsAutoStartWar3     = data.IsAutoStartWar3;
            IsAutoCloseWithWar3 = data.IsAutoCloseWithWar3;

            var props = new[] { "Slot1","Slot2","Slot3","Slot4","Slot5","Slot6" };
            for (int i = 0; i < 6; i++)
            {
                if (data.Slots[i] is { } d)
                {
                    var prop = typeof(WarkeyViewModel).GetProperty(props[i])!;
                    prop.SetValue(warkeyVm, new HotkeyItem { Alt = d.Alt, Key = d.Key });
                }
            }
        }
        catch { /* corrupted file — use defaults */ }
    }

    public static async Task SaveAsync(WarkeyViewModel warkeyVm)
    {
        var props = new[] { "Slot1","Slot2","Slot3","Slot4","Slot5","Slot6" };
        var slots = new HotkeyData?[6];
        for (int i = 0; i < 6; i++)
        {
            var item = (HotkeyItem?)typeof(WarkeyViewModel).GetProperty(props[i])!.GetValue(warkeyVm);
            if (item is not null)
                slots[i] = new HotkeyData { Alt = item.Alt, Key = item.Key };
        }

        var data = new SettingsData
        {
            Slots               = slots,
            IsStartMinimized    = IsStartMinimized,
            IsAutoStartWar3     = IsAutoStartWar3,
            IsAutoCloseWithWar3 = IsAutoCloseWithWar3,
        };

        using var fs = File.Create(FileName);
        await JsonSerializer.SerializeAsync(fs, data, JsonOpts);
    }
}
