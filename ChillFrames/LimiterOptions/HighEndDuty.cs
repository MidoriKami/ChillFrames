using ChillFrames.Classes;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.Sheets;

namespace ChillFrames.LimiterOptions;

public unsafe class HighEndDuty : IFrameLimiterOption {

	public string Label => "Duties - High End";

	public bool Active {
		get {
			var currentContentFinderCondition = GameMain.Instance()->CurrentContentFinderConditionId;
			if (currentContentFinderCondition is 0) return false;

			return IDataManager.Get()
			               .GetExcelSheet<ContentFinderCondition>()
			               .GetRow(currentContentFinderCondition)
			               .HighEndDuty;
		}
	}

	public ref LimiterStateTarget Target => ref System.Config.General.HighEndDutyTarget;
}