﻿using ChillFrames.Classes;
using FFXIVClientStructs.FFXIV.Client.Game.MJI;

namespace ChillFrames.LimiterOptions;

public unsafe class IslandSanctuary : IFrameLimiterOption {
    public string Label => "Island Sanctuary";
    
    public bool Active => MJIManager.Instance() is not null && MJIManager.Instance()->IsPlayerInSanctuary;
    
    public ref bool Enabled => ref System.Config.General.DisableIslandSanctuarySetting;
}