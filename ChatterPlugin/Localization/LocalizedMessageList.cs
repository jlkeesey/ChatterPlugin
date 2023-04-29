using System;
using System.Collections.Generic;
using Dalamud.Logging;

namespace ChatterPlugin.Localization;

internal class LocalizedMessageList
{
    private readonly IDictionary<string, LocalizedMessage> _dictionary = new Dictionary<string, LocalizedMessage>();

    public LocalizedMessageList() : this(new List<LocalizedMessage>())
    {
    }

    public LocalizedMessageList(ICollection<LocalizedMessage> messages)
    {
        Messages = messages;
    }

    /// <summary>
    /// Merges another LocalizedMessageList into this one overwriting and values with the same key.
    /// </summary>
    /// <param name="other">The LocalizedMessageList to merge into this one.</param>
    public void Merge(LocalizedMessageList other)
    {
        foreach (var localizedMessage in other._dictionary.Values)
        {
            _dictionary[localizedMessage.Key] = localizedMessage;
        }
    }

    public ICollection<LocalizedMessage> Messages
    {
        get => _dictionary.Values;
        set
        {
            _dictionary.Clear();
            foreach (var localizedMessage in value)
            {
                _dictionary.Add(localizedMessage.Key, localizedMessage);
            }
        }
    }

    public bool TryGetValue(string key, out string message)
    {
        if (_dictionary.Count == 0)
            foreach (var localizedMessage in Messages)
                _dictionary.Add(localizedMessage.Key, localizedMessage);

        if (_dictionary.TryGetValue(key, out var locMessage))
        {
            message = locMessage.Message;
            return true;
        }

        message = string.Empty;
        return false;
    }
}