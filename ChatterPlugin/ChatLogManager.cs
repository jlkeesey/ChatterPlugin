using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Logging;

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
    public const string AllLogPrefix = "all";

    private readonly Dictionary<string, ChatLog> logs = new();

    private string logDirectory = string.Empty;
    private string logFileNamePrefix = string.Empty;
    private DateTime logStartTime = DateTime.Now;

    public ChatLog this[string index]
    {
        get
        {
            UpdateConfigValues();
            if (!logs.ContainsKey(index)) logs[index] = new ChatLog(this, index);
            return logs[index].Open();
        }
    }

    public void Dispose()
    {
        CloseLogs();
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

    public sealed class ChatLog : IDisposable
    {
        private readonly ChatLogManager manager;

        public ChatLog(ChatLogManager chatLogManager, string prefix)
        {
            manager = chatLogManager;
            Prefix = prefix;
        }

        public string Prefix { get; init; }
        public string FileName { get; private set; } = string.Empty;
        private StreamWriter Log { get; set; } = StreamWriter.Null;

        public void Dispose()
        {
            Close();
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
                    FileHelper.FullFileNameWithDateTime(manager.logDirectory, $"{manager.logFileNamePrefix}-{Prefix}",
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
            PluginLog.Log($"{Prefix,-12}  {Log != StreamWriter.Null,-5}  '{FileName}'");
        }
    }
}
