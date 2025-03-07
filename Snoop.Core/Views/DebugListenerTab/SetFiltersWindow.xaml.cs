namespace Snoop.Views.DebugListenerTab;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Snoop.Core;

public partial class SetFiltersWindow
{
    public SetFiltersWindow(FiltersViewModel viewModel)
    {
        this.DataContext = viewModel;
        viewModel.ResetDirtyFlag();

        this.InitializeComponent();

        this.initialFilters = this.MakeDeepCopyOfFilters(this.ViewModel.Filters);

        this.Closed += this.SetFiltersWindow_Closed;
    }

    internal FiltersViewModel ViewModel => (FiltersViewModel)this.DataContext;

    private void SetFiltersWindow_Closed(object? sender, EventArgs e)
    {
        if (this.setFilterClicked || !this.ViewModel.IsDirty)
        {
            return;
        }

        var saveChanges = MessageBox.Show("Save changes?", "Changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        if (saveChanges)
        {
            this.ViewModel.SetIsSet();
            this.SaveFiltersToSettings();
            return;
        }

        this.ViewModel.InitializeFilters(this.initialFilters);
    }

    private void ButtonAddFilter_Click(object sender, RoutedEventArgs e)
    {
        this.ViewModel.AddFilter(new SnoopSingleFilter());
    }

    private void ButtonRemoveFilter_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: SnoopFilter filter })
        {
            this.ViewModel.RemoveFilter(filter);
        }
    }

    private void ButtonSetFilter_Click(object sender, RoutedEventArgs e)
    {
        this.SaveFiltersToSettings();

        this.ViewModel.SetIsSet();
        this.setFilterClicked = true;
        this.Close();
    }

    private void TextBlockFilter_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.Focus();
            this.listBoxFilters.ScrollIntoView(textBox);
        }
    }

    private void MenuItemGroupFilters_Click(object sender, RoutedEventArgs e)
    {
        var filtersToGroup = new List<SnoopFilter>();
        foreach (var item in this.listBoxFilters.SelectedItems)
        {
            if (item is SnoopFilter filter && filter.SupportsGrouping)
            {
                filtersToGroup.Add(filter);
            }
        }

        this.ViewModel.GroupFilters(filtersToGroup);
    }

    private void MenuItemClearFilterGroups_Click(object sender, RoutedEventArgs e)
    {
        this.ViewModel.ClearFilterGroups();
    }

    private void MenuItemSetInverse_Click(object sender, RoutedEventArgs e)
    {
        foreach (SnoopFilter? filter in this.listBoxFilters.SelectedItems)
        {
            if (filter is null)
            {
                continue;
            }

            filter.IsInverse = !filter.IsInverse;
        }
    }

    private void SaveFiltersToSettings()
    {
        var singleFilters = new List<SnoopSingleFilter>();
        foreach (var filter in this.ViewModel.Filters)
        {
            if (filter is SnoopSingleFilter)
            {
                singleFilters.Add((SnoopSingleFilter)filter);
            }
        }

        Settings.Default.SnoopDebugFilters.UpdateWith(singleFilters.ToArray());
    }

    private SnoopSingleFilter[] MakeDeepCopyOfFilters(IEnumerable<SnoopFilter> filters)
    {
        return MakeCopyOfFiltersEnum(filters).ToArray();
    }

    private static IEnumerable<SnoopSingleFilter> MakeCopyOfFiltersEnum(IEnumerable<SnoopFilter> filters)
    {
        foreach (var filter in filters)
        {
            if (filter is SnoopSingleFilter singleFilter)
            {
                yield return (SnoopSingleFilter)singleFilter.Clone();
            }
        }
    }

    private readonly SnoopSingleFilter[] initialFilters;
    private bool setFilterClicked;
}