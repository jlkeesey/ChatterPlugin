using ChatterPlugin.Data;

namespace ChatterPlugin.Friends;

/// <summary>
///     Represents a single friend.
/// </summary>
public class Friend
{
    /// <summary>
    ///     This friend's content id.
    /// </summary>
    public readonly ulong ContentId;

    /// <summary>
    ///     This friend's name.
    /// </summary>
    public readonly string Name;

    /// <summary>
    ///     This friend's free company tag.
    /// </summary>
    public readonly string FreeCompany;

    /// <summary>
    ///     This friend's home world.
    /// </summary>
    public readonly World HomeWorld;

    /// <summary>
    ///     The world this friend is currently on.
    /// </summary>
    public readonly World CurrentWorld;

    /// <summary>
    ///     True if this friend is online.
    /// </summary>
    public readonly bool IsOnline;

    public Friend(ulong contentId, string name, string freeCompany, World homeWorld, World currentWorld,
        bool isOnline)
    {
        ContentId = contentId;
        Name = name;
        FreeCompany = freeCompany;
        HomeWorld = homeWorld;
        CurrentWorld = currentWorld;
        IsOnline = isOnline;
    }
}