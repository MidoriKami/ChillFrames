using System;
using System.Threading.Tasks;
using Dalamud.Memory;
using Dalamud.Plugin.Services;
using Dalamud.Utility;

namespace ChillFrames.Classes;

public class MemoryReplacement(nint address, byte[] replacementBytes) : IDisposable, IAsyncDisposable {

	private byte[]? originalBytes;
	private byte[] replacementBytes = replacementBytes;

	/// <summary>
	/// Applies memory patch.
	/// </summary>
	/// <exception cref="InvalidOperationException">When executed while not on the main thread. Await <see cref="EnableAsync"/> while not on main thread.</exception>
	public void Enable() {
		ThreadSafety.AssertMainThread();

		if (originalBytes != null)
			return;

		originalBytes = ReplaceRaw(address, replacementBytes);
	}

	public async Task EnableAsync()
		=> await IFramework.Get().Run(Enable);

	public void Disable() {
		ThreadSafety.AssertMainThread();

		if (originalBytes == null)
			return;

		ReplaceRaw(address, originalBytes);
		originalBytes = null;
	}

	// Update the current value while maintaining the original bytes.
	// Allows for continuous updating.
	public void UpdateReplacementBytes(byte[] newBytes) {
		ThreadSafety.AssertMainThread();

		// Abort replacement if we haven't Enabled at least once yet.
		if (originalBytes == null)
			return;

		replacementBytes = newBytes;

		ReplaceRaw(address, replacementBytes);
	}

	public async Task DisableAsync()
		=> await IFramework.Get().Run(Disable);

	public void Dispose()
		=> Disable();

	public async ValueTask DisposeAsync()
		=> await DisableAsync();

	private static byte[] ReplaceRaw(nint address, byte[] data) {
		var originalBytes = MemoryHelper.ReadRaw(address, data.Length);

		MemoryHelper.ChangePermission(address, data.Length, MemoryProtection.ExecuteReadWrite, out var oldPermissions);
		MemoryHelper.WriteRaw(address, data);
		MemoryHelper.ChangePermission(address, data.Length, oldPermissions);

		return originalBytes;
	}
}
