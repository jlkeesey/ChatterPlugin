using System;
using Dalamud.Interface.Windowing;

namespace ChatterPlugin.Windows;

public sealed class PluginWindowManager : IDisposable
{
    private readonly ConfigWindow configWindow;
    private readonly MainWindow mainWindow;
    private readonly Plugin plugin;

    public PluginWindowManager(Plugin plugin)
    {
        this.plugin = plugin;

        configWindow = Add(new ConfigWindow(plugin));
        mainWindow = Add(new MainWindow(plugin));

        Dalamud.PluginInterface.UiBuilder.Draw += plugin.WindowSystem.Draw;
        // this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    public void Dispose()
    {
        Dalamud.PluginInterface.UiBuilder.Draw -= plugin.WindowSystem.Draw;

        plugin.WindowSystem.RemoveAllWindows();

        mainWindow.Dispose();
        configWindow.Dispose();
    }

    public void ToggleMain()
    {
        mainWindow.IsOpen = !mainWindow.IsOpen;
    }

    public void ToggleConfig()
    {
        configWindow.IsOpen = !configWindow.IsOpen;
    }

    private TType Add<TType>(TType window) where TType : Window
    {
        plugin.WindowSystem.AddWindow(window);
        return window;
    }
}
