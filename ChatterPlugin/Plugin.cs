using ChatterPlugin.Attributes;
using ChatterPlugin.Windows;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using System.Reflection;
using System;

namespace ChatterPlugin;

public sealed class Plugin : IDalamudPlugin
{
    public static string Version = string.Empty;

    private readonly JlkCommandManager<Plugin> commandManager;
    private readonly JlkWindowManager windowManager;

    public Plugin(DalamudPluginInterface pluginInterface)
    {
        try
        {
            Dalamud.Initialize(pluginInterface);
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";

            Configuration = Configuration.Load();
            Settings = new Settings(Configuration);

            WindowSystem = new WindowSystem(Name);

            var log = Settings.Log;

            // you might normally want to embed resources and load them from the manifest stream
            // var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            // var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            windowManager = new JlkWindowManager(this);
            commandManager = new JlkCommandManager<Plugin>(this);
        }
        catch
        {
            ((IDisposable)this).Dispose();
            throw;
        }
    }

    public Configuration Configuration { get; private set; }
    public Settings Settings { get; private set; }
    public WindowSystem WindowSystem { get; private set; }
    public string Name => "ChatterPlugin";

    public void Dispose()
    {
        Configuration.Save();                   // Should be auto-saved but let's be sure
        commandManager.Dispose();
        windowManager.Dispose();
    }

    [Command("/chatter")]
    [HelpMessage("Show Chatter window.")]
    public void ChatterCommand(string command, string args)
    {
        windowManager.ToggleMain();
    }

    [Command("/chatterconfig")]
    [Aliases("/chattercfg")]
    [HelpMessage("Show Chatter configuration window.")]
    public void ChatterConfigCommand(string command, string args)
    {
        windowManager.ToggleConfig();
    }
}
