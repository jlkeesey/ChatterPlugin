using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Command;
using Dalamud.Logging;
using Dalamud.Utility;

namespace ChatterPlugin;

public sealed partial class Chatter
{
    private const string CommandChatter = "/chatter";
    private const string CommandConfig = "/chatterconfig";
    private const string CommandCfg = "/chattercfg";
    private const string CommandDebug = "/chatterdebug";

    private const string DebugChatDump = "chatdump";
    private const string DebugList = "list";

    private readonly Dictionary<string, CommandInfo> _commands = new();

    private readonly Dictionary<string, Func<bool>> _debugFlags = new()
    {
        {"debug", () => Configuration.IsDebug}
    };

    private void RegisterCommands()
    {
        // _commands[CommandChatter] = new CommandInfo(OnChatter)
        // {
        //     HelpMessage = "Opens the Chatter interface.",
        //     ShowInHelp = true
        // };

        _commands[CommandConfig] = new CommandInfo(OnChatterConfig)
        {
            HelpMessage = "Opens the Chatter configuration window.",
            ShowInHelp = true
        };
        _commands[CommandChatter] = _commands[CommandConfig];
        _commands[CommandCfg] = _commands[CommandConfig];

        _commands[CommandDebug] = new CommandInfo(OnChatterDebug)
        {
            HelpMessage = "Executes debug commands",
            ShowInHelp = false
        };

        foreach (var (command, info) in _commands) Dalamud.Commands.AddHandler(command, info);
    }

    private void UnregisterCommands()
    {
        foreach (var command in _commands.Keys) Dalamud.Commands.RemoveHandler(command);
    }

    // private void OnChatter(string command, string arguments)
    // {
    //     _windowManager.ToggleMain();
    // }

    private void OnChatterConfig(string command, string arguments)
    {
        _windowManager.ToggleConfig();
    }

    private void OnChatterDebug(string command, string arguments)
    {
        PluginLog.Log($"Debug command: '{arguments}'");
        PluginLog.Debug($"Debug command: '{arguments}'");
        if (arguments.IsNullOrEmpty())
        {
            Configuration.IsDebug = !Configuration.IsDebug;
            PluginLog.Debug($"Debug mode is {(Configuration.IsDebug ? "on" : "off")}");
        }
        else
        {
            var args = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var debugCommand = args[0].ToLower();
            switch (debugCommand)
            {
                case DebugChatDump:
                    ChatLogManager.DumpLogs();
                    break;
                case DebugList:
                    ListDebugFlags();
                    break;
                default:
                    PluginLog.Debug($"Debug command not recognized: '{debugCommand}'");
                    break;
            }
        }

        void ListDebugFlags()
        {
            var length = _debugFlags.Keys.Select(x => x.Length).Max();
            var flagString = "Flag".PadRight(length);
            var flagUnderscores = new string('-', length);
            PluginLog.Debug($"{flagString}  on/off");
            PluginLog.Debug($"{flagUnderscores}  ------");
            foreach (var (name, func) in _debugFlags) ListDebugFlag(name, func(), length);
        }

        void ListDebugFlag(string name, bool value, int length)
        {
            var onOff = value ? "on" : "off";
            PluginLog.Debug($"{name.PadRight(length)}  {onOff}");
        }
    }
}