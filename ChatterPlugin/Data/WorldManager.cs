using System.Collections.Generic;

namespace ChatterPlugin.Data;

/// <summary>
///     Utilities for manipulating World objects.
/// </summary>
public class WorldManager
{
    private static readonly Dictionary<uint, World> Worlds = new();
    // private static readonly World DefaultWorld = new World(0, "?world?", "?dc?");

    /// <summary>
    ///     Retrieve the world object from the given id.
    /// </summary>
    /// <param name="id">The id to lookup.</param>
    /// <returns>The found world. This will always return an object, even if the data cannot be found.</returns>
    public static World GetWorld(uint id)
    {
        if (Worlds.TryGetValue(id, out var world)) return world;
        var w = Dalamud.GameData.Excel.GetSheet<Lumina.Excel.GeneratedSheets.World>()?.GetRow(id);
        world = new World(id, w?.Name.ToString() ?? "?world?", w?.DataCenter?.Value?.Name ?? "?dc?");
        Worlds.Add(id, world);
        return world;
    }
    //
    // /// <summary>
    // /// Returns the 
    // /// </summary>
    // /// <param name="name"></param>
    // /// <returns></returns>
    // public static World GetWorld(string name)
    // {
    //     return Worlds.Select(w => w.Value).Where(w => w.Name == name).FirstOrDefault(DefaultWorld);
    // }
}