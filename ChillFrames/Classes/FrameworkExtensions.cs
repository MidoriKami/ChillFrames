using System;
using System.Threading.Tasks;
using Dalamud.Plugin.Services;

namespace ChillFrames.Classes;

public static class FrameworkExtensions {
	extension(IFramework framework) {
		/// <summary>
		/// Helper method for calling Framework.Run in a safe way that does nothing in the case of a game shutdown.
		/// </summary>
		public Task RunSafely(Action runAction)
			=> framework.IsFrameworkUnloading ? Task.CompletedTask : framework.Run(runAction);
	}
}