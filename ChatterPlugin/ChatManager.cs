using System;
using ChatterPlugin.Model;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace ChatterPlugin;

/// <summary>
///     Handles capturing chat messages and passing them on the to chat log manager for processing.
/// </summary>
public sealed class ChatManager : IDisposable
{
    private readonly ChatLogManager _logManager;

    public ChatManager(ChatLogManager logManager)
    {
        _logManager = logManager;
        Dalamud.Chat.ChatMessage += HandleChatMessage;
    }

    public void Dispose()
    {
        Dalamud.Chat.ChatMessage -= HandleChatMessage;
    }

    /// <summary>
    ///     Chat message handler. This is called for every chat message that passes through the system.
    /// </summary>
    /// <param name="xivType">The chat type.</param>
    /// <param name="senderId">The id of the sender.</param>
    /// <param name="seSender">
    ///     The name of the sender. The will include the world name if the world is different from the user,
    ///     but the world will not be separated from the user name.
    /// </param>
    /// <param name="seMessage">
    ///     The chat message text. User names will include the world name is the world is different from the user,
    ///     but the world will not be separated from the user name.
    /// </param>
    /// <param name="isHandled">
    ///     Can be set to <c>true</c> to indicate that this handle handled the message and it should not be
    ///     passed on.
    /// </param>
    private void HandleChatMessage(
        XivChatType xivType, uint senderId, ref SeString seSender, ref SeString seMessage, ref bool isHandled)
    {
        var sender = CleanUpSender(seSender);
        var message = CleanUpMessage(seMessage);
        _logManager.LogInfo(xivType, senderId, sender, message);
    }

    /// <summary>
    ///     Cleans up the chat message. The world names are separated from the user names by an at sign (@).
    /// </summary>
    /// <param name="seMessage">The message to clean.</param>
    /// <returns>The cleaned message.</returns>
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

    /// <summary>
    ///     Cleans up the sender name. This removed any non-name characters and separated the world name from the user name by
    ///     an at sign (@).
    /// </summary>
    /// <param name="seSender">The sender name.</param>
    /// <returns>The cleaned sender name.</returns>
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