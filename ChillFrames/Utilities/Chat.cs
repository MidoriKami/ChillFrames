using System;
using Dalamud.Game.Text.SeStringHandling;

namespace ChillFrames.Utilities;

internal static class Chat
{
    public static void Print(string tag, string message)
    {
        var debugEnabled = Service.Configuration.System.EnableDebugOutput;
        if (debugEnabled == false && tag.ToLower() == "debug") return;

        var stringBuilder = new SeStringBuilder();
        stringBuilder.AddUiForeground(45);
        stringBuilder.AddText($"[ChillFrames] ");
        stringBuilder.AddUiForegroundOff();
        stringBuilder.AddUiForeground(62);
        stringBuilder.AddText($"[{tag}] ");
        stringBuilder.AddUiForegroundOff();
        stringBuilder.AddText(message);

        Service.Chat.Print(stringBuilder.BuiltString);
    }

    public static void Debug(string data)
    {
        Print("Debug", data);
    }

    public static unsafe void Debug(string data, void* address)
    {
        Print("Debug", data + $" {(IntPtr) address:x8}");
    }
}