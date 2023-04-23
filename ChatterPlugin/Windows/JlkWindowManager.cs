using System;
using Dalamud.Interface.Windowing;

namespace ChatterPlugin.Windows;

public sealed class JlkWindowManager : IDisposable
{
    private readonly Chatter _chatter;
    private readonly ConfigWindow _configWindow;
    // private readonly MainWindow _mainWindow;

    public JlkWindowManager(Chatter chatter)
    {
        this._chatter = chatter;

        _configWindow = Add(new ConfigWindow());
        // _mainWindow = Add(new MainWindow(chatter));

        Dalamud.PluginInterface.UiBuilder.Draw += chatter.WindowSystem.Draw;
        Dalamud.PluginInterface.UiBuilder.OpenConfigUi += ToggleConfig;
    }

    public void Dispose()
    {
        Dalamud.PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfig;
        Dalamud.PluginInterface.UiBuilder.Draw -= _chatter.WindowSystem.Draw;

        _chatter.WindowSystem.RemoveAllWindows();

        // _mainWindow.Dispose();
        _configWindow.Dispose();
    }

    // public void ToggleMain()
    // {
    //     _mainWindow.IsOpen = !_mainWindow.IsOpen;
    // }

    public void ToggleConfig()
    {
        _configWindow.IsOpen = !_configWindow.IsOpen;
    }

    private TType Add<TType>(TType window) where TType : Window
    {
        _chatter.WindowSystem.AddWindow(window);
        return window;
    }
}