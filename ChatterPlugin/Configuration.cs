using System;
using Dalamud.Configuration;

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
        if (Dalamud.PluginInterface.GetPluginConfig() is Configuration config)
        {
            // Add migration here
            return config;
        }

        config = new Configuration();
        config.Save();
        return config;
    }
}
