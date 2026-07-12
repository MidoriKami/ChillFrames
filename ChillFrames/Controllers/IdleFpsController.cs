using System;
using System.Threading.Tasks;
using ChillFrames.Classes;
using Dalamud.Utility;
using Dalamud.Utility.Signatures;

namespace ChillFrames.Controllers;

public class IdleFpsController : IAsyncDisposable {

	[Signature("74 ?? B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? B0 ?? 48 83 C4")]
	private readonly nint? jumpInstructionAddress = null;
	private MemoryReplacement? waitTimePatch;

	public IdleFpsController() {
		Services.Hooker.InitializeFromAttributes(this);
		ApplyPatch();
	}

	public void SetWaitTime(int waitTime) {
		if (waitTimePatch is null) return;
		ThreadSafety.AssertMainThread();

		var bytes = BitConverter.GetBytes(waitTime);
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}

		waitTimePatch.UpdateReplacementBytes(bytes);
	}

	public void UpdateWaitTime()
		=> SetWaitTime(System.Config.IdleFpsWaitTime);

	private void ApplyPatch() {
		if (jumpInstructionAddress is null) return;
		if (jumpInstructionAddress == nint.Zero) return;

		var bytes = BitConverter.GetBytes(System.Config.IdleFpsWaitTime);
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}

		// Address is the address of the jump instruction as the mov doesn't have a valid sig.
		// Offsets by 3 bytes to get to the 32h, which is the value that is modified.
		// .text:00000001400D1F10                 jz      short loc_1400D1F1C <-- Instruction Signature.
		// .text:00000001400D1F12                 mov     ecx, 32h ; '2'  ; dwMilliseconds <-- Edited value
		// .text:00000001400D1F17                 call    j_SleepEx

		waitTimePatch = new MemoryReplacement(jumpInstructionAddress.Value + 3, bytes);

		Services.Framework.RunSafely(() => waitTimePatch.Enable());
	}

	public async ValueTask DisposeAsync() {
		if (waitTimePatch is not null) {
			await waitTimePatch.DisposeAsync();
		}
	}
}