using System.Collections.Generic;
using Dalamud.Game.Text;

namespace ChatterPlugin;

/// <summary>
///     Utilities for working with the <see cref="XivChatType" /> enum.
/// </summary>
internal class ChatTypeHelper
{
    private static readonly Dictionary<XivChatType, string> ChatCodeToShortName = new()
    {
        {XivChatType.TellOutgoing, "tellOut"}
    };

    /// <summary>
    ///     Converts the <see cref="XivChatType" /> to a string. We use the one on the enum if present, or lookup in
    ///     <see cref="ChatCodeToShortName" /> if not.
    /// </summary>
    /// <param name="chatType">The chat type to examine.</param>
    /// <returns>The corresponding name or a name in the form '?45?' if none found.</returns>
    public static string TypeToName(XivChatType chatType)
    {
        var details = chatType.GetDetails();
        var defaultValue = Chatter.Configuration.IsDebug ? $"?{(int) chatType}?" : string.Empty;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return details != null ? details.Slug : ChatCodeToShortName.GetValueOrDefault(chatType, defaultValue);
    }
}