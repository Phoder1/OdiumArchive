using Sirenix.OdinInspector;
using System;

namespace WizardParty.Input.Runtime
{
    [IncludeMyAttributes]
    [ValueDropdown("@InputDropdownAttribute.ActionsDropdown")]
    public class InputDropdownAttribute : Attribute
    {
        public static ValueDropdownList<string> ActionsDropdown
        {
            get
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();

                foreach (var map in InputManager.Controls.asset.actionMaps)
                    foreach (var action in map.actions)
                        list.Add(new ValueDropdownItem<string>($"{map.name}/{action.name}", action.id.ToString()));

                return list;
            }
        }
    }
}
