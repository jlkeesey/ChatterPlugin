using System;

namespace ChatterPlugin.Attributes
{
    /// <summary>
    /// The help message text for this command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HelpMessageAttribute : Attribute
    {
        public string HelpMessage { get; }

        /// <summary>
        /// The help text.
        /// </summary>
        /// <param name="helpMessage">The help text.</param>
        public HelpMessageAttribute(string helpMessage)
        {
            HelpMessage = helpMessage;
        }
    }
}
