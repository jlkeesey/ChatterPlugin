using System;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;

namespace ChatterPlugin;

public sealed class ChatManager : IDisposable
{
    public ChatManager(Chatter plugin)
    {
        chatter = plugin;
        Dalamud.Chat.ChatMessage += HandleChatMessage;
    }

    private readonly Chatter chatter;

    public void Dispose()
    {
        Dalamud.Chat.ChatMessage -= HandleChatMessage;
    }

    private void HandleChatMessage(
        XivChatType xivType, uint senderId, ref SeString seSender, ref SeString seMessage, ref bool isHandled)
    {
        var type = CleanUpType(xivType);
        var sender = CleanUpSender(seSender);
        var message = CleanUpMessage(seMessage);
        LogChatMessage(type, sender, message);
    }

    private static string CleanUpType(XivChatType xivType)
    {
        return ChatTypeHelper.TypeToName(xivType);
    }

    private static string CleanUpMessage(SeString seMessage)
    {
        return seMessage.TextValue;
    }

    private static string CleanUpSender(SeString sender)
    {
        var name = sender.TextValue;
        while (name.Length > 0 && !char.IsLetter(name[0])) name = name[1..];
        while (name.Length > 0 && !char.IsLetter(name[^1])) name = name[..^1];
        return name;
    }

    private void LogChatMessage(string type, string sender, string message)
    {
        chatter.ChatLogManager[ChatLogManager.AllLogPrefix].WriteLine($"{type}:{sender}:{message}");
        chatter.ChatLogManager[ChatLogManager.AllLogPrefix].Flush();
        chatter.Settings.Log.WriteLine($"{type}:{sender}:{message}");
        chatter.Settings.Log.Flush();
    }
}
