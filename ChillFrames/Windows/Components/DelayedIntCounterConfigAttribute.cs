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

    private readonly int minValue;
    private readonly int maxValue;

    public DelayedIntCounterConfigAttribute(string? label, int minValue = 0, int maxValue = 100, string? helpText = null) : base(label)
    {
        helpTextKey = helpText;
        showStep = true;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
    
    public DelayedIntCounterConfigAttribute(string? label, bool showStepButtons, int minValue = 0, int maxValue = 100, string? helpText = null) : base(label)
    {
        helpTextKey = helpText;
        showStep = showStepButtons;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
    
    protected override void DrawLeftColumn(object obj, MemberInfo field, Action? saveAction = null)
    {
        var intValue = GetValue<int>(obj, field);

        ImGui.InputInt($"##{field.Name}", ref intValue, showStep ? 1 : 0);

        if (ImGui.IsItemDeactivatedAfterEdit())
        {
            intValue = Math.Clamp(intValue, minValue, maxValue);
            
            SetValue(obj, field, intValue);
            saveAction?.Invoke();
        }
        
        if(helpTextKey is not null) ImGuiComponents.HelpMarker(HelpText);
    }
}