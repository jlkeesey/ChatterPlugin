using System;
using System.IO;
using Dalamud.Logging;

namespace ChatterPlugin;

/// <summary>
///     This encapsulates all of the derived values from the Configuration.
/// </summary>
/// <remarks>
///     While the Configuration contains the values that the user enters and are in human-usable
///     formats, this object contains the derived values that the plugin needs in order to run.
/// </remarks>
public sealed class Settings : IDisposable
{
    private StreamWriter log = StreamWriter.Null;

    private string logDirectory = string.Empty;
    private string logFileName = string.Empty;
    private string logFileNamePrefix = string.Empty;

    public string LogDirectory
    {
        get
        {
            if (Chatter.Configuration.LogDirectory != logDirectory)
            {
                CloseLog();
                logFileName = string.Empty;
                logDirectory = Chatter.Configuration.LogDirectory;
            }

            return logDirectory;
        }
    }

    public string LogFileNamePrefix
    {
        get
        {
            if (Chatter.Configuration.LogFileNamePrefix != logFileNamePrefix)
            {
                CloseLog();
                logFileName = string.Empty;
                logFileNamePrefix = Chatter.Configuration.LogFileNamePrefix;
            }

            return logFileNamePrefix;
        }
    }

    public string LogFileName
    {
        get
        {
            var directory = LogDirectory;   // Side effect
            var prefix = LogFileNamePrefix; // Side effect
            if (logFileName == string.Empty)
            {
                logFileName =
                    FileHelper.FullFileNameWithDateTime(directory, prefix, FileHelper.LogFileExtension);
            }

            return logFileName;
        }
    }

    /// <summary>
    ///     Returns the target for the complete chatter log.
    /// </summary>
    public StreamWriter Log
    {
        get
        {
            if (log == StreamWriter.Null)
            {
                FileHelper.EnsureDirectoryExists(LogDirectory);
                PluginLog.Log("@@@@ Log directory exists '{0}'", LogDirectory);
                log = new StreamWriter(LogFileName);
                PluginLog.Log("@@@@ Created log file '{0}'", LogFileName);
            }

            return log;
        }
    }

    public void Dispose()
    {
        CloseLog();
    }

    private void CloseLog()
    {
        if (log != StreamWriter.Null)
        {
            log.Close();
            log = StreamWriter.Null;
        }
    }
}
