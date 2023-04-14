using System;

namespace ChatterPlugin.Attributes
{
    /// <summary>
    /// Defines an alias for this command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AliasesAttribute : Attribute
    {
        public string[] Aliases { get; }

        /// <summary>
        /// The set of aliases for this command.
        /// </summary>
        /// <param name="aliases">The aliases.</param>
        public AliasesAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}
