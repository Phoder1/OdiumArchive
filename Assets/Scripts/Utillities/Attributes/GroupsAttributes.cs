using Sirenix.OdinInspector;
using System;

namespace WizardParty
{
    public static class GroupsOrder
    {
        public const int Events = 999;
    }
    [IncludeMyAttributes]
    [FoldoutGroup("Events", GroupsOrder.Events)]
    public class EventsGroup : Attribute { }
}
