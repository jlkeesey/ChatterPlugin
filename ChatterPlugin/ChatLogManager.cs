using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Game.Text;
using Dalamud.Logging;
using Dalamud.Utility;

namespace ChatterPlugin;

/// <summary>
///     Manages a set of logs.
/// </summary>
/// <remarks>
///     <para>
///         A set of logs is created based on the current configuration. If that configuration changes, then we
///         create a new set of logs that reflect the new settings.
///         There is one main or all log that contains all messages and one log for each group set up in the
///         configuration.
///     </para>
///     TODO is this still true?
///     <para>
///         We should only create a new set if the log path or file prefix changes. Other changes should just
///         create new logs or stop using old logs but we continue with the others.
///     </para>
/// </remarks>
public sealed class ChatLogManager : IDisposable
{
    private readonly Dictionary<string, ChatLog> logs = new();

    private string logDirectory = string.Empty;
    private string logFileNamePrefix = string.Empty;
    private DateTime logStartTime = DateTime.Now;

    public void Dispose()
    {
        CloseLogs();
    }

    public ChatLog GetLog(Configuration.ChatLogConfiguration cfg)
    {
        UpdateConfigValues();
        if (!logs.ContainsKey(cfg.Name)) logs[cfg.Name] = new ChatLog(this, cfg);

        return logs[cfg.Name].Open();
    }

    private void UpdateConfigValues()
    {
        if (Chatter.Configuration.LogDirectory != logDirectory ||
            Chatter.Configuration.LogFileNamePrefix != logFileNamePrefix)
        {
            CloseLogs();
            logDirectory = Chatter.Configuration.LogDirectory;
            logFileNamePrefix = Chatter.Configuration.LogFileNamePrefix;
            logStartTime = DateTime.Now;
        }
    }

    public void DumpLogs()
    {
        PluginLog.Log("Prefix        Open   Path");
        PluginLog.Log("------------  -----  ----");
        foreach (var (_, entry) in logs)
            entry.DumpLog();
    }

    private void CloseLogs()
    {
        foreach (var (_, entry) in logs)
            entry.Close();
    }

    /// <summary>
    ///     Sends the chat info to all of the currently defined logs. Each log decides what to do with the information.
    /// </summary>
    /// <param name="xivType">The chat type (say, tell, shout, etc.)</param>
    /// <param name="senderId">The id of the sender.</param>
    /// <param name="sender">The name of the sender. This may include a server name separated by and @ sign.</param>
    /// <param name="message">The text of chat. User names in the message may include a server name separated by and @ sign.</param>
    public void LogInfo(XivChatType xivType, uint senderId, string sender, string message)
    {
        foreach (var (_, configurationChatLog) in Chatter.Configuration.ChatLogs)
            GetLog(configurationChatLog).LogInfo(xivType, senderId, sender, message);
    }

    public class ChatLog : IDisposable
    {
        private readonly Configuration.ChatLogConfiguration config;
        private readonly ChatLogManager manager;

        public ChatLog(ChatLogManager chatLogManager, Configuration.ChatLogConfiguration configuration)
        {
            manager = chatLogManager;
            config = configuration;
        }

        public string FileName { get; private set; } = string.Empty;
        private StreamWriter Log { get; set; } = StreamWriter.Null;

        public void Dispose()
        {
            Close();
        }

        /// <summary>
        ///     Decides if the give log information compared with the configuration determines that the char information
        ///     should be written to this log.
        /// </summary>
        /// <param name="xivType">The chat type (say, tell, shout, etc.)</param>
        /// <param name="typeLabel">The string version of the chat type. Might be string.Empty.</param>
        /// <param name="senderId">The id of the sender.</param>
        /// <param name="s"></param>
        /// <param name="sender">The name of the sender. This may include a server name separated by and @ sign.</param>
        /// <param name="message">The text of chat. User names in the message may include a server name separated by and @ sign.</param>
        /// <returns></returns>
        protected bool ShouldLog(
            XivChatType xivType, string typeLabel, uint senderId, string sender, string cleanedSender, string message)
        {
            if (!config.IsActive) return false;
            if (config.DebugIncludeAllMessages) return true;
            if (config.Users.Count != 0 && !config.Users.ContainsKey(cleanedSender)) return false;
            return !typeLabel.IsNullOrEmpty() && config.ChatTypeFilterFlags.GetValueOrDefault(xivType, false);
        }

        /// <summary>
        ///     Logs the chat information to the target log.
        /// </summary>
        /// <remarks>
        ///     The configuration defines whether the given message should be logged as well as what massaging of the data
        ///     is required.
        /// </remarks>
        /// <param name="xivType">The chat type (say, tell, shout, etc.)</param>
        /// <param name="senderId">The id of the sender.</param>
        /// <param name="sender">The name of the sender. This may include a server name separated by and @ sign.</param>
        /// <param name="message">The text of chat. User names in the message may include a server name separated by and @ sign.</param>
        public void LogInfo(XivChatType xivType, uint senderId, string sender, string message)
        {
            var typeLabel = ChatTypeHelper.TypeToName(xivType);
            var cleanedSender = CleanUpSender(sender);
            if (ShouldLog(xivType, typeLabel, senderId, sender, cleanedSender, message))
            {
                var cleanedMessage = CleanUpMessage(message);
                WriteLog(xivType, typeLabel, senderId, sender, cleanedSender, message, cleanedMessage);
            }
        }

        protected void WriteLog(
            XivChatType xivType, string typeLabel, uint senderId, string sender, string cleanedSender, string message,
            string cleanedMessage)
        {
            WriteLine($"{typeLabel}:{cleanedSender}:{cleanedMessage}");
        }

        private static string CleanUpMessage(string message)
        {
            var cleanedMessage = message;
            foreach (var server in DataCenter.Servers)
            {
                var startIndex = 0;
                while (true)
                {
                    var index = cleanedMessage.IndexOf(server, startIndex, StringComparison.InvariantCulture);
                    if (index == -1) break;
                    cleanedMessage = cleanedMessage.Insert(index, "@");
                    startIndex = index + server.Length + 1;
                }
            }

            return cleanedMessage;
        }

        private string CleanUpSender(string sender)
        {
            var cleanedSender = sender;
            if (!config.IncludeServer)
            {
                var index = sender.LastIndexOf('@');
                if (index == -1) cleanedSender = cleanedSender.Remove(index);
            }

            var replacementUser = config.Users.GetValueOrDefault(cleanedSender, string.Empty);
            if (replacementUser != string.Empty) cleanedSender = replacementUser;

            return cleanedSender;
        }

        public void WriteLine(string line)
        {
            Log.WriteLine(line);
            Log.Flush();
        }

        public ChatLog Open()
        {
            if (Log == StreamWriter.Null)
            {
                FileName =
                    FileHelper.FullFileNameWithDateTime(manager.logDirectory,
                                                        $"{manager.logFileNamePrefix}-{config.Name}",
                                                        FileHelper.LogFileExtension,
                                                        manager.logStartTime);
                FileHelper.EnsureDirectoryExists(manager.logDirectory);
                Log = new StreamWriter(FileName);
            }

            return this;
        }

        public void Close()
        {
            if (Log != StreamWriter.Null)
            {
                Log.Close();
                Log = StreamWriter.Null;
                FileName = string.Empty;
            }
        }

        public void DumpLog()
        {
            PluginLog.Log($"{config.Name,-12}  {Log != StreamWriter.Null,-5}  '{FileName}'");
        }
    }
}
