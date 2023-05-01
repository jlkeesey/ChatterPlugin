using System.Collections.Generic;
using System.Linq;

namespace ChatterPlugin.Model;

/// <summary>
///     Data center information as of patch 6.38.
/// </summary>
internal class DataCenter
{
    private static SortedSet<string>? _dataCenterList;
    private static SortedSet<string>? _serversList;
    private static readonly SortedSet<string> EmptySet = new();

    /// <summary>
    ///     Contains all of the data centers and their servers.
    /// </summary>
    private static readonly Dictionary<string, SortedSet<string>> DataCenterData = new()
    {
        // TODO replace this with calls to internal sheets when I can figure them out

        // ReSharper disable StringLiteralTypo
        {
            "Aether",
            new SortedSet<string>
            {
                "Adamantoise", "Cactuar", "Faerie", "Gilgamesh", "Jenova", "Midgardsormr", "Sargatanas", "Siren"
            }
        },
        {
            "Chaos",
            new SortedSet<string>
            {
                "Cerberus", "Louisoix", "Moogle", "Omega", "Phantom", "Ragnarok", "Sagittarius", "Spriggan"
            }
        },
        {
            "Crystal",
            new SortedSet<string>
            {
                "Balmung", "Brynhildr", "Coeurl", "Diabolos", "Goblin", "Malboro", "Mateus", "Zalera"
            }
        },
        {
            "Dynamis",
            new SortedSet<string>
            {
                "Halicarnassus", "Maduin", "Marilith", "Seraph"
            }
        },
        {
            "Elemental",
            new SortedSet<string>
            {
                "Aegis", "Atomos", "Carbuncle", "Garuda", "Gungir", "Kujata", "Tonberry", "Typhon"
            }
        },
        {
            "Gaia",
            new SortedSet<string>
            {
                "Alexander", "Bahamut", "Durandal", "Fenrir", "Ifrit", "Ridill", "Tiamat", "Ultima"
            }
        },
        {
            "Light",
            new SortedSet<string>
            {
                "Alpha", "Lich", "Odin", "Phoenix", "Raiden", "Shiva", "Twintania", "Zodiark"
            }
        },
        {
            "Mana",
            new SortedSet<string>
            {
                "Anima", "Asura", "Chocobo", "Hades", "Ixion", "Masamune", "Pandaemonium", "Titan"
            }
        },
        {
            "Materia",
            new SortedSet<string>
            {
                "Bismarck", "Ravana", "Sephirot", "Sophia", "Zurvan"
            }
        },
        {
            "Meteor",
            new SortedSet<string>
            {
                "Belias", "Mandragora", "Ramuh", "Shinryu", "Unicorn", "Valefor", "Yojimbo", "Zeromus"
            }
        },
        {
            "Primal",
            new SortedSet<string>
            {
                "Behemoth", "Excalibur", "Exodus", "Famfrit", "Hyperion", "Lamia", "Leviathan", "Ultros"
            }
        }
        // ReSharper restore StringLiteralTypo
    };

    /// <summary>
    ///     Set of all servers in all data centers.
    /// </summary>
    public static IReadOnlySet<string> Servers
    {
        get { return _serversList ??= new SortedSet<string>(DataCenterData.SelectMany(d => d.Value)); }
    }

    /// <summary>
    ///     Set of all data centers.
    /// </summary>
    public static IReadOnlySet<string> DataCenters
    {
        get { return _dataCenterList ??= new SortedSet<string>(DataCenterData.Keys); }
    }

    /// <summary>
    ///     Returns the Set of all servers in the given data center.
    /// </summary>
    /// <param name="dataCenter">The name of the data center to query.</param>
    /// <returns>The Set of servers or an empty set if the data center is not known.</returns>
    public static IReadOnlySet<string> GetServers(string dataCenter)
    {
        return DataCenterData.GetValueOrDefault(dataCenter, EmptySet);
    }
}