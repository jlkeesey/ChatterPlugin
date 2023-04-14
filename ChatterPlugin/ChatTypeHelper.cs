using System.Collections.Generic;
using Dalamud.Game.Text;

namespace ChatterPlugin;

internal class ChatTypeHelper
{
    private static readonly Dictionary<XivChatType, string> ChatCodeToShortName = new()
    {
        {XivChatType.None, "-"},
        {XivChatType.Debug, "debug"},
        {XivChatType.TellOutgoing, "tellOut"},
        {XivChatType.SystemError, "systemError"},
        {XivChatType.SystemMessage, "systemMessage"},
        {XivChatType.GatheringSystemMessage, "gatheringSystemMessage"},
        {XivChatType.ErrorMessage, "errorMessage"},
        {XivChatType.NPCDialogue, "npcDialogue"},
        {XivChatType.NPCDialogueAnnouncements, "npcDialogueAnnouncements"},
        {XivChatType.RetainerSale, "retainerSale"}
        // {(XivChatType)72, "recruiting"},
        // {(XivChatType)76, "playlist"},
        // {(XivChatType)2105, "retainerAdd"},
        // {(XivChatType)2110, "inventory"},
        // {(XivChatType)3129, "retainerPay"},
        // {(XivChatType)2115, "gathering"},
    };

    // TODO filter out any value we don't recognize. There are a lot of them.
    public static string TypeToName(XivChatType chatType)
    {
        var details = chatType.GetDetails();
        var defaultValue = Chatter.Configuration.IsDebug ? $"?{(int)chatType}?" : string.Empty;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return details != null ? details.Slug : ChatCodeToShortName.GetValueOrDefault(chatType, defaultValue);
    }
}
