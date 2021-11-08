using System;
using UnityEngine.InputSystem;

namespace WizardParty.Input
{
    public static class InputManager
    {
        private static readonly WizardPartyControls _controls = new();
        public static WizardPartyControls Controls => _controls;
        static InputManager()
        {
            _controls.Enable();
        }

        public static InputAction FindAction(this Guid guid) => guid == Guid.Empty ? null : Controls.asset.FindAction(guid);
    }
}
