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

    private readonly Dictionary<string, LogEntry> logs = new()
    {
        {
            AllLogPrefix, new LogEntry(AllLogPrefix)
        }
    };

    private string logDirectory = string.Empty;
    private string logFileNamePrefix = string.Empty;

    public StreamWriter this[string index]
    {
        get
        {
            UpdateConfigValues();
            if (!logs.ContainsKey(index)) logs[index] = new LogEntry(index);

            var entry = logs[index];
            if (entry.Log == StreamWriter.Null)
            {
                entry.FileName =
                    FileHelper.FullFileNameWithDateTime(logDirectory, $"{logFileNamePrefix}-{entry.Prefix}",
                                                        FileHelper.LogFileExtension);
                FileHelper.EnsureDirectoryExists(logDirectory);
                entry.Log = new StreamWriter(entry.FileName);
            }

            return entry.Log;
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
        }
    }

    public void DumpLogs()
    {
        foreach (var (_, entry) in logs)
            PluginLog.Log($"{entry.Prefix}: {entry.Log != StreamWriter.Null}  '{entry.FileName}'");
    }

    private void CloseLogs()
    {
        foreach (var (_, entry) in logs)
            if (entry.Log != StreamWriter.Null)
            {
                entry.Log.Close();
                entry.Log = StreamWriter.Null;
                entry.FileName = string.Empty;
            }
    }

    private class LogEntry
    {
        public readonly string Prefix;
        public string FileName = string.Empty;
        public StreamWriter Log = StreamWriter.Null;

        public LogEntry(string prefix)
        {
            Prefix = prefix;
        }
    }
}
