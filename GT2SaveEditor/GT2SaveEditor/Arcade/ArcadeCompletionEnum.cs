using System;

namespace GT2.SaveEditor.Arcade
{
    [Flags] // Treated as flags by the game, but only one is ever set at a time
    public enum ArcadeCompletionEnum
    {
        None,
        Easy,
        Normal,
        Difficult = 4
    }
}