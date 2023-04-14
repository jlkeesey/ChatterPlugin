using System;

namespace ChatterPlugin.Attributes
{
    /// <summary>
    /// Marks this function as a Dalamud slash command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Command { get; }

        /// <summary>
        /// The slash command to invoke this action.
        /// </summary>
        /// <param name="command">The slash command name.</param>
        public CommandAttribute(string command)
        {
            Command = command;
        }
    }
}
