using System.Reflection;
using ChatterPlugin.Localization;
using ChatterPlugin.Properties;
using ChatterPlugin.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ImGuiScene;

namespace ChatterPlugin;

// TODO NodaTime?
// TODO Fix tell in vs out

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
            Loc.Load();

            Configuration = Configuration.Load();

            WindowSystem = new WindowSystem(Name);

            ChatLogManager = new ChatLogManager();
            ChatManager = new ChatManager(Configuration, ChatLogManager);

            ChatterImage = Dalamud.PluginInterface.UiBuilder.LoadImage(Resources.chatter);

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

    public TextureWrap? ChatterImage { get; }
    private ChatManager ChatManager { get; }
    private ChatLogManager ChatLogManager { get; }
    public WindowSystem WindowSystem { get; private set; }
    public string Name => "ChatterPlugin";

    public void Dispose()
    {
        // ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Configuration?.Save(); // Should be auto-saved but let's be sure

        UnregisterCommands();
        ChatterImage?.Dispose();
        ChatLogManager?.Dispose();
        ChatManager?.Dispose();
        _windowManager?.Dispose();
        // ReSharper restore ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
    }
}