using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dalamud.Logging;

namespace ChatterPlugin;

/// <summary>
///     Helpers for dealing with the private user area characters that FFXIV uses.
/// </summary>
/// <remarks>
///     The Unicode block from U+E000..U+F8FF is a private user area where applications can install their
///     own characters. FFXIV uses this area for special characters. As such none of these characters
///     will display properly in any situation other than inside of FFXIV.
/// </remarks>
public static class SeSpecialCharacters
{
    /// <summary>
    ///     Provides reasonable replacements for some of the special characters that FFXIV uses into
    ///     well-defined Unicode characters.
    /// </summary>
    private static readonly Dictionary<char, string> SpecialCharacterMap = new()
    { // : 10 - 
        {'\uE03C', "\u2747"},
        {'\uE06D', "\u33C2"},
        {'\uE06E', "\u33D8"},
        {'\uE040', "["},
        {'\uE041', "]"},
        {'\uE05D', "\u273F"},
        {'\uE071', "\u24B6"},
        {'\uE072', "\u24B7"},
        {'\uE073', "\u24B8"},
        {'\uE074', "\u24B9"},
        {'\uE075', "\u24BA"},
        {'\uE076', "\u24BB"},
        {'\uE077', "\u24BC"},
        {'\uE078', "\u24BD"},
        {'\uE079', "\u24BE"},
        {'\uE07A', "\u24BF"},
        {'\uE07B', "\u24C0"},
        {'\uE07C', "\u24C1"},
        {'\uE07D', "\u24C2"},
        {'\uE07E', "\u24C3"},
        {'\uE07F', "\u24C4"},
        {'\uE080', "\u24C5"},
        {'\uE081', "\u24C6"},
        {'\uE082', "\u24C7"},
        {'\uE083', "\u24C8"},
        {'\uE084', "\u24C9"},
        {'\uE085', "\u24CA"},
        {'\uE086', "\u24CB"},
        {'\uE087', "\u24CC"},
        {'\uE088', "\u24CD"},
        {'\uE089', "\u24CE"},
        {'\uE08A', "\u24CF"},
        {'\uE090', "\u2460"},
        {'\uE091', "\u2461"},
        {'\uE092', "\u2462"},
        {'\uE093', "\u2463"},
        {'\uE094', "\u2464"},
        {'\uE095', "\u2465"},
        {'\uE096', "\u2466"},
        {'\uE097', "\u2467"},
        {'\uE098', "\u2468"},
        {'\uE099', "\u2469"},
        {'\uE0BB', "\u02A2"},
    };

    public static bool IsSpecial(char ch)
    {
        return ch is >= '\uE000' and <= '\uF8FF';
    }

    /// <summary>
    ///     Removes all special characters from the given string. Contrast this with <see cref="Replace" />
    ///     which replaces the special characters with suitable alternates.
    /// </summary>
    /// <param name="s">The string to process.</param>
    /// <returns>The processed string.</returns>
    public static string Clean(string s)
    {
        var sb = new StringBuilder();
        foreach (var ch in s.Where(ch => !IsSpecial(ch))) sb.Append(ch);
        return sb.ToString();
    }

    /// <summary>
    ///     Returns the given string with all special characters replaced.
    /// </summary>
    /// <param name="s">The string to process.</param>
    /// <returns>The processed string.</returns>
    public static string Replace(string s)
    {
        var sb = new StringBuilder();
        foreach (var ch in s)
            sb.Append(Replacement(ch));
        return sb.ToString();
    }

    /// <summary>
    ///     Provides a reasonable replacement for one of the FFXIV special characters. All replacements will be
    ///     well-defined Unicode characters. If there is no defined replacement or if the character is not a
    ///     special character, it is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     Because the special characters are, well, special, there may not be a single character replacement that works, so
    ///     the replacement is a string.
    /// </remarks>
    /// <param name="ch">The character to replace.</param>
    /// <returns>The replacement string.</returns>
    public static string Replacement(char ch)
    {
        if (IsSpecial(ch))
        {
            if (SpecialCharacterMap.TryGetValue(ch, out var value))
                return value;
#if DEBUG
            PluginLog.Debug("Unhandled FFXIV character: (\\u{0:X4})", (int) ch);
#endif
        }

        return ch.ToString();
    }
}