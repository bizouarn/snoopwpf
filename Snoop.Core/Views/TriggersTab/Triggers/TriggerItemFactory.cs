namespace Snoop.Views.TriggersTab.Triggers;

using System.Windows;

public static class TriggerItemFactory
{
    public static TriggerItemBase? GetTriggerItem(TriggerBase triggerBase, DependencyObject source, TriggerSource triggerSource)
    {
        TriggerItemBase triggerItem;
        if (triggerBase is Trigger trigger)
        {
            triggerItem = new TriggerItem(trigger, source, triggerSource);
        }
        else if (triggerBase is DataTrigger dataTrigger)
        {
            triggerItem = new DataTriggerItem(dataTrigger, source, triggerSource);
        }
        else if (triggerBase is MultiTrigger multiTrigger)
        {
            triggerItem = new MultiTriggerItem(multiTrigger, source, triggerSource);
        }
        else if (triggerBase is MultiDataTrigger multiDataTrigger)
        {
            triggerItem = new MultiDataTriggerItem(multiDataTrigger, source, triggerSource);
        }
        else if (triggerBase is EventTrigger eventTrigger)
        {
            triggerItem = new EventTriggerItem(eventTrigger, source, triggerSource);
        }
        else
        {
            return null;
        }

        triggerItem.Initialize();
        return triggerItem;
    }
}