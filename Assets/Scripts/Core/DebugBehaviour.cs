using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace WizardParty
{
    public abstract class DebugBehaviour : MonoBehaviour
    {
#if ENABLE_EDITOR_HUB_LICENSE
        [SerializeField, DebugToggleGroup]
        protected bool _debuggingEnabled = false;

		// Send a log to a specific filter regardless of contents
		// Ex: ConsoleProDebug.LogToFilter("Hi", "CustomFilter");
		public static void LogToFilter(string inLog, string inFilterName, UnityEngine.Object inContext = null)
			=> ConsoleProDebug.LogToFilter(inLog, inFilterName, inContext);

		// Send a log as a regular log but change its type in ConsolePro
		// Ex: ConsoleProDebug.LogAsType("Hi", "Error");
		public static void LogAsType(string inLog, string inTypeName, UnityEngine.Object inContext = null)
			=> ConsoleProDebug.LogAsType(inLog, inTypeName, inContext);

		// Watch a variable. This will only produce one log entry regardless of how many times it is logged, allowing you to track variables without spam.
		// Ex:
		// void Update() {
		// ConsoleProDebug.Watch("Player X Position", transform.position.x);
		// }
		public static void Watch(string inName, string inValue)
			=> ConsoleProDebug.Watch(inName, inValue);

		[IncludeMyAttributes]
        [ToggleGroup(nameof(_debuggingEnabled), 9999, "Debug", AnimateVisibility = false, CollapseOthersOnExpand = false, HideWhenChildrenAreInvisible = false)]
        protected class DebugToggleGroup : Attribute { }
#endif
    }
}
