namespace Snoop.Infrastructure.Helpers;

using System.Windows;
using System.Windows.Controls;

public static class TemplateHelper
{
    public static object? GetChildFromTemplateIfNeeded(DependencyObject element, string? templatePartName)
    {
        if (string.IsNullOrEmpty(templatePartName))
        {
            return element;
        }

        return element switch
        {
            Control { Template: { } } control => control.Template.FindName(templatePartName, control),
            FrameworkElement fe => fe.FindName(templatePartName),
            FrameworkContentElement fec => fec.FindName(templatePartName),
            _ => null
        };
    }
}