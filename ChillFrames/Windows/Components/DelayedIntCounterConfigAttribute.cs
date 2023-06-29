using System;
using System.Reflection;
using Dalamud.Interface.Components;
using ImGuiNET;
using KamiLib.AutomaticUserInterface;

namespace ChillFrames.Windows.Components;

public class DelayedIntCounterConfigAttribute : IntCounterConfigAttribute
{
    private readonly string? helpTextKey;
    private string HelpText => TryGetLocalizedString(helpTextKey);

    private readonly bool showStep;

    public DelayedIntCounterConfigAttribute(string? label, string? helpText = null) : base(label)
    {
        helpTextKey = helpText;
        showStep = true;
    }
    
    public DelayedIntCounterConfigAttribute(string? label, bool showStepButtons, string? helpText = null) : base(label)
    {
        helpTextKey = helpText;
        showStep = showStepButtons;
    }
    
    protected override void DrawLeftColumn(object obj, MemberInfo field, Action? saveAction = null)
    {
        var intValue = GetValue<int>(obj, field);

        ImGui.InputInt($"##{field.Name}", ref intValue, showStep ? 1 : 0);

        if (ImGui.IsItemDeactivatedAfterEdit())
        {
            SetValue(obj, field, intValue);
            saveAction?.Invoke();
        }
        
        if(helpTextKey is not null) ImGuiComponents.HelpMarker(HelpText);
    }
}