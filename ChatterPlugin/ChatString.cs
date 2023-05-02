using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatterPlugin.Model;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Utility;
using static ChatterPlugin.Configuration;

namespace ChatterPlugin;

/// <summary>
///     A string from the chat system.
/// </summary>
/// <remarks>
///     Dalamud pass around lists of objects for string called <see cref="SeString" /> which corresponds to the stream
///     of data passed by FFXIV for stringy objects. These lists allow for non-text items to be placed into the stream
///     to add to the metadata sent e.g. icons, formatting, etc. We use <c>ChatString</c> as a container for the string
///     parts with the behaviors that we need for handle chat messages and users (both of which use the same string
///     structure).
/// </remarks>
public class ChatString
{
    private readonly List<CsItem> _items = new();

    public ChatString(CsItem item)
    {
        _items.Add(item);
    }

    /// <summary>
    ///     Creates a ChatString from an <see cref="SeString" />. The <see cref="SeString" /> is parsed and converted to the
    ///     items that we need for processing chat logs.
    /// </summary>
    /// <param name="seString">The <see cref="SeString" /> to process.</param>
    public ChatString(SeString seString)
    {
        var nameState = NameState.Nothing;
        CsPlayerItem? player = null;

        var payloads = seString.Payloads;
        var count = payloads.Count;
        for (var i = 0; i < count; i++)
        {
            var payload = payloads[i];

            switch (payload)
            {
                case PlayerPayload p:
                    player = new CsPlayerItem(p.PlayerName, p.World.Name);
                    _items.Add(player);
                    nameState = NameState.LookingForName;
                    break;
                case AutoTranslatePayload atp:
                    var atpText = atp.Text;
                    atpText = atpText.Trim();
                    if (!atpText.IsNullOrWhitespace()) _items.Add(new CsTextItem(atpText));
                    nameState = NameState.Nothing;
                    player = null;
                    break;
                case TextPayload text:
                    var str = text.Text ?? string.Empty;
                    switch (nameState)
                    {
                        case NameState.LookingForName:
                        {
                            nameState = NameState.Nothing;
                            if (str == player!.Name)
                            {
                                nameState = NameState.LookingForWorld;
                                continue;
                            }

                            break;
                        }
                        case NameState.LookingForWorld:
                        {
                            nameState = NameState.Nothing;
                            if (str.StartsWith(player!.World)) str = str[player.World.Length..];
                            player = null;
                            break;
                        }
                        case NameState.Nothing:
                            break;
                    }

                    str = str.Trim();
                    if (!str.IsNullOrWhitespace()) _items.Add(new CsTextItem(str));
                    break;
                default:
                    var payType = payload.GetType();
                    break;
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        return sb.AppendJoin(' ', _items.Select(item => item.ToString())).ToString();
    }

    /// <summary>
    ///     Returns <c>true</c> if the first item in the list is a <see cref="CsPlayerItem" />.
    /// </summary>
    /// <returns><c>true</c> if the first item in the list is a <see cref="CsPlayerItem" />.</returns>
    public bool HasInitialPlayer()
    {
        return _items.Count > 0 && _items[0] is CsPlayerItem;
    }

    /// <summary>
    ///     Returns the first item if it is a <see cref="CsPlayerItem" /> or a newly created <see cref="CsPlayerItem" /> from
    ///     the given information.
    /// </summary>
    /// <param name="player">The player name to use if we cannot find a player item.</param>
    /// <param name="world">The optional world, default to the executing player's home world.</param>
    /// <returns>A <see cref="CsPlayerItem" />.</returns>
    public CsPlayerItem GetInitialPlayerItem(string player, string? world)
    {
        if (_items.Count > 0 && _items[0] is CsPlayerItem cpi) return cpi;

        return new CsPlayerItem(player, world ?? Myself.HomeWorld);
    }

    /// <summary>
    ///     Returns the text representation of all of the items concatenated.
    /// </summary>
    /// <param name="config">The <see cref="ChatLogConfiguration" /> that controls the required output.</param>
    /// <returns>The <c>string</c> value of this string.</returns>
    public string AsText(ChatLogConfiguration config)
    {
        var sb = new StringBuilder();
        foreach (var item in _items)
        {
            var theText = item.AsText(config);
            if (sb.Length > 0 && sb[^1] != ' ' && theText[0] != '.' && theText[0] != ',') sb.Append(' ');

            sb.Append(theText);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     The states of looking for a player's name and world in the text stream.
    /// </summary>
    private enum NameState
    {
        Nothing,
        LookingForName,
        LookingForWorld
    }

    /// <summary>
    ///     Base class for all items that appear in a ChatString.
    /// </summary>
    public abstract class CsItem
    {
        /// <summary>
        ///     Returns the text value of this item based on the given configuration.
        /// </summary>
        /// <param name="config">The <see cref="ChatLogConfiguration" /> that controls the required output.</param>
        /// <returns>The <c>string</c> value of this item.</returns>
        public abstract string AsText(ChatLogConfiguration config);
    }

    /// <summary>
    ///     Represents a player's info in the stream which consists of their name and home world.
    /// </summary>
    public class CsPlayerItem : CsItem
    {
        public CsPlayerItem(string name, string world)
        {
            Name = name;
            World = world;
        }

        public string Name { get; }
        public string World { get; }

        public override string ToString()
        {
            return $"{Name}@{World}";
        }

        /// <summary>
        ///     Returns the player's name with their home world optional appended. This is controlled by the configuration.
        /// </summary>
        /// <param name="config">The <see cref="ChatLogConfiguration" /> that controls the required output.</param>
        /// <returns>The <c>string</c> value of this item.</returns>
        public override string AsText(ChatLogConfiguration config)
        {
            return config.IncludeServer ? $"{Name}@{World}" : Name;
        }
    }

    /// <summary>
    ///     Represents a text sequence in the stream.
    /// </summary>
    private class CsTextItem : CsItem
    {
        public CsTextItem(string text)
        {
            Text = text;
        }

        private string Text { get; }

        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        ///     Returns the value of this item as text for appending into a single string. Funky characters (the one in the
        ///     FFXIV plane) are converted to a more friendly form.
        /// </summary>
        /// <param name="config">The <see cref="ChatLogConfiguration" /> that controls the required output.</param>
        /// <returns>The <c>string</c> value of this item.</returns>
        public override string AsText(ChatLogConfiguration config)
        {
            var sb = new StringBuilder();
            foreach (var ch in Text)
                switch (ch)
                {
                    case '\u2665':

                    case '\uE040':
                        sb.Append('[');
                        break;
                    case '\uE041':
                        sb.Append(']');
                        break;
                    default:
#if DEBUG
                        if ( ch is >= '\uE000' and <= '\uE400')
                        {
                            PluginLog.Log($"Unknown FFXIV character: ({(int)ch})");
                        }
#endif
                        sb.Append(ch);
                        break;
                }

            return sb.ToString();
        }
    }
}