namespace Snoop.Views.DebugListenerTab;

using System;
using System.ComponentModel;
using System.Xml.Serialization;
using JetBrains.Annotations;

[Serializable]
public abstract class SnoopFilter : INotifyPropertyChanged
{
    private string groupId = string.Empty;
    private bool isDirty;
    private bool isGrouped;
    private bool isInverse;

    [XmlIgnore]
    public bool IsDirty
    {
        get => this.isDirty;

        protected set
        {
            this.isDirty = value;
            this.RaisePropertyChanged(nameof(this.IsDirty));
        }
    }

    public virtual bool SupportsGrouping => true;

    public bool IsInverse
    {
        get => this.isInverse;

        set
        {
            if (value == this.isInverse)
            {
                return;
            }

            this.isInverse = value;
            this.RaisePropertyChanged(nameof(this.IsInverse));
            this.RaisePropertyChanged(nameof(this.IsInverseText));
        }
    }

    public string IsInverseText => this.isInverse ? "NOT" : string.Empty;

    public bool IsGrouped
    {
        get => this.isGrouped;

        set
        {
            this.isGrouped = value;
            this.RaisePropertyChanged(nameof(this.IsGrouped));
            this.groupId = string.Empty;
        }
    }

    public virtual string GroupId
    {
        get => this.groupId;

        set
        {
            this.groupId = value;
            this.RaisePropertyChanged(nameof(this.GroupId));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void ResetDirtyFlag()
    {
        this.IsDirty = false;
    }

    public abstract bool FilterMatches(string? debugLine);

    [NotifyPropertyChangedInvocator]
    protected void RaisePropertyChanged(string propertyName)
    {
        this.isDirty = true;
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}