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
    private readonly Configuration configuration;

    private StreamWriter log = StreamWriter.Null;

    private string logDirectory = string.Empty;
    private string logFileName = string.Empty;
    private string logFileNamePrefix = string.Empty;

    public Settings(Configuration configuration)
    {
        this.configuration = configuration;
    }

    public string LogFileName
    {
        get
        {
            if (configuration.LogFileNamePrefix != logFileNamePrefix)
            {
                CloseLog();
                logFileName = string.Empty;
                logFileNamePrefix = configuration.LogFileNamePrefix;
            }

            if (configuration.LogDirectory != logDirectory)
            {
                CloseLog();
                logFileName = string.Empty;
                logDirectory = configuration.LogDirectory;
            }

            if (logFileName == string.Empty)
            {
                logFileName =
                    FileHelper.FullFileNameWithDateTime(logDirectory, logFileNamePrefix, FileHelper.LogFileExtension);
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
                FileHelper.EnsureDirectoryExists(logDirectory);
                PluginLog.Log("@@@@ Created log directory '{0}'", logDirectory);
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
