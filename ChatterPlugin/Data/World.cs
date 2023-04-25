namespace ChatterPlugin.Data;

/// <summary>
///     Represents an FFXIV world.
/// </summary>
public class World
{
    /// <summary>
    ///     This world's data center.
    /// </summary>
    public readonly string DataCenter;

    /// <summary>
    ///     This world's content id.
    /// </summary>
    public readonly uint Id;

    /// <summary>
    ///     This world's name.
    /// </summary>
    public readonly string Name;

    public World(uint id, string name, string dataCenter)
    {
        Id = id;
        Name = name;
        DataCenter = dataCenter;
    }
}