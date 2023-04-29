using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Dalamud.Logging;
using Dalamud.Utility;

namespace ChatterPlugin.Localization;

/// <summary>
///     Handles basic message localization.
/// </summary>
public class Loc
{
    private static LocalizedMessageList _messages = new();

    private static readonly JsonSerializerOptions SerializeOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    ///     Loads the localized messages for the current Culture and Region.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The are up to 3 messages files that are read. The first is messages.json which contains the fallback
    ///         for all messages (it is in en-US as that is where the author is from). The second is the language specific
    ///         file which is based on the current CultureInfo. The name of the file is messages-LL.json where LL is
    ///         the two letter language code e.g. en for English. The third is the culture specific file which is
    ///         based on the current CultureInfo and RegionInfo. The name of the file is messages-LL-CC.json where
    ///         LL is the two letter language code as above and CC is the two letter country code e.g. US for United States.
    ///     </para>
    /// </remarks>
    public static void Load()
    {
        _messages = LoadMessageList(string.Empty) ?? new LocalizedMessageList();
        var shortLanguage = GetLanguageTagShort();
        var languageMessages = LoadMessageList(shortLanguage);
        if (languageMessages != null) _messages.Merge(languageMessages);

        var regionalLanguage = GetLanguageTagLong();
        var regionalLanguageMessages = LoadMessageList(regionalLanguage);
        if (regionalLanguageMessages != null) _messages.Merge(regionalLanguageMessages);
    }

    /// <summary>
    ///     Returns the short language tag e.g. en for English.
    /// </summary>
    /// <returns>The short language code.</returns>
    private static string GetLanguageTagShort()
    {
        return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    }

    /// <summary>
    ///     Gets the current full IETF language tag, e.g. en_US for U.S. English.
    /// </summary>
    /// <returns>The full language tag.</returns>
    private static string GetLanguageTagLong()
    {
        var language = GetLanguageTagShort();
        var country = RegionInfo.CurrentRegion.TwoLetterISORegionName;
        return $"{language}-{country}";
    }

    /// <summary>
    ///     Loads ans parses a message list JSON file into a LocalizedMessageList.
    /// </summary>
    /// <param name="suffix">The suffix for the file, maybe empty.</param>
    /// <returns>The loaded LocalizedMessageList or null if the file was not found.</returns>
    private static LocalizedMessageList? LoadMessageList(string suffix)
    {
        var fileName = suffix.IsNullOrWhitespace() ? "messages.json" : $"messages-{suffix}.json";
        var messageFilePath = Path.Combine(Dalamud.PluginInterface.AssemblyLocation.Directory?.FullName!,
            "Localization", fileName);
        if (!File.Exists(messageFilePath))
        {
            return null;
        }

        var fileContents = ReadFile(messageFilePath);
        return ParseList(messageFilePath, fileContents);
    }

    /// <summary>
    ///     Parses a JSON string into a LocalizedMessageList.
    /// </summary>
    /// <param name="fileName">The file name that the JSON string was read from.</param>
    /// <param name="json">The JSON string to parse.</param>
    /// <returns>The parsed LocalizedMessageList or an empty LocalizedMessageList if the string could not be parsed.</returns>
    private static LocalizedMessageList ParseList(string fileName, string json)
    {
        LocalizedMessageList? lml = null;
        try
        {
            lml = JsonSerializer.Deserialize<LocalizedMessageList>(json, SerializeOptions);
        }
        catch (Exception ex)
        {
            PluginLog.Error(ex, $"Cannot parse JSON message file: '{fileName}'");
        }

        return lml ?? new LocalizedMessageList();
    }

    /// <summary>
    ///     Reads the contents of a JSON file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>Returns the file contents or string.Empty of the file could not be read.</returns>
    private static string ReadFile(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (IOException ex)
        {
            PluginLog.Error(ex, $"Cannot read message file: '{filePath}'");
            return string.Empty;
        }
    }

    /// <summary>
    ///     Looks up the message by key from the language files and returns it formatted with the given arguments.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The lookup for a message works by looking the most specific set first, then the less specific, then the
    ///         fallback set. So for a system set to en_US, the search is messages-en-US.json, then messages-en.json, then
    ///         messages.json. If no message is found then a default message constructed from the key is returned.
    ///     </para>
    /// </remarks>
    /// <param name="key">The key to lookup.</param>
    /// <param name="args">The optional arguments for formatting the string.</param>
    /// <returns>The formatter message string.</returns>
    public static string Message(string key, params object[] args)
    {
        if (!_messages.TryGetValue(key, out var message)) message = $"??[[{key}]]??";

        return string.Format(message, args);
    }
}