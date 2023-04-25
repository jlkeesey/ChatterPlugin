using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ChatterPlugin.Data;
using Dalamud.Logging;
using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace ChatterPlugin.Friends;

/// <summary>
///     Utilities for manipulating friends.
/// </summary>
public static class FriendManager
{
    private const int InfoOffset = 0x28;
    private const int LengthOffset = 0x10;
    private const int ListOffset = 0x98;

    public static unsafe IReadOnlyList<Friend> GetFriends()
    {
        List<Friend> friends = new();

        var socialFriendAgent = (IntPtr) Framework.Instance()->
                    GetUiModule()->
                GetAgentModule()->
            GetAgentByInternalId(AgentId.SocialFriendList);
        if (socialFriendAgent == IntPtr.Zero) return friends;

        var info = *(IntPtr*) (socialFriendAgent + InfoOffset);
        if (info == IntPtr.Zero) return friends;

        var length = *(ushort*) (info + LengthOffset);
        if (length == 0) return friends;

        var list = *(IntPtr*) (info + ListOffset);
        if (list == IntPtr.Zero) return friends;

        for (var i = 0; i < length; i++)
        {
            var entry = (FriendEntry*) (list + i * FriendEntry.Size);
            var homeWorld = WorldManager.GetWorld(entry->HomeWorld);
            var currentWorld = WorldManager.GetWorld(entry->CurrentWorld);
            PluginLog.Log($"@@@@ {entry->Name}: w:'{homeWorld.Name}' cur:'{currentWorld.Name}' ");
            friends.Add(new Friend(entry->ContentId, entry->Name, entry->FreeCompany, homeWorld, currentWorld,
                entry->IsOnline));
        }

        return friends;
    }

    [StructLayout(LayoutKind.Explicit, Size = Size)]
    private unsafe struct FriendEntry
    {
        internal const int Size = 96;

        /// <summary>
        ///     The friend's content id.
        /// </summary>
        [FieldOffset(0)] public readonly ulong ContentId;

        /// <summary>
        ///     The friend's online status;
        /// </summary>
        [FieldOffset(13)] private readonly byte OnlineStatus;

        /// <summary>
        ///     The world the friend is currently on.
        /// </summary>
        [FieldOffset(22)] public readonly ushort CurrentWorld;

        /// <summary>
        ///     The friend's home world.
        /// </summary>
        [FieldOffset(24)] public readonly ushort HomeWorld;

        // /// <summary>
        // ///     The friend's current job.
        // /// </summary>
        // [FieldOffset(33)] public readonly byte Job;

        /// <summary>
        ///     The friend's raw SeString name.
        /// </summary>
        [FieldOffset(34)] private fixed byte RawName[32];

        /// <summary>
        ///     The friend's raw SeString free company tag.
        /// </summary>
        [FieldOffset(66)] private fixed byte RawFreeCompany[5];

        /// <summary>
        ///     The friend's name.
        /// </summary>
        public string Name
        {
            get
            {
                fixed (byte* ptr = RawName)
                {
                    return MemoryHelper.ReadSeStringNullTerminated((IntPtr) ptr).TextValue;
                }
            }
        }

        /// <summary>
        ///     The friend's free company tag.
        /// </summary>
        public string FreeCompany
        {
            get
            {
                fixed (byte* ptr = RawFreeCompany)
                {
                    return MemoryHelper.ReadSeStringNullTerminated((IntPtr) ptr).TextValue;
                }
            }
        }

        public bool IsOnline => OnlineStatus == 0x80;
    }
}