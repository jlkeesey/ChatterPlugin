using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using ChatterPlugin.Friends;
using Dalamud.Game.Text;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Utility;
using ImGuiNET;
using static System.String;

// ReSharper disable InvertIf

namespace ChatterPlugin.Windows;

/// <summary>
///     Defines the configuration editing window.
/// </summary>
public sealed class ConfigWindow : Window, IDisposable
{
    private const string Title = "Chatter Configuration";

    /// <summary>
    ///     The list of chat types for the General section.
    /// </summary>
    private static readonly ChatTypeFlagList GeneralChatTypeFlags = new()
    {
        {XivChatType.Say, "Say", "When checked /say (/s) messages are included."},
        {XivChatType.Yell, "Yell", "When checked /yell (/y) messages are included."},
        {XivChatType.Shout, "Shout", "When checked /shout (/sh) messages are included."},
        {XivChatType.TellIncoming, "Tell", "When checked /tell (/t) messages are included."},
        {XivChatType.Party, "Party", "When checked /party (/p) messages are included."},
        {XivChatType.FreeCompany, "FC", "When checked /freecompany (/fc) messages are included."},
        {XivChatType.Alliance, "Alliance", "When checked /alliance (/a) messages are included."},
        {XivChatType.StandardEmote, "Emote", "When checked /emote (/em) messages are included."}
    };

    /// <summary>
    ///     The list of chat types for the Linkshell section.
    /// </summary>
    private static readonly ChatTypeFlagList LinkShellChatTypeFlags = new()
    {
        {XivChatType.Ls1, "Ls1", "When checked /linkshell1 (/ls1) messages are included."},
        {XivChatType.Ls2, "Ls2", "When checked /linkshell2 (/ls2) messages are included."},
        {XivChatType.Ls3, "Ls3", "When checked /linkshell3 (/ls3) messages are included."},
        {XivChatType.Ls4, "Ls4", "When checked /linkshell4 (/ls4) messages are included."},
        {XivChatType.Ls5, "Ls5", "When checked /linkshell5 (/ls5) messages are included."},
        {XivChatType.Ls6, "Ls6", "When checked /linkshell6 (/ls6) messages are included."},
        {XivChatType.Ls7, "Ls7", "When checked /linkshell7 (/ls7) messages are included."},
        {XivChatType.Ls8, "Ls8", "When checked /linkshell8 (/ls8) messages are included."}
    };

    /// <summary>
    ///     The list of chat types for the Cross-World Linkshell section.
    /// </summary>
    private static readonly ChatTypeFlagList CrossWorldLinkShellChatTypeFlags = new()
    {
        // ReSharper disable StringLiteralTypo
        {XivChatType.CrossLinkShell1, "Cwls1", "When checked /cwlinkshell1 (/cwl1) messages are included."},
        {XivChatType.CrossLinkShell2, "Cwls2", "When checked /cwlinkshell2 (/cwl2) messages are included."},
        {XivChatType.CrossLinkShell3, "Cwls3", "When checked /cwlinkshell3 (/cwl3) messages are included."},
        {XivChatType.CrossLinkShell4, "Cwls4", "When checked /cwlinkshell4 (/cwl4) messages are included."},
        {XivChatType.CrossLinkShell5, "Cwls5", "When checked /cwlinkshell5 (/cwl5) messages are included."},
        {XivChatType.CrossLinkShell6, "Cwls6", "When checked /cwlinkshell6 (/cwl6) messages are included."},
        {XivChatType.CrossLinkShell7, "Cwls7", "When checked /cwlinkshell7 (/cwl7) messages are included."},
        {XivChatType.CrossLinkShell8, "Cwls8", "When checked /cwlinkshell8 (/cwl8) messages are included."}
        // ReSharper restore StringLiteralTypo
    };

    /// <summary>
    ///     The list of chat types for the Other section.
    /// </summary>
    private static readonly ChatTypeFlagList OtherChatTypeFlags = new()
    {
        // ReSharper disable StringLiteralTypo
        {XivChatType.Urgent, "Urgent", "When checked include urgent messages sent by Square Enix."},
        {XivChatType.Notice, "Notice", "When checked include notices sent by Square Enix."},
        {XivChatType.NoviceNetwork, "Novice", "When checked include novice network messages."},
        {XivChatType.PvPTeam, "Pvp", "When checked include PVP team messages."},
        {XivChatType.Echo, "Echo", "When checked include /echo messages."},
        {XivChatType.SystemError, "Sys Error", "When checked include system error messages sent by Square Enix."},
        {XivChatType.SystemMessage, "Sys Message", "When checked include system messages sent by Square Enix."}
        // ReSharper restore StringLiteralTypo
    };

    private readonly Configuration _configuration;
    private bool _addUserAlreadyExists;
    private string _addUserFullName = Empty;
    private string _addUserReplacementName = Empty;
    private IEnumerable<Friend> _filteredFriends = new List<Friend>();
    private string _friendFilter = Empty;
    private IEnumerable<Friend> _friends = new List<Friend>();
    private bool _removeDialogIsOpen = true;
    private string _removeDialogUser = Empty;
    private string _removeUserName = Empty;
    private string _selectedFriend = Empty;
    private string _selectedGroup = Configuration.AllLogName;

    /// <summary>
    ///     Constructs the configuration editing window.
    /// </summary>
    public ConfigWindow() : base(Title)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(450, 520),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Size = new Vector2(800, 520);
        SizeCondition = ImGuiCond.FirstUseEver;

        _configuration = Chatter.Configuration;
    }

    public void Dispose()
    {
    }

    /// <summary>
    ///     Draws this window.
    /// </summary>
    public override void Draw()
    {
        if (ImGui.BeginTabBar("tabBar", ImGuiTabBarFlags.None))
        {
            if (ImGui.BeginTabItem("General"))
            {
                DrawGeneralTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Groups"))
            {
                DrawGroupsTab();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }
    }

    /// <summary>
    ///     Draws the general settings tab. This is where all the settings that affect the entire plugin are edited.
    /// </summary>
    private void DrawGeneralTab()
    {
        ImGui.InputText("File name prefix", ref _configuration.LogFileNamePrefix, 50);
        HelpMarker("The prefix for all log files created by Chatter.");

        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();

        LongInputField("Save directory", ref _configuration.LogDirectory, 1024, "##saveDirectory",
            "The directory where all logs are written. If the final part of the directory does not exist it will be created.");

        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();

        ImGui.Separator();
        ImGui.Text($"Size: [{ImGui.GetWindowSize().X}, {ImGui.GetWindowSize().Y}]");
        ImGui.Separator();

        ImGui.Spacing();
        if (ImGui.Button("Save")) _configuration.Save();
    }

    /// <summary>
    ///     Draws the group tab. This allows for editing a single group definition.
    /// </summary>
    private void DrawGroupsTab()
    {
        DrawGroupsList();

        ImGui.SameLine();

        DrawGroupEdit();
    }

    /// <summary>
    ///     Draws the editor for a single group's configuration.
    /// </summary>
    private void DrawGroupEdit()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5.0f);
        ImGui.BeginChild("groupData", new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetContentRegionAvail().Y),
            true);
        var chatLog = _configuration.ChatLogs[_selectedGroup];
        ImGui.TextColored(new Vector4(0.0f, 1.0f, 0.0f, 1.0f), chatLog.Name);
        ImGui.Separator();
        ImGui.Spacing();

        if (ImGui.BeginTable("general", 2))
        {
            ImGui.TableNextColumn();
            DrawCheckbox("Is active", ref chatLog.IsActive,
                "When checked messages that match this group will be written to its log file. " +
                "Uncheck to stop writing out this log. The all log cannot be disabled. Cannot be changed for the all group.",
                chatLog.Name == Configuration.AllLogName);
            ImGui.TableNextColumn();
            DrawCheckbox("Include all users", ref chatLog.IncludeAllUsers,
                "When checked all users will be included in this log even if they are not in the user list. " +
                "When unchecked, only users in the user list will be included in the log. Cannot be changed for the all group.",
                chatLog.Name == Configuration.AllLogName);
            ImGui.TableNextColumn();
            DrawCheckbox("Include server name", ref chatLog.IncludeServer,
                "When checked the server names will be appended to each player's name. " +
                "When unchecked server names will be removed from all names. " +
                "Note: Adding server names to player names in messages may not work reliably for players on the same server as you.");
            ImGui.TableNextColumn();
            DrawCheckbox("Include me", ref chatLog.IncludeMe,
                "When checked you will be included in this log even if you are not in the user list. " +
                "When unchecked, you will only be included in the log if you are included in the user list.");
#if DEBUG
            ImGui.TableNextColumn();
            DrawCheckbox("Include all messages (debug)", ref chatLog.DebugIncludeAllMessages,
                "When checked all messages are included in this log, even ones that are normally ignored. " +
                "This is only for debugging.");
#endif
            ImGui.EndTable();
        }

        ImGui.Spacing();

        if (ImGui.CollapsingHeader("Included Users"))
        {
            VerticalSpace(5.0f);
            ImGui.TextWrapped("These are the users that will be included in this log. You can also enter what you " +
                              "want each user to be renamed to in the log. This is generally used to shorten names to " +
                              "make the log easier to read.");
            VerticalSpace();
            if (ImGui.Button("Add user")) ImGui.OpenPopup("Add User");

            DrawAddUserPopup(chatLog);

            const ImGuiTableFlags tableFlags = ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg |
                                               ImGuiTableFlags.BordersOuter |
                                               ImGuiTableFlags.SizingFixedFit |
                                               ImGuiTableFlags.BordersV;
            var textBaseHeight = ImGui.GetTextLineHeightWithSpacing();
            var outerSize = new Vector2(0.0f, textBaseHeight * 8);
            if (ImGui.BeginTable("userTable", 3, tableFlags, outerSize))
            {
                ImGui.TableSetupScrollFreeze(0, 1); // Make top row always visible
                ImGui.TableSetupColumn("Player full name", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("Replace with", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 22.0f);
                ImGui.TableHeadersRow();

                foreach (var (userFrom, userTo) in chatLog.Users)
                {
                    ImGui.PushID(userFrom);
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextColored(new Vector4(0.6f, 0.7f, 1.0f, 1.0f), userFrom);

                    ImGui.TableSetColumnIndex(1);
                    ImGui.TextColored(new Vector4(0.7f, 0.9f, 0.6f, 1.0f),
                        userTo.IsNullOrWhitespace() ? "-" : userTo);

                    ImGui.TableSetColumnIndex(2);
                    if (DrawUserRemoveButton(userFrom))
                    {
                        if (!chatLog.Users.ContainsKey(userFrom)) return;
                        _removeDialogUser = userFrom;
                        ImGui.OpenPopup("Remove?");
                    }

                    DrawRemoveUserDialog();
                    ImGui.PopID();
                }


                ImGui.EndTable();
                VerticalSpace();
            }
        }

        if (_removeUserName != Empty)
        {
            chatLog.Users.Remove(_removeUserName);
            _removeUserName = Empty;
        }

        if (ImGui.CollapsingHeader("Included Chat Types"))
        {
            DrawChatTypeFlags("flagGeneral", chatLog, GeneralChatTypeFlags);
            DrawChatTypeFlags("flagLs", chatLog, LinkShellChatTypeFlags);
            DrawChatTypeFlags("flagCwls", chatLog, CrossWorldLinkShellChatTypeFlags);
            DrawChatTypeFlags("flagOther", chatLog, OtherChatTypeFlags);
        }

        ImGui.EndChild();
        ImGui.PopStyleVar();
    }

    /// <summary>
    ///     Adds vertical space to the output.
    /// </summary>
    /// <param name="space">The amount of extra space to add.</param>
    private static void VerticalSpace(float space = 3.0f)
    {
        ImGui.Dummy(new Vector2(0.0f, space));
    }

    /// <summary>
    ///     Draws the popup to add a new user to the user list.
    /// </summary>
    /// <param name="chatLog">The chat log configuration to edit.</param>
    private void DrawAddUserPopup(Configuration.ChatLogConfiguration chatLog)
    {
        ImGui.SetNextWindowSizeConstraints(new Vector2(350.0f, 100.0f), new Vector2(350.0f, 200.0f));
        //ImGui.SetNextWindowSize(new Vector2(350.0f, 170.0f));
        if (ImGui.BeginPopup("Add User", ImGuiWindowFlags.ChildWindow))
        {
            if (_addUserAlreadyExists)
            {
                VerticalSpace();
                ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f), "That player is already in the list.");
                VerticalSpace();
            }

            LongInputField("Player full name", ref _addUserFullName, 128, "##playerFullName",
                "The full name of the player including the player's home world separated by an at (@) sign e.g. Lord Farquod@Duloc. " +
                $"If a world is not present it will default to your home world of {Myself.HomeWorld}. " +
                "You can also use the Friend selector button to pick a player from your friend list.",
                extraWidth: 30, extra: () =>
                {
                    if (DrawFindFriendButton())
                    {
                        _friendFilter = Empty;
                        _friends = _filteredFriends = FriendManager.GetFriends();
                        _selectedFriend = Empty;
                        ImGui.OpenPopup("Find Friend");
                    }
                });

            VerticalSpace();
            LongInputField("Player replacement name", ref _addUserReplacementName, 128, "##playerReplaceName",
                "If present, this is the name that will replace the full play name in the log, " +
                "typically used to simplify names of players that are well-known to you.",
                extraWidth: 30);

            VerticalSpace();
            ImGui.Separator();
            VerticalSpace();

            if (ImGui.Button("Add", new Vector2(120, 0)))
            {
                _addUserAlreadyExists = false;
                var fullName = _addUserFullName.Trim();
                if (!fullName.IsNullOrWhitespace())
                {
                    if (!fullName.Contains('@')) fullName = $"{fullName}@{Myself.HomeWorld}";
                    var replacement = _addUserReplacementName.Trim();
                    if (chatLog.Users.TryAdd(fullName, replacement))
                        ImGui.CloseCurrentPopup();
                    else
                        _addUserAlreadyExists = true;
                }
            }

            ImGui.SameLine();
            if (ImGui.Button("Cancel", new Vector2(120, 0)))
            {
                _addUserAlreadyExists = false;
                ImGui.CloseCurrentPopup();
            }

            DrawFindFriendPopup(ref _addUserFullName);

            ImGui.EndPopup();
        }
    }

    /// <summary>
    ///     Draws the popup that lists all the users friends for selection.
    /// </summary>
    /// <param name="targetUserFullName">Where to put the chosen friend name.</param>
    private void DrawFindFriendPopup(ref string targetUserFullName)
    {
        if (ImGui.BeginPopup("Find Friend", ImGuiWindowFlags.AlwaysAutoResize))
        {
            // if (ImGui.InputText("##filter", ref _friendFilter, 100))
            // {
            //     _filteredFriends = _friends
            //         .Where(f => f.Name.Contains(_friendFilter, StringComparison.OrdinalIgnoreCase))
            //         .ToImmutableSortedSet();
            // }
            if (ImGui.InputText("##filter", ref _friendFilter, 100))
            {
                _filteredFriends = _friends
                    .Where(f => f.Name.Contains(_friendFilter, StringComparison.OrdinalIgnoreCase))
                    .ToImmutableSortedSet();
            }
            ImGui.SameLine();
            if (DrawClearFilterButton())
            {
                _friendFilter = Empty;
            }

            var textBaseHeight = ImGui.GetTextLineHeightWithSpacing();
            var outerSize = new Vector2(-1.0f, textBaseHeight * 8);
            if (ImGui.BeginListBox("##friendList", outerSize))
            {
                foreach (var filteredFriend in _filteredFriends)
                    if (ImGui.Selectable(filteredFriend.FullName, filteredFriend.FullName == _selectedFriend))
                        _selectedFriend = filteredFriend.FullName;

                ImGui.EndListBox();
            }

            ImGui.Separator();
            if (ImGui.Button("Add", new Vector2(120, 0)))
            {
                if (!_selectedFriend.IsNullOrWhitespace())
                    targetUserFullName = _selectedFriend;
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            if (ImGui.Button("Cancel", new Vector2(120, 0))) ImGui.CloseCurrentPopup();

            ImGui.EndPopup();
        }
    }

    /// <summary>
    ///     Draws the button that brings up the friend selection dialog.
    /// </summary>
    /// <returns>True if the button was pressed.</returns>
    private static bool DrawClearFilterButton()
    {
        ImGui.PushFont(UiBuilder.IconFont);
        var buttonPressed = ImGui.Button($"{(char) FontAwesomeIcon.SquareXmark}##findFriend");
        ImGui.PopFont();
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled)) DrawTooltip("Clears the filter.");
        return buttonPressed;
    }

    /// <summary>
    ///     Draws the button that brings up the friend selection dialog.
    /// </summary>
    /// <returns>True if the button was pressed.</returns>
    private static bool DrawFindFriendButton()
    {
        ImGui.PushFont(UiBuilder.IconFont);
        var buttonPressed = ImGui.Button($"{(char) FontAwesomeIcon.PersonCirclePlus}##findFriend");
        ImGui.PopFont();
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled)) DrawTooltip("Brings up the Friend selector.");
        return buttonPressed;
    }

    /// <summary>
    ///     Draws the button that brings up the remove user dialog.
    /// </summary>
    /// <param name="user">Which user is being removed.</param>
    /// <returns>True if the button was pressed.</returns>
    private static bool DrawUserRemoveButton(string user)
    {
        ImGui.PushFont(UiBuilder.IconFont);
        var buttonPressed = ImGui.Button($"{(char) FontAwesomeIcon.Trash}##{user}Trash");
        ImGui.PopFont();
        return buttonPressed;
    }

    /// <summary>
    ///     Draws the remove user dialog.
    /// </summary>
    private void DrawRemoveUserDialog()
    {
        // Always center this window when appearing
        // var center = ImGui.GetMainViewport().GetCenter();
        // ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
        if (ImGui.BeginPopupModal("Remove?", ref _removeDialogIsOpen, ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.TextWrapped($"Do you want to remove {_removeDialogUser} from this log?");
            ImGui.Separator();

            if (ImGui.Button("Remove", new Vector2(120, 0)))
            {
                _removeUserName = _removeDialogUser;
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            if (ImGui.Button("Cancel", new Vector2(120, 0))) ImGui.CloseCurrentPopup();


            ImGui.EndPopup();
        }
    }

    /// <summary>
    ///     Draws a check box control with the optional help text.
    /// </summary>
    /// <param name="label">The label for the checkbox.</param>
    /// <param name="itemChecked">True if this check box is checked.</param>
    /// <param name="helpText">The optional help text.</param>
    /// <param name="disabled">True if this control should be disabled.</param>
    private static void DrawCheckbox(string label, ref bool itemChecked, string? helpText = null, bool disabled = false)
    {
        if (disabled) ImGui.BeginDisabled();
        ImGui.Checkbox(label, ref itemChecked);
        if (disabled) ImGui.EndDisabled();
        HelpMarker(helpText);
    }

    /// <summary>
    ///     Draws the list of groups for selecting into the editor.
    /// </summary>
    private void DrawGroupsList()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5.0f);
        ImGui.BeginChild("groupsChild",
            new Vector2(ImGui.GetContentRegionAvail().X * 0.25f, ImGui.GetContentRegionAvail().Y), true);
        if (ImGui.BeginListBox("##groups",
                new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetContentRegionAvail().Y)))
        {
            foreach (var (_, cl) in _configuration.ChatLogs)
            {
                var isSelected = _selectedGroup == cl.Name;
                if (ImGui.Selectable(cl.Name, isSelected)) _selectedGroup = cl.Name;
                if (ImGui.IsItemHovered()) DrawTooltip(cl.Name);
                if (isSelected) ImGui.SetItemDefaultFocus();
            }

            ImGui.EndListBox();
        }

        ImGui.EndChild();
        ImGui.PopStyleVar();
    }

    /// <summary>
    ///     Draws all of the chat flags in a given list.
    /// </summary>
    /// <param name="id">The id for the list of items.</param>
    /// <param name="chatLog">The chat log configuration.</param>
    /// <param name="flagList">The list of flags to draw. If a flag is in the list but not set in the config it is ignored.</param>
    private static void DrawChatTypeFlags(string id, Configuration.ChatLogConfiguration chatLog,
        ChatTypeFlagList flagList)
    {
        ImGui.Spacing();
        var flags = chatLog.ChatTypeFilterFlags;
        if (ImGui.BeginTable(id, 4))
        {
            foreach (var flag in flagList)
                if (flags.TryGetValue(flag.Item1, out var flagValue))
                    DrawFlag(flag.Item2, ref flagValue.Value, flag.Item3);

            ImGui.EndTable();
        }

        ImGui.Spacing();
    }

    /// <summary>
    ///     Draws a single chat flag item.
    /// </summary>
    /// <param name="label">The label for the flag.</param>
    /// <param name="flag">The flag value location.</param>
    /// <param name="helpText">optional help description for this chat type.</param>
    private static void DrawFlag(string label, ref bool flag, string? helpText = null)
    {
        ImGui.TableNextColumn();
        ImGui.Checkbox(label, ref flag);
        HelpMarker(helpText);
    }

    /// <summary>
    ///     Creates an input field for a long value such that the label is not on the same line as the input field.
    /// </summary>
    /// <param name="label">The text label for this field.</param>
    /// <param name="value">The field value. This must be a ref value.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <param name="id">The optional id for the field. This is only necessary if the label is not unique.</param>
    /// <param name="help">The optional help text displayed when hovering over the help button.</param>
    /// <param name="extra">Function to add extra parts to the end of the widget.</param>
    /// <param name="extraWidth">The width of the extra element(s).</param>
    private static bool LongInputField(string label, ref string value, uint maxLength = 100, string? id = null,
        string? help = null, Action? extra = null, int extraWidth = 0)
    {
        ImGui.Text(label);
        HelpMarker(help);

        ImGui.SetNextItemWidth(extraWidth == 0 ? -1 : -extraWidth);
        var result = ImGui.InputText(id ?? label, ref value, maxLength);
        if (extra != null)
        {
            ImGui.SameLine();
            extra();
        }

        return result;
    }


    /// <summary>
    ///     Adds a help button that shows the given help text when hovered over.
    /// </summary>
    /// <param name="description">The description to show. If this is null, empty, or all whitespace, nothing is created.</param>
    /// <param name="sameLine">True if this should be on the same line as the previous item.</param>
    private static void HelpMarker(string? description, bool sameLine = true)
    {
        var text = description?.Trim() ?? Empty;
        if (text.IsNullOrEmpty()) return;
        if (sameLine) ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.4f, 0.4f, 0.55f, 1.0f));
        ImGui.PushFont(UiBuilder.IconFont);
        ImGui.Text($"{(char) FontAwesomeIcon.QuestionCircle}");
        ImGui.PopFont();
        ImGui.PopStyleColor();
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled)) DrawTooltip(text);
    }

    /// <summary>
    ///     Creates a tooltip box with the given content text which will be wrapped as necessary.
    /// </summary>
    /// <param name="description">The contents of the tooltip box. If null or empty the tooltip box is not created.</param>
    private static void DrawTooltip(string? description)
    {
        var text = description?.Trim() ?? Empty;
        if (text.IsNullOrEmpty()) return;
        ImGui.BeginTooltip();
        ImGui.PushTextWrapPos(ImGui.GetFontSize() * 20.0f);
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.9f, 0.9f, 0.3f, 1.0f));
        ImGui.TextUnformatted(text);
        ImGui.PopStyleColor();
        ImGui.PopTextWrapPos();
        ImGui.EndTooltip();
    }

    /// <summary>
    ///     Helper type to make creating a list of tuples easier.
    /// </summary>
    private class ChatTypeFlagList : List<Tuple<XivChatType, string, string?>>
    {
        public void Add(XivChatType item, string item2, string? item3 = null)
        {
            Add(new Tuple<XivChatType, string, string?>(item, item2, item3));
        }
    }
}