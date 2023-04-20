using System.Collections.Generic;
using Dalamud.Game.Text;

namespace ChatterPlugin;

internal class ChatTypeHelper
{
    private static readonly Dictionary<XivChatType, string> ChatCodeToShortName = new()
    {
        {XivChatType.TellOutgoing, "tellOut"},
    };

    public static string TypeToName(XivChatType chatType)
    {
        var details = chatType.GetDetails();
        var defaultValue = Chatter.Configuration.IsDebug ? $"?{(int) chatType}?" : string.Empty;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return details != null ? details.Slug : ChatCodeToShortName.GetValueOrDefault(chatType, defaultValue);
    }
}