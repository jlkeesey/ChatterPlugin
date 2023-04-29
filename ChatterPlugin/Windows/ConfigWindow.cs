using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using ChatterPlugin.Friends;
using ChatterPlugin.Localization;
using Dalamud.Game.Text;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
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
        {XivChatType.Say, Loc.Message("ChatType.Say"), Loc.Message("ChatType.Say.Help")},
        {XivChatType.Yell, Loc.Message("ChatType.Yell"), Loc.Message("ChatType.Yell.Help")},
        {XivChatType.Shout, Loc.Message("ChatType.Shout"), Loc.Message("ChatType.Shout.Help")},
        {XivChatType.TellIncoming, Loc.Message("ChatType.Tell"), Loc.Message("ChatType.Tell.Help")},
        {XivChatType.Party, Loc.Message("ChatType.Party"), Loc.Message("ChatType.Party.Help")},
        {XivChatType.FreeCompany, Loc.Message("ChatType.FreeCompany"), Loc.Message("ChatType.FreeCompany.Help")},
        {XivChatType.Alliance, Loc.Message("ChatType.Alliance"), Loc.Message("ChatType.Alliance.Help")},
        {XivChatType.StandardEmote, Loc.Message("ChatType.Emote"), Loc.Message("ChatType.Emote.Help")}
    };

    /// <summary>
    ///     The list of chat types for the Linkshell section.
    /// </summary>
    private static readonly ChatTypeFlagList LinkShellChatTypeFlags = new()
    {
        {XivChatType.Ls1, Loc.Message("ChatType.Ls1"), Loc.Message("ChatType.Ls1.Help")},
        {XivChatType.Ls2, Loc.Message("ChatType.Ls2"), Loc.Message("ChatType.Ls2.Help")},
        {XivChatType.Ls3, Loc.Message("ChatType.Ls3"), Loc.Message("ChatType.Ls3.Help")},
        {XivChatType.Ls4, Loc.Message("ChatType.Ls4"), Loc.Message("ChatType.Ls4.Help")},
        {XivChatType.Ls5, Loc.Message("ChatType.Ls5"), Loc.Message("ChatType.Ls5.Help")},
        {XivChatType.Ls6, Loc.Message("ChatType.Ls6"), Loc.Message("ChatType.Ls6.Help")},
        {XivChatType.Ls7, Loc.Message("ChatType.Ls7"), Loc.Message("ChatType.Ls7.Help")},
        {XivChatType.Ls8, Loc.Message("ChatType.Ls8"), Loc.Message("ChatType.Ls8.Help")}
    };

    /// <summary>
    ///     The list of chat types for the Cross-World Linkshell section.
    /// </summary>
    private static readonly ChatTypeFlagList CrossWorldLinkShellChatTypeFlags = new()
    {
        {XivChatType.CrossLinkShell1, Loc.Message("ChatType.Cwls1"), Loc.Message("ChatType.Cwls1.Help")},
        {XivChatType.CrossLinkShell2, Loc.Message("ChatType.Cwls2"), Loc.Message("ChatType.Cwls2.Help")},
        {XivChatType.CrossLinkShell3, Loc.Message("ChatType.Cwls3"), Loc.Message("ChatType.Cwls3.Help")},
        {XivChatType.CrossLinkShell4, Loc.Message("ChatType.Cwls4"), Loc.Message("ChatType.Cwls4.Help")},
        {XivChatType.CrossLinkShell5, Loc.Message("ChatType.Cwls5"), Loc.Message("ChatType.Cwls5.Help")},
        {XivChatType.CrossLinkShell6, Loc.Message("ChatType.Cwls6"), Loc.Message("ChatType.Cwls6.Help")},
        {XivChatType.CrossLinkShell7, Loc.Message("ChatType.Cwls7"), Loc.Message("ChatType.Cwls7.Help")},
        {XivChatType.CrossLinkShell8, Loc.Message("ChatType.Cwls8"), Loc.Message("ChatType.Cwls8.Help")}
    };

    /// <summary>
    ///     The list of chat types for the Other section.
    /// </summary>
    private static readonly ChatTypeFlagList OtherChatTypeFlags = new()
    {
        {XivChatType.Urgent, Loc.Message("ChatType.Urgent"), Loc.Message("ChatType.Urgent.Help")},
        {XivChatType.Notice, Loc.Message("ChatType.Notice"), Loc.Message("ChatType.Notice.Help")},
        {XivChatType.NoviceNetwork, Loc.Message("ChatType.NoviceNetwork"), Loc.Message("ChatType.NoviceNetwork.Help")},
        {XivChatType.PvPTeam, Loc.Message("ChatType.PvPTeam"), Loc.Message("ChatType.PvPTeam.Help")},
        {XivChatType.Echo, Loc.Message("ChatType.Echo"), Loc.Message("ChatType.Echo.Help")},
        {XivChatType.SystemError, Loc.Message("ChatType.SystemError"), Loc.Message("ChatType.SystemError.Help")},
        {XivChatType.SystemMessage, Loc.Message("ChatType.SystemMessage"), Loc.Message("ChatType.SystemMessage.Help")}
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
            if (ImGui.BeginTabItem(Loc.Message("Tab.General")))
            {
                DrawGeneralTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem(Loc.Message("Tab.Groups")))
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
        LongInputField(Loc.Message("Label.FileNamePrefix"), ref _configuration.LogFileNamePrefix, 50,
            "##fileNamePrefix",
            Loc.Message("Label.FileNamePrefix.Help"));

        VerticalSpace();

        LongInputField(Loc.Message("Label.SaveDirectory"), ref _configuration.LogDirectory, 1024, "##saveDirectory",
            Loc.Message("Label.SaveDirectory.Help"));
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
            DrawCheckbox(Loc.Message("Label.IsActive.Checkbox"), ref chatLog.IsActive,
                Loc.Message("Label.IsActive.Checkbox.Help"), chatLog.Name == Configuration.AllLogName);
            ImGui.TableNextColumn();
            DrawCheckbox(Loc.Message("Label.IncludeAllUsers.Checkbox"), ref chatLog.IncludeAllUsers,
                Loc.Message("Label.IncludeAllUsers.Checkbox.Help"), chatLog.Name == Configuration.AllLogName);
            ImGui.TableNextColumn();
            DrawCheckbox(Loc.Message("Label.IncludeServerName.Checkbox"), ref chatLog.IncludeServer,
                Loc.Message("Label.IncludeServerName.Checkbox.Help"));
            ImGui.TableNextColumn();
            DrawCheckbox(Loc.Message("Label.IncludeSelf.Checkbox"), ref chatLog.IncludeMe,
                Loc.Message("Label.IncludeSelf.Checkbox.Help"));
#if DEBUG
            ImGui.TableNextColumn();
            DrawCheckbox(Loc.Message("Label.IncludeAll.Checkbox"), ref chatLog.DebugIncludeAllMessages,
                Loc.Message("Label.IncludeAll.Checkbox.Help"));
#endif
            ImGui.EndTable();
        }

        ImGui.Spacing();

        if (ImGui.CollapsingHeader(Loc.Message("Header.IncludedUsers")))
        {
            VerticalSpace(5.0f);
            ImGui.TextWrapped(Loc.Message("Description.IncludedUsers"));
            VerticalSpace();
            if (ImGui.Button(Loc.Message("Button.AddUser"))) ImGui.OpenPopup("addUser");

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
                ImGui.TableSetupColumn(Loc.Message("ColumnHeader.FullName"), ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn(Loc.Message("ColumnHeader.Replacement"), ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn(Empty, ImGuiTableColumnFlags.WidthFixed, 22.0f);
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
                        ImGui.OpenPopup("removeUser");
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

        if (ImGui.CollapsingHeader(Loc.Message("Header.IncludedChatTypes")))
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
        if (ImGui.BeginPopup("addUser", ImGuiWindowFlags.ChildWindow))
        {
            if (_addUserAlreadyExists)
            {
                VerticalSpace();
                ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f), Loc.Message("Message.PlayerAlreadyInList"));
                VerticalSpace();
            }

            LongInputField(Loc.Message("Label.PlayerFullName"), ref _addUserFullName, 128, "##playerFullName",
                Loc.Message("Label.PlayerFullName.Help", Myself.HomeWorld),
                extraWidth: 30, extra: () =>
                {
                    if (DrawFindFriendButton())
                    {
                        _friendFilter = Empty;
                        _friends = _filteredFriends = FriendManager.GetFriends();
                        _selectedFriend = Empty;
                        ImGui.OpenPopup("findFriend");
                    }
                });

            VerticalSpace();
            LongInputField(Loc.Message("Label.PlayerReplacement"), ref _addUserReplacementName, 128,
                "##playerReplaceName",
                Loc.Message("Label.PlayerReplacement.Help"),
                extraWidth: 30);

            VerticalSpace();
            ImGui.Separator();
            VerticalSpace();

            if (ImGui.Button(Loc.Message("Button.Add"), new Vector2(120, 0)))
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
            if (ImGui.Button(Loc.Message("Button.Cancel"), new Vector2(120, 0)))
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
        if (ImGui.BeginPopup("findFriend", ImGuiWindowFlags.AlwaysAutoResize))
        {
            // if (ImGui.InputText("##filter", ref _friendFilter, 100))
            // {
            //     _filteredFriends = _friends
            //         .Where(f => f.Name.Contains(_friendFilter, StringComparison.OrdinalIgnoreCase))
            //         .ToImmutableSortedSet();
            // }
            if (ImGui.InputText("##filter", ref _friendFilter, 100))
                _filteredFriends = _friends
                    .Where(f => f.Name.Contains(_friendFilter, StringComparison.OrdinalIgnoreCase))
                    .ToImmutableSortedSet();
            ImGui.SameLine();
            if (DrawClearFilterButton()) _friendFilter = Empty;

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
            if (ImGui.Button(Loc.Message("Button.Add"), new Vector2(120, 0)))
            {
                if (!_selectedFriend.IsNullOrWhitespace())
                    targetUserFullName = _selectedFriend;
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            if (ImGui.Button(Loc.Message("Button.Cancel"), new Vector2(120, 0))) ImGui.CloseCurrentPopup();

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
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
            DrawTooltip(Loc.Message("Button.ClearFilter.Help"));
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
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
            DrawTooltip(Loc.Message("Button.FriendSelector.Help"));
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
        if (ImGui.BeginPopupModal("removeUser", ref _removeDialogIsOpen, ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.TextWrapped(Loc.Message("Button.FriendSelector.Help", _removeDialogUser));
            ImGui.Separator();

            if (ImGui.Button(Loc.Message("Button.Remove"), new Vector2(120, 0)))
            {
                _removeUserName = _removeDialogUser;
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            if (ImGui.Button(Loc.Message("Button.Cancel"), new Vector2(120, 0))) ImGui.CloseCurrentPopup();


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
    private static void LongInputField(string label, ref string value, uint maxLength = 100, string? id = null,
        string? help = null, Action? extra = null, int extraWidth = 0)
    {
        ImGui.Text(label);
        HelpMarker(help);

        ImGui.SetNextItemWidth(extraWidth == 0 ? -1 : -extraWidth);
        ImGui.InputText(id ?? label, ref value, maxLength);
        if (extra != null)
        {
            ImGui.SameLine();
            extra();
        }
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