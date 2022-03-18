using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ChillFrames
{
    internal class SettingsWindow : Window, IDisposable
    {
        private readonly SaveAndCloseButtons saveAndCloseButtons;

        public const ImGuiWindowFlags DefaultFlags = ImGuiWindowFlags.NoFocusOnAppearing |
                                                     ImGuiWindowFlags.NoScrollbar |
                                                     ImGuiWindowFlags.NoCollapse |
                                                     ImGuiWindowFlags.NoResize;

        private int newFramerateLimit = Service.Configuration.FrameRateLimit;
        private int TargetFramerate => Service.Configuration.FrameRateLimit;
        private float TargetFrametime => 1000.0f / TargetFramerate;

        public SettingsWindow() : base("ChillFrames Settings", DefaultFlags)
        {
            Service.WindowSystem.AddWindow(this);

            saveAndCloseButtons = new(this);

            SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(300,225),
                MaximumSize = new(300,225)
            };
        }
        public void Dispose()
        {
            Service.WindowSystem.RemoveWindow(this);
        }

        public override void Draw()
        {
            ImGui.PushID("SettingsWindow");

            ImGui.Text("This plugin will limit your games framerate\n" +
                       "when you are not in combat or in a duty");

            ImGui.Spacing();

            if (ImGui.Checkbox("Enable", ref Service.Configuration.EnableLimiter))
            {
                Service.Configuration.Save();
            }

            if (Service.Configuration.EnableLimiter)
            {
                ImGui.SetNextItemWidth(50 * ImGuiHelpers.GlobalScale);
                ImGui.InputInt("Framerate Limit", ref newFramerateLimit, 0, 0);

                var frametimeExact = 1000 / newFramerateLimit + 1;
                var approximateFramerate = 1000 / frametimeExact;
                
                ImGui.Text("Approximated Framerate: " + approximateFramerate);
                ImGuiComponents.HelpMarker("Framerate limit will be approximated not exact");

                if (ImGui.Button("Apply", new Vector2(75, 23)))
                {
                    Service.Configuration.FrameRateLimit = Math.Max(newFramerateLimit, 10);
                    Service.Configuration.Save();
                }
            }

            saveAndCloseButtons.Draw();

            ImGui.PopID();
        }
    }
}
