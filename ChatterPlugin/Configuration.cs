using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Configuration;
using Dalamud.Game.Text;

namespace ChatterPlugin;

/// <summary>
///     Contains all of the user configuration settings.
/// </summary>
/// <remarks>
///     Config stuff.
/// </remarks>
[Serializable]
public class Configuration : IPluginConfiguration
{
    /// <summary>
    ///     The directory to write all logs to. This directory may not exist.
    /// </summary>
    public string LogDirectory { get; set; } = FileHelper.InitialLogDirectory();

    /// <summary>
    ///     The prefix for the logs. This will be the first part of all log file names.
    /// </summary>
    public string LogFileNamePrefix { get; set; } = "chatter";

    public readonly Dictionary<XivChatType, bool> ChatTypeFilterFlags = new();

#if DEBUG
    public bool IsDebug { get; set; } = true;
#else
    public bool IsDebug { get; set; } = false;
#endif

    public int Version { get; set; } = 1;

    /// <summary>
    ///     Saves the current state of the configuration.
    /// </summary>
    public void Save()
    {
        Dalamud.PluginInterface.SavePluginConfig(this);
    }

    public static Configuration Load()
    {
        if (Dalamud.PluginInterface.GetPluginConfig() is not Configuration config) config = new Configuration();
        // else
        // {
        //     // Add migration here
        // }

        InitializeTypeFlags(config);
        config.Save();
        return config;
    }

    private static void InitializeTypeFlags(Configuration config)
    {
        config.ChatTypeFilterFlags.Clear();  // TODO remove this once setup is working
        foreach (var type in DefaultEnabledTypes)
            config.ChatTypeFilterFlags.TryAdd(type, true);
    }

    private static List<XivChatType> DefaultEnabledTypes = new()
    {
        XivChatType.Say,
        XivChatType.TellOutgoing,
        XivChatType.TellIncoming,
        XivChatType.Shout,
        XivChatType.Party,
        XivChatType.Alliance,
        XivChatType.Ls1,
        XivChatType.Ls2,
        XivChatType.Ls3,
        XivChatType.Ls4,
        XivChatType.Ls5,
        XivChatType.Ls6,
        XivChatType.Ls7,
        XivChatType.Ls8,
        XivChatType.FreeCompany,
        XivChatType.NoviceNetwork,
        XivChatType.CustomEmote,
        XivChatType.StandardEmote,
        XivChatType.Yell,
        XivChatType.CrossParty,
        XivChatType.PvPTeam,
        XivChatType.CrossLinkShell1,
        XivChatType.CrossLinkShell2,
        XivChatType.CrossLinkShell3,
        XivChatType.CrossLinkShell4,
        XivChatType.CrossLinkShell5,
        XivChatType.CrossLinkShell6,
        XivChatType.CrossLinkShell7,
        XivChatType.CrossLinkShell8,
    };
}
