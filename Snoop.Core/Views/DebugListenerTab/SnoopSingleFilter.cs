namespace Snoop.Views.DebugListenerTab;

using System;
using System.Text.RegularExpressions;

[Serializable]
public class SnoopSingleFilter : SnoopFilter, ICloneable
{
    private string text;
    private FilterType filterType;

    public SnoopSingleFilter()
    {
        this.text = string.Empty;
    }

    public FilterType FilterType
    {
        get => this.filterType;
        set
        {
            if (value == this.filterType)
            {
                return;
            }

            this.filterType = value;
            this.RaisePropertyChanged(nameof(this.FilterType));
        }
    }

    public string Text
    {
        get
        {
            return this.text;
        }

        set
        {
            this.text = value;
            this.RaisePropertyChanged(nameof(this.Text));
        }
    }

    public override bool FilterMatches(string? debugLine)
    {
        debugLine = debugLine?.ToLower() ?? string.Empty;
        var lowerText = this.Text.ToLower();
        var filterMatches = this.FilterType switch
        {
            FilterType.Contains => debugLine.Contains(lowerText, StringComparison.Ordinal),
            FilterType.StartsWith => debugLine.StartsWith(lowerText, StringComparison.Ordinal),
            FilterType.EndsWith => debugLine.EndsWith(lowerText, StringComparison.Ordinal),
            FilterType.RegularExpression => TryMatch(debugLine, lowerText),
            _ => false
        };

        if (this.IsInverse)
        {
            filterMatches = !filterMatches;
        }

        return filterMatches;
    }

    private static bool TryMatch(string input, string pattern)
    {
        try
        {
            return Regex.IsMatch(input, pattern);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public object Clone()
    {
        var newFilter = new SnoopSingleFilter
        {
            IsGrouped = this.IsGrouped,
            GroupId = this.GroupId,
            Text = this.Text,
            FilterType = this.FilterType,
            IsInverse = this.IsInverse
        };
        return newFilter;
    }
}