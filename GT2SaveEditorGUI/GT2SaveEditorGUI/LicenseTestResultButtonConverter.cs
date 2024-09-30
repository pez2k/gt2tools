using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using GT2.SaveEditor.GTMode.License;

namespace GT2.SaveEditor.GUI
{
    public class LicenseTestResultButtonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (LicenseTestResultEnum)(value ?? 0) == Enum.Parse<LicenseTestResultEnum>((string?)parameter ?? "None");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return ((bool ?)value == true) ? Enum.Parse<LicenseTestResultEnum>((string?)parameter ?? "None") : BindingOperations.DoNothing;
        }
    }
}