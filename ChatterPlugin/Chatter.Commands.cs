using System.Collections.Generic;
using Dalamud.Game.Command;

namespace ChatterPlugin;

public sealed partial class Chatter
{
    private readonly Dictionary<string, CommandInfo> commands = new();

    private void RegisterCommands()
    {
        commands["/chatter"] = new CommandInfo(OnChatter)
        {
            HelpMessage = "Opens the Chatter interface.",
            ShowInHelp = true
        };

        commands["/chatterconfig"] = new CommandInfo(OnChatterConfig)
        {
            HelpMessage = "Opens the Chatter configuration window.",
            ShowInHelp = true
        };
        commands["/chattercfg"] = commands["/chatterconfig"];

        commands["/chatterdebug"] = new CommandInfo(OnChatterDebug)
        {
            HelpMessage = "Toggles debug mode.",
            ShowInHelp = false
        };

        commands["/chatterdump"] = new CommandInfo(OnChatterDump)
        {
            HelpMessage = "Dumps the active chat log information.",
            ShowInHelp = false
        };

        foreach (var (command, info) in commands) Dalamud.Commands.AddHandler(command, info);
    }

    private void UnregisterCommands()
    {
        foreach (var command in commands.Keys) Dalamud.Commands.RemoveHandler(command);
    }

    private void OnChatter(string command, string args)
    {
        windowManager.ToggleMain();
    }

    private void OnChatterConfig(string command, string args)
    {
        windowManager.ToggleConfig();
    }

    private void OnChatterDebug(string command, string args)
    {
        Configuration.IsDebug = !Configuration.IsDebug;
    }

    private void OnChatterDump(string command, string args)
    {
        ChatLogManager.DumpLogs();
    }
}
