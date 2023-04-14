using Dalamud.Game.Command;
using ChatterPlugin.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Dalamud.Game.Command.CommandInfo;

namespace ChatterPlugin
{
    /// <summary>
    /// Registers all of the slash commands in the given class.
    /// </summary>
    /// <typeparam name="THost">The class type containing the commands.</typeparam>
    public class PluginCommandManager<THost> : IDisposable
    {
        private readonly (string, CommandInfo)[] pluginCommands;
        private readonly THost host;

        /// <summary>
        /// Creates the command manager for the given class type.
        /// </summary>
        /// <param name="host">The class that contains the slash command definitions.</param>
        /// <exception cref="ArgumentNullException">If any of the parameters are null.</exception>
        public PluginCommandManager(THost host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));

            this.pluginCommands = host.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public |
                                                            BindingFlags.Static | BindingFlags.Instance)
                .Where(method => method.GetCustomAttribute<CommandAttribute>() != null)
                .SelectMany(GetCommandInfoTuple)
                .ToArray();

            AddCommandHandlers();
        }

        private void AddCommandHandlers()
        {
            foreach (var (command, commandInfo) in this.pluginCommands)
            {
                Dalamud.Commands.AddHandler(command, commandInfo);
            }
        }

        private void RemoveCommandHandlers()
        {
            foreach (var (command, _) in this.pluginCommands)
            {
                Dalamud.Commands.RemoveHandler(command);
            }
        }

        private IEnumerable<(string, CommandInfo)> GetCommandInfoTuple(MethodInfo method)
        {
            var handlerDelegate = (HandlerDelegate)Delegate.CreateDelegate(typeof(HandlerDelegate), this.host, method);

            var command = handlerDelegate.Method.GetCustomAttribute<CommandAttribute>();
            var aliases = handlerDelegate.Method.GetCustomAttribute<AliasesAttribute>();
            var helpMessage = handlerDelegate.Method.GetCustomAttribute<HelpMessageAttribute>();
            var doNotShowInHelp = handlerDelegate.Method.GetCustomAttribute<DoNotShowInHelpAttribute>();

            var commandInfo = new CommandInfo(handlerDelegate)
            {
                HelpMessage = helpMessage?.HelpMessage ?? string.Empty,
                ShowInHelp = doNotShowInHelp == null,
            };

            // Create list of tuples that will be filled with one tuple per alias, in addition to the base command tuple.
            var commandInfoTuples = new List<(string, CommandInfo)> { (command!.Command, commandInfo) };
            if (aliases != null)
            {
                foreach (var alias in aliases.Aliases)
                {
                    commandInfoTuples.Add((alias, commandInfo));
                }
            }

            return commandInfoTuples;
        }

        public void Dispose()
        {
            RemoveCommandHandlers();
            GC.SuppressFinalize(this);
        }
    }
}
