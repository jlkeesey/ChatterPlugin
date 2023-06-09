using Dalamud.Data;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

// Re Sharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Chatter;

/// <summary>
///     Global access to all of the Dalamud services that we use.
/// </summary>
public class Dalamud
{
    /// <summary>
    ///     Initializes this object by telling the Dalamud plugin interface to do injection.
    /// </summary>
    /// <param name="pluginInterface">The <see cref="DalamudPluginInterface" /> to use for injection.</param>
    public static void Initialize(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Dalamud>();
    }

    // @formatter:off
    [PluginService][RequiredVersion("1.0")] public static ChatGui Chat { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static ClientState ClientState { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static CommandManager Commands { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static DataManager GameData { get; private set; } = null!;

    //[PluginService][RequiredVersion("1.0")] public static BuddyList              Buddies         { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static ChatHandlers           ChatHandlers    { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static Condition              Conditions      { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static FateTable              Fates           { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static FlyTextGui             FlyTexts        { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static Framework              Framework       { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static GameGui                GameGui         { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static GameNetwork            Network         { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static JobGauges              Gauges          { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static KeyState               Keys            { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static LibcFunction           LibC            { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static ObjectTable            Objects         { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static PartyFinderGui         PartyFinder     { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static PartyList              Party           { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static SeStringManager        SeStrings       { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static SigScanner             SigScanner      { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static TargetManager          Targets         { get; private set; } = null!;
    //[PluginService][RequiredVersion("1.0")] public static ToastGui               Toasts          { get; private set; } = null!;
    // @formatter:on
}