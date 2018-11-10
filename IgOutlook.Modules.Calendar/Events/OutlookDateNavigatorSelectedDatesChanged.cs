using Prism.Events;
using System;
using System.Collections.ObjectModel;

namespace IgOutlook.Modules.Calendar.Events
{
    public class OutlookDateNavigatorSelectedDatesChanged : PubSubEvent<ObservableCollection<DateTime>> { }
}
