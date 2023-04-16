using System;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace ChatterPlugin;

public sealed class ChatManager : IDisposable
{
    private readonly ChatLogManager logManager;

    public ChatManager(ChatLogManager logManager)
    {
        this.logManager = logManager;
        Dalamud.Chat.ChatMessage += HandleChatMessage;
    }

    public void Dispose()
    {
        Dalamud.Chat.ChatMessage -= HandleChatMessage;
    }

    private void HandleChatMessage(
        XivChatType xivType, uint senderId, ref SeString seSender, ref SeString seMessage, ref bool isHandled)
    {
        var sender = CleanUpSender(seSender);
        var message = CleanUpMessage(seMessage);
        logManager.LogInfo(xivType, senderId, sender, message);
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

    private static string CleanUpSender(SeString seSender)
    {
        var sender = seSender.TextValue;
        while (sender.Length > 0 && !char.IsLetter(sender[0])) sender = sender[1..];
        while (sender.Length > 0 && !char.IsLetter(sender[^1])) sender = sender[..^1];
        foreach (var server in DataCenter.Servers)
            if (sender.EndsWith(server))
            {
                sender = sender.Insert(sender.Length - server.Length, "@");
                break;
            }

        return sender;
    }
}
