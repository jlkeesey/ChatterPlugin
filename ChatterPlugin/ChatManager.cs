using System;
using System.Collections.Generic;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace ChatterPlugin;

public sealed class ChatManager : IDisposable
{
    private readonly Chatter chatter;

    public ChatManager(Chatter plugin)
    {
        chatter = plugin;
        Dalamud.Chat.ChatMessage += HandleChatMessage;
    }

    public void Dispose()
    {
        Dalamud.Chat.ChatMessage -= HandleChatMessage;
    }

    private void HandleChatMessage(
        XivChatType xivType, uint senderId, ref SeString seSender, ref SeString seMessage, ref bool isHandled)
    {
        var type = CleanUpType(xivType);
        if (type != string.Empty && Chatter.Configuration.ChatTypeFilterFlags.GetValueOrDefault(xivType, false))
        {
            var sender = CleanUpSender(seSender);
            var message = CleanUpMessage(seMessage);
            LogChatMessage(type, sender, message);
        }
    }

    private static string CleanUpType(XivChatType xivType)
    {
        return ChatTypeHelper.TypeToName(xivType);
    }

    private static string CleanUpMessage(SeString seMessage)
    {
        var message = seMessage.TextValue;
        foreach (var server in DataCenter.Servers)
        {
            var startIndex = 0;
            while (true)
            {
                var index = message.IndexOf(server, startIndex, StringComparison.InvariantCulture);
                if (index == -1) break;
                message = message.Insert(index, "@");
                startIndex = index + server.Length + 1;
            }
        }

        return message;
    }

    private static string CleanUpSender(SeString sender)
    {
        var name = sender.TextValue;
        while (name.Length > 0 && !char.IsLetter(name[0])) name = name[1..];
        while (name.Length > 0 && !char.IsLetter(name[^1])) name = name[..^1];
        foreach (var server in DataCenter.Servers)
            if (name.EndsWith(server))
            {
                name = name.Insert(name.Length - server.Length, "@");
                break;
            }

        return name;
    }

    private void LogChatMessage(string type, string sender, string message)
    {
        // \ue05d is the flower character used to separate names from servers
        chatter.ChatLogManager[ChatLogManager.AllLogPrefix].LogInfo(type, sender, message);
    }
}
