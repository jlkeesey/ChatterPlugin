using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ChatterPlugin.Windows;

public sealed class MainWindow : Window, IDisposable
{
    private const string Title = "Chatter";

    private bool visible;

    public bool Visible
    {
        get => visible;
        set => visible = value;
    }

    private readonly Chatter chatter;

    public MainWindow(Chatter chatter) : base(
        Title, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.chatter = chatter;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (!Visible)
        {
            return;
        }

        if (ImGui.Begin(Title, ref visible))
        {
            ImGui.Text($"The config path is '{Chatter.Configuration.LogDirectory}' ");

            ImGui.Spacing();

            ImGui.Text("Have a goat:");
            ImGui.Indent(55);
            ImGui.Unindent(55);
        }

        ImGui.End();
    }
}
