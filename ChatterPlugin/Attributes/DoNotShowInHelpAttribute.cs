using System;

namespace ChatterPlugin.Attributes
{
    /// <summary>
    /// Hides this command from the help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DoNotShowInHelpAttribute : Attribute
    {
    }
}
