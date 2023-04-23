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

    /// <summary>
    ///     The configuration for a single chat log.
    /// </summary>
    public sealed class ChatLogConfiguration
    {
        public class ChatTypeFlag
        {
            public ChatTypeFlag(bool value = false)
            {
                Value = value;
            }

            public bool Value;
        }

        /// <summary>
        ///     The include/exclude flags for each ChatType.
        /// </summary>
        public readonly Dictionary<XivChatType, ChatTypeFlag> ChatTypeFilterFlags = new();

        public ChatLogConfiguration(
            string name, bool isActive = false, bool includeServer = false, bool includeMe = true,
            bool includeAll = false,
            string? format = null)
        {
            Name = name;
            IsActive = isActive;
            IncludeServer = includeServer;
            IncludeMe = includeMe;
            DebugIncludeAllMessages = includeAll;
            Format = format;
        }

        /// <summary>
        ///     The name of this group. Will be part of the log file name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     Whether this log is active and writing out to the file.
        /// </summary>
        public bool IsActive;

        /// <summary>
        /// If true then I am included in the log even if I'm not in the user list. This will generally be true always.
        /// </summary>
        public bool IncludeMe;

        /// <summary>
        ///     If this is true then server names are included in the output, otherwise they are stripped from
        ///     the output, both in the name column as well as the message.
        /// </summary>
        public bool IncludeServer;

        /// <summary>
        ///     When true all messages get written to this log including ones that normally would be filtered out.
        /// </summary>
        public bool DebugIncludeAllMessages;

        /// <summary>
        ///     The format string to use for formatting the messages.
        /// </summary>
        /// <remarks>
        ///     Every message will use this format pattern to format the message that is logged. If this is null then the default
        ///     depends on the type of chat log. Below is the list of replacement parameters that are permitted.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Replacement</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>{0}</term>
        ///             <description>The long chat type name.</description>
        ///         </item>
        ///         <item>
        ///             <term>{1}</term>
        ///             <description>The short chat type name.</description>
        ///         </item>
        ///         <item>
        ///             <term>{2}</term>
        ///             <description>
        ///                 The long sender name. This will include the world name if different from the user. This is
        ///                 without any name replacement.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>{3}</term>
        ///             <description>The short sender name. This may have the world remove and will have any replacements done.</description>
        ///         </item>
        ///         <item>
        ///             <term>{4}</term>
        ///             <description>The unmodified text of the chat message.</description>
        ///         </item>
        ///         <item>
        ///             <term>{5}</term>
        ///             <description>
        ///                 The cleaned text of the chat message. This may have the world names removed and will have any
        ///                 name replacements.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public string? Format;

        /// <summary>
        ///     The set of users to include.
        /// </summary>
        /// <remarks>
        ///     The key is the full name of the user to include, if the value is not string.Empty, it what the user's
        ///     name should be renamed in the output. If this is empty, then all users are included.
        /// </remarks>
        public SortedDictionary<string, string> Users { get; init; } = new();

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
                ChatTypeFilterFlags.TryAdd(type, new ChatTypeFlag(true));
        }
    }

    /// <summary>
    ///     The directory to write all logs to. This directory may not exist.
    /// </summary>
    public string LogDirectory = FileHelper.InitialLogDirectory();

    /// <summary>
    ///     The prefix for the logs. This will be the first part of all log file names.
    /// </summary>
    public string LogFileNamePrefix = "chatter";

    /// <summary>
    ///     The configurations for the individual chat logs.
    /// </summary>
    public Dictionary<string, ChatLogConfiguration> ChatLogs = new();

#if DEBUG
    public bool IsDebug = true;
#else
    public bool IsDebug = false;
#endif

    public int Version { get; set; } = 1;

    public void AddLog(ChatLogConfiguration logConfiguration)
    {
        ChatLogs[logConfiguration.Name] = logConfiguration;
    }

    public void RemoveLog(ChatLogConfiguration logConfiguration)
    {
        ChatLogs.Remove(logConfiguration.Name);
    }

    /// <summary>
    ///     Saves the current state of the configuration.
    /// </summary>
    public void Save()
    {
        Dalamud.PluginInterface.SavePluginConfig(this);
    }

    /// <summary>
    ///     Loads the most recently saved configuration or creates a new one.
    /// </summary>
    /// <returns>The configuration to use.</returns>
    public static Configuration Load()
    {
//         Configuration config = new Configuration();
        if (Dalamud.PluginInterface.GetPluginConfig() is not Configuration config) config = new Configuration();
        // else
        // {
        //     Add migration here
        // }

        if (!config.ChatLogs.ContainsKey(AllLogName))
        {
            config.AddLog(new ChatLogConfiguration(AllLogName, true));
        }

        var logConfiguration = new ChatLogConfiguration("Tifaa", true);
        logConfiguration.Users["Tifaa Sidrasylan"] = String.Empty;
        logConfiguration.Users["Aelym Sidrasylan"] = "Stud Muffin";
        logConfiguration.Users["Fiora Greyback"] = "The Oppressed";
        config.AddLog(logConfiguration);
        config.AddLog(new ChatLogConfiguration("Pups", true));
        config.AddLog(new ChatLogConfiguration("Goobtube", false));

        foreach (var (_, chatLogConfiguration) in config.ChatLogs) chatLogConfiguration.InitializeTypeFlags();

        config.Save();
        return config;
    }


    /// <summary>
    ///     These chat type should all be enabled by default on new configurations.
    /// </summary>
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