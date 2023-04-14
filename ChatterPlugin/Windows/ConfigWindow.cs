using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ChatterPlugin.Windows;

public sealed class ConfigWindow : Window, IDisposable
{
    private const string title = "Chatter Configuration";
    private bool visible;

    public bool Visible
    {
        get => visible;
        set => visible = value;
    }

    private readonly Configuration configuration;

    public ConfigWindow(Plugin plugin) : base(title)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        configuration = plugin.Configuration;
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
            var filePath = configuration.LogDirectory;
            if (ImGui.InputText("Save directory", ref filePath, 256))
            {
                configuration.LogDirectory = filePath;
                configuration.Save();
            }
        }
        ImGui.End();
    }
}
