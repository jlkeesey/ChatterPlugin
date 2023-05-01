using System;
using System.Collections.Generic;
using System.IO;
using ChatterPlugin.Model;
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
///         create a new set of logs that reflect the new settings. There is one main or all log that contains all messages
///         and one log for each group set up in the configuration.
///     </para>
/// </remarks>
public sealed class ChatLogManager : IDisposable
{
    private readonly Dictionary<string, ChatLog> _logs = new();

    private string _logDirectory = string.Empty;
    private string _logFileNamePrefix = string.Empty;
    private DateTime _logStartTime = DateTime.Now;

    public void Dispose()
    {
        CloseLogs();
    }

    /// <summary>
    ///     Returns the <see cref="ChatLog"/> for the given configuration. If one does not exist then a new one is created.
    /// </summary>
    /// <param name="cfg">The configuration to use for this log.</param>
    /// <returns>The <see cref="ChatLog"/></returns>
    public ChatLog GetLog(Configuration.ChatLogConfiguration cfg)
    {
        UpdateConfigValues();
        if (!_logs.ContainsKey(cfg.Name))
            _logs[cfg.Name] = cfg.Name == Configuration.AllLogName
                ? new AllChatLog(this, cfg)
                : new GroupChatLog(this, cfg);

        return _logs[cfg.Name];
    }

    /// <summary>
    ///     Checks if any configuration values have changed that warrant the closing and possible reopening of the
    ///     currently open log files.
    /// </summary>
    private void UpdateConfigValues()
    {
        if (Chatter.Configuration.LogDirectory == _logDirectory &&
            Chatter.Configuration.LogFileNamePrefix == _logFileNamePrefix) return;
        CloseLogs();
        _logDirectory = Chatter.Configuration.LogDirectory;
        _logFileNamePrefix = Chatter.Configuration.LogFileNamePrefix;
        _logStartTime = DateTime.Now;
    }

    /// <summary>
    ///     Dumps the currently open configuration files to the dev log.
    /// </summary>
    public void DumpLogs()
    {
        PluginLog.Log("Prefix        Open   Path");
        PluginLog.Log("------------  -----  ----");
        foreach (var (_, entry) in _logs)
            entry.DumpLog();
    }

    /// <summary>
    ///     Closes all of the open log files.
    /// </summary>
    private void CloseLogs()
    {
        foreach (var (_, entry) in _logs)
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

    /// <summary>
    ///     Defines the handler for a single log file.
    /// </summary>
    public abstract class ChatLog : IDisposable
    {
        private static readonly Configuration.ChatLogConfiguration.ChatTypeFlag DefaultChatTypeFlag = new();
        private readonly ChatLogManager _manager;
        protected readonly Configuration.ChatLogConfiguration Config;

        protected ChatLog(ChatLogManager chatLogManager, Configuration.ChatLogConfiguration configuration)
        {
            _manager = chatLogManager;
            Config = configuration;
        }

        /// <summary>
        ///     The name of the log file if this log is open.
        /// </summary>
        public string FileName { get; private set; } = string.Empty;

        /// <summary>
        ///     The <see cref="StreamWriter"/> that we are writing to.
        /// </summary>
        private StreamWriter Log { get; set; } = StreamWriter.Null;

        /// <summary>
        ///     The default format string for this log.
        /// </summary>
        public abstract string DefaultFormat { get; }

        /// <summary>
        ///     Returns true if this log is open.
        /// </summary>
        public bool IsOpen => Log != StreamWriter.Null;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Close();
        }

        /// <summary>
        ///     Determines if the give log information should be sent to the log output by examining the configuration.
        /// </summary>
        /// <param name="chatMessage">The chat message information.</param>
        /// <returns><c>true</c> if this message should be logged.</returns>
        protected virtual bool ShouldLog(ChatMessage chatMessage)
        {
            if (!Config.IsActive) return false;
            if (Config.DebugIncludeAllMessages) return true;
            return !chatMessage.TypeLabel.IsNullOrEmpty() &&
                   Config.ChatTypeFilterFlags.GetValueOrDefault(chatMessage.ChatType, DefaultChatTypeFlag).Value;
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
            var chatMessage = new ChatMessage(Config, xivType, senderId, sender, message);
            if (ShouldLog(chatMessage)) WriteLog(chatMessage);
        }

        /// <summary>
        ///     Formats the chat message using this logs format string and sends it to the log output.
        /// </summary>
        /// <param name="chatMessage">The chat message information.</param>
        protected void WriteLog(ChatMessage chatMessage)
        {
            var format = Config.Format ?? DefaultFormat;
            var logText = string.Format(format, chatMessage.TypeLabel, chatMessage.ShortTypeLabel, chatMessage.Sender,
                chatMessage.CleanedSender, chatMessage.Message, chatMessage.CleanedMessage);
            WriteLine(logText);
        }

        /// <summary>
        ///     Writes a single line to the log output. The output is flushed after every line.
        /// </summary>
        /// <param name="line">The text to output.</param>
        private void WriteLine(string line)
        {
            Open();
            Log.WriteLine(line);
            Log.Flush();
        }

        /// <summary>
        ///     Opens this log if it is not already open. A new filename may be created if config values have changed.
        /// </summary>
        public void Open()
        {
            if (Log != StreamWriter.Null) return;
            FileName =
                FileHelper.FullFileNameWithDateTime(_manager._logDirectory,
                    $"{_manager._logFileNamePrefix}-{Config.Name}",
                    FileHelper.LogFileExtension,
                    _manager._logStartTime);
            FileHelper.EnsureDirectoryExists(_manager._logDirectory);
            Log = new StreamWriter(FileName, true);
        }

        /// <summary>
        ///     Closes this log if open.
        /// </summary>
        public void Close()
        {
            if (Log != StreamWriter.Null)
            {
                Log.Close();
                Log = StreamWriter.Null;
                FileName = string.Empty;
            }
        }

        /// <summary>
        ///     Dumps the information about this logger to the dev log.
        /// </summary>
        public void DumpLog()
        {
            PluginLog.Log($"{Config.Name,-12}  {Log != StreamWriter.Null,-5}  '{FileName}'");
        }

        /// <summary>
        ///     The information about a single chat message line.
        /// </summary>
        protected class ChatMessage
        {
            private readonly Configuration.ChatLogConfiguration _config;
            private string? _cleanedMessage;
            private string? _cleanedSender;
            private string? _typeLabel;

            public ChatMessage(Configuration.ChatLogConfiguration configuration, XivChatType xivType, uint senderId,
                string sender, string message)
            {
                _config = configuration;
                ChatType = xivType;
                SenderId = senderId;
                Sender = sender;
                Message = message;
            }

            public XivChatType ChatType { get; init; }
            public uint SenderId { get; init; }
            public string Sender { get; init; }
            public string Message { get; init; }


            /// <summary>
            ///     Returns the long version of the type label.
            /// </summary>
            public string TypeLabel
            {
                get { return _typeLabel ??= ChatTypeHelper.TypeToName(ChatType); }
            }

            /// <summary>
            ///     Returns the short version of the type label, usually 1-3 characters.
            /// </summary>
            public string ShortTypeLabel => TypeLabel;

            /// <summary>
            ///     Returns the cleaned sender name. This includes removing the world name and doing any name replacements.
            /// </summary>
            public string CleanedSender
            {
                get { return _cleanedSender ??= CleanUpSender(Sender); }
            }

            /// <summary>
            ///     Returns the cleaned version of the message text. Currently this merely removes worlds from names.
            /// </summary>
            public string CleanedMessage
            {
                get { return _cleanedMessage ??= CleanUpMessage(Message); }
            }

            private string CleanUpSender(string sender)
            {
                var cleanedSender = sender;
                if (!_config.IncludeServer)
                {
                    var index = sender.LastIndexOf('@');
                    if (index != -1) cleanedSender = cleanedSender.Remove(index);
                }

                return _config.Users.GetValueOrDefault(cleanedSender, cleanedSender);
            }

            private string CleanUpMessage(string message)
            {
                var cleanedMessage = message;
                if (!_config.IncludeServer)
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
        }
    }

    /// <summary>
    ///     Chat log for group based logs.
    /// </summary>
    public class GroupChatLog : ChatLog
    {
        public GroupChatLog(ChatLogManager chatLogManager, Configuration.ChatLogConfiguration configuration) : base(
            chatLogManager, configuration)
        {
        }

        public override string DefaultFormat => "{3}   {5}";

        protected override bool ShouldLog(ChatMessage chatMessage)
        {
            return base.ShouldLog(chatMessage) &&
                   (Config.IncludeMe || Config.Users.ContainsKey(chatMessage.CleanedSender));
        }
    }

    /// <summary>
    ///     Chat log for the log that record everything.
    /// </summary>
    public class AllChatLog : ChatLog
    {
        public AllChatLog(ChatLogManager chatLogManager, Configuration.ChatLogConfiguration configuration) : base(
            chatLogManager, configuration)
        {
        }

        public override string DefaultFormat => "{0}:{2}:{4}";
    }
}