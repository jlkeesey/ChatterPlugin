using System.Collections.Generic;
using Dalamud.Game.Text;

namespace ChatterPlugin;

internal class ChatTypeHelper
{
    private static readonly Dictionary<XivChatType, string> ChatCodeToShortName = new()
    {
        {XivChatType.TellOutgoing, "tellOut"},
        // {XivChatType.None, "-"},
        // {XivChatType.Debug, "debug"},
        // {XivChatType.SystemError, "systemError"},
        // {XivChatType.SystemMessage, "systemMessage"},
        // {XivChatType.GatheringSystemMessage, "gatheringSystemMessage"},
        // {XivChatType.ErrorMessage, "errorMessage"},
        // {XivChatType.NPCDialogue, "npcDialogue"},
        // {XivChatType.NPCDialogueAnnouncements, "npcDialogueAnnouncements"},
        // {XivChatType.RetainerSale, "retainerSale"}
        // {(XivChatType)76, "playlist"},
    };

    public static string TypeToName(XivChatType chatType)
    {
        var details = chatType.GetDetails();
        var defaultValue = Chatter.Configuration.IsDebug ? $"?{(int)chatType}?" : string.Empty;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return details != null ? details.Slug : ChatCodeToShortName.GetValueOrDefault(chatType, defaultValue);
    }
}
