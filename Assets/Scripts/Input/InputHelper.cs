using UnityEngine.InputSystem;

namespace WizardParty.Input
{
    public static class InputHelper
    {
        public static bool IsValid(this InputAction action)
            => action != null && action.bindings.Count != 0;
        public static bool IsValid(this InputCallbacks action)
            => action != null && IsValid(action.Action);
        public static bool IsValid(this InputActionMap map)
            => map != null && map.actions.Count != 0;
    }
}
