using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ChatterPlugin.Windows;

public sealed class MainWindow : Window, IDisposable
{
    private const string title = "Chatter";

    private bool visible;

    public bool Visible
    {
        get => visible;
        set => visible = value;
    }

    private readonly Plugin plugin;

    public MainWindow(Plugin plugin) : base(
        title, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (!Visible)
        {
            return;
        }

        if (ImGui.Begin(title, ref visible))
        {
            ImGui.Text($"The config path is '{plugin.Configuration.LogDirectory}' ");

            ImGui.Spacing();

            ImGui.Text("Have a goat:");
            ImGui.Indent(55);
            ImGui.Unindent(55);
        }

        ImGui.End();
    }
}
