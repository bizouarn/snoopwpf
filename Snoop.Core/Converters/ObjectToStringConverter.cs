// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace Snoop.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

public class ObjectToStringConverter : IValueConverter
{
    public static readonly ObjectToStringConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return this.Convert(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public string Convert(object value)
    {
        return value switch
        {
            null => "{null}",
            FrameworkElement item when !string.IsNullOrEmpty(item.Name) => $"{item.Name} {FormattedTypeName(item)}",
            RoutedCommand item when !string.IsNullOrEmpty(item.Name) => $"{item.Name} {FormattedTypeName(item)}",
            _ => FormattedTypeName(value)
        };
    }

    private static string FormattedTypeName(object item)
    {
        return $"({item.GetType().Name})";
    }
}