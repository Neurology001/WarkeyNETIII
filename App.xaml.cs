namespace Dota1Warkey;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, ex) =>
            LogCrash(ex.ExceptionObject as Exception);

        DispatcherUnhandledException += (_, ex) =>
        {
            LogCrash(ex.Exception);
            ex.Handled = true;
        };

        base.OnStartup(e);
    }

    private static void LogCrash(Exception? ex)
    {
        try
        {
            File.WriteAllText("crash.log",
                $"[{DateTime.Now}]\n{ex}\n");
            MessageBox.Show(ex?.Message, "启动错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch { }
    }
}
