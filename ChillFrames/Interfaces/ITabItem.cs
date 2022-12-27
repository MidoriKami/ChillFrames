using System;

namespace ChillFrames.Interfaces;

internal interface ITabItem : IDisposable
{
    string TabName { get; }
    bool Enabled { get; }
    void Draw();
}