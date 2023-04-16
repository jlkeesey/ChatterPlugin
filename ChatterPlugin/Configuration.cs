using System;
using System.Collections.Generic;
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
    public const string AllLogName = "all";

    public sealed class ChatLogConfiguration
    {
        public readonly Dictionary<XivChatType, bool> ChatTypeFilterFlags = new();

        public ChatLogConfiguration(
            string name, bool isActive = false, bool includeServer = false, bool includeAll = false)
        {
            Name = name;
            IsActive = isActive;
            IncludeServer = includeServer;
            DebugIncludeAllMessages = includeAll;
        }

        /// <summary>
        ///     The name of this group. Will be part of the log file name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     Whether this log is active and writing out to the file.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///     If this is true then server names are included in the output, otherwise they are stripped from
        ///     the output, both in the name column as well as the message.
        /// </summary>
        public bool IncludeServer { get; set; }

        /// <summary>
        /// When true all messages get written to this log including ones that normally would be filtered out.
        /// </summary>
        public bool DebugIncludeAllMessages { get; set; }

        /// <summary>
        ///     The set of users to include.
        /// </summary>
        /// <remarks>
        ///     The key is the full name of the user to include, if the value is not string.Empty, it what the user's
        ///     name should be renamed in the output. If this is empty, then all users are included.
        /// </remarks>
        public SortedDictionary<string, string> Users { get; init; } = new SortedDictionary<string, string>();

        /// <summary>
        ///     Initializes the enabled chat type flags.
        /// </summary>
        /// <remarks>
        ///     This has two purposes. The first is to set the default flags for this object. The second is to
        ///     set any new chat types that did not exist is previously created configuration. Serialization will
        ///     do that to you.
        /// </remarks>
        public void InitializeTypeFlags()
        {
            ChatTypeFilterFlags.Clear(); // TODO remove this once setup is working
            foreach (var type in DefaultEnabledTypes)
                ChatTypeFilterFlags.TryAdd(type, true);
        }
    }

    /// <summary>
    ///     The directory to write all logs to. This directory may not exist.
    /// </summary>
    public string LogDirectory { get; set; } = FileHelper.InitialLogDirectory();

    /// <summary>
    ///     The prefix for the logs. This will be the first part of all log file names.
    /// </summary>
    public string LogFileNamePrefix { get; set; } = "chatter";

    /// <summary>
    ///     The configurations for the individual chat logs.
    /// </summary>
    public Dictionary<string, ChatLogConfiguration> ChatLogs { get; set; } = new();

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
        //     Add migration here
        // }

        if (!config.ChatLogs.ContainsKey(AllLogName))
        {
            config.ChatLogs[AllLogName] = new ChatLogConfiguration(AllLogName, isActive: true);
        }

        foreach (var (_, chatLogConfiguration) in config.ChatLogs) chatLogConfiguration.InitializeTypeFlags();

        config.Save();
        return config;
    }

    private static readonly List<XivChatType> DefaultEnabledTypes = new()
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
        XivChatType.CrossLinkShell8
    };
}
