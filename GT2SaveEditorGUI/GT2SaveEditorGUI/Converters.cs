using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using GT2.SaveEditor.GTMode.License;

namespace GT2.SaveEditor.GUI
{
    public static class Converters
    {
        private static readonly SolidColorBrush goldColour = new(new Color(255, 255, 200, 0));
        private static readonly SolidColorBrush silverColour = new(new Color(255, 195, 195, 195));
        private static readonly SolidColorBrush bronzeColour = new(new Color(255, 175, 120, 0));
        private static readonly SolidColorBrush kidsPrizeColour = new(new Color(255, 0, 200, 0));
        private static readonly SolidColorBrush blankColour = new(new Color(0, 0, 0, 0));

        public static FuncValueConverter<double?, string> HandicapStartConverter { get; } = new FuncValueConverter<double?, string>(GetHandicapStartLabel);

        public static FuncValueConverter<LicenseTestResultEnum?, IBrush> LicenseResultColourConverter { get; } = new FuncValueConverter<LicenseTestResultEnum?, IBrush>(GetLicenseResultColour);

        public static FuncValueConverter<int?, string> RecordTimeConverter { get; } = new FuncValueConverter<int?, string>(FormatRecordTime);

        public static FuncValueConverter<ushort?, string> RecordSpeedConverter { get; } = new FuncValueConverter<ushort?, string>(FormatRecordSpeed);

        private static string GetHandicapStartLabel(double? value)
        {
            double handicap = value ?? 0;
            return handicap == 0 ? "0m" : $"{(handicap > 0 ? "1P" : "2P")} -{Math.Abs(handicap) * 10}m";
        }

        private static SolidColorBrush GetLicenseResultColour(LicenseTestResultEnum? result) =>
            result switch
            {
                LicenseTestResultEnum.Gold => goldColour,
                LicenseTestResultEnum.Silver => silverColour,
                LicenseTestResultEnum.Bronze => bronzeColour,
                LicenseTestResultEnum.KidsPrize => kidsPrizeColour,
                _ => blankColour
            };

        private static string FormatRecordTime(int? value) =>
            value == -1 ? "--:--.---" : TimeSpan.FromMilliseconds(value ?? 0).ToString("mm\\:ss\\.fff");

        private static string FormatRecordSpeed(ushort? value) => $"{(value ?? 0) / 100M:N2} km/h";
    }
}