using System;
using Avalonia.Data.Converters;

namespace GT2.SaveEditor.GUI
{
    public static class Converters
    {
        public static FuncValueConverter<double?, string> HandicapStartConverter { get; } = new FuncValueConverter<double?, string>(GetHandicapStartLabel);

        private static string GetHandicapStartLabel(double? value)
        {
            double handicap = value ?? 0;
            return handicap == 0 ? "0m" : $"{(handicap > 0 ? "1P" : "2P")} -{Math.Abs(handicap) * 10}m";
        }
    }
}