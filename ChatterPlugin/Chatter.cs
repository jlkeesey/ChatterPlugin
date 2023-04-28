using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ChatterPlugin.Friends;
using ChatterPlugin.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using ImGuiNET;

namespace ChatterPlugin;

public sealed partial class Chatter : IDalamudPlugin
{
    public static string Version = string.Empty;

    private readonly JlkWindowManager _windowManager;

    public Chatter(DalamudPluginInterface pluginInterface)
    {
        try
        {
            Dalamud.Initialize(pluginInterface);
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";

            Configuration = Configuration.Load();

            WindowSystem = new WindowSystem(Name);

            ChatLogManager = new ChatLogManager();
            ChatManager = new ChatManager(ChatLogManager);

            // you might normally want to embed resources and load them from the manifest stream
            // var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            // var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            _windowManager = new JlkWindowManager(this);
            RegisterCommands();
        }
        catch
        {
            Dispose();
            throw;
        }
    }

#pragma warning disable CS8618
    /// <summary>
    ///     The configuration of this plugin.
    /// </summary>
    public static Configuration Configuration { get; private set; }
#pragma warning restore CS8618

    public ChatManager ChatManager { get; }
    public ChatLogManager ChatLogManager { get; }
    public WindowSystem WindowSystem { get; private set; }
    public string Name => "ChatterPlugin";

    public void Dispose()
    {
        Configuration?.Save(); // Should be auto-saved but let's be sure

        UnregisterCommands();
        ChatLogManager?.Dispose();
        ChatManager?.Dispose();
        _windowManager?.Dispose();
    }
}