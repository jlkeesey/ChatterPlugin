using System;
using Dalamud.Interface.Windowing;

namespace ChatterPlugin.Windows;

public sealed class JlkWindowManager : IDisposable
{
    private readonly ConfigWindow configWindow;
    private readonly MainWindow mainWindow;
    private readonly Chatter chatter;

    public JlkWindowManager(Chatter chatter)
    {
        this.chatter = chatter;

        configWindow = Add(new ConfigWindow());
        mainWindow = Add(new MainWindow(chatter));

        Dalamud.PluginInterface.UiBuilder.Draw += chatter.WindowSystem.Draw;
        // this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    public void Dispose()
    {
        Dalamud.PluginInterface.UiBuilder.Draw -= chatter.WindowSystem.Draw;

        chatter.WindowSystem.RemoveAllWindows();

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
        chatter.WindowSystem.AddWindow(window);
        return window;
    }
}
