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

        public static InputAction FindAction(Guid guid) => Controls.asset.FindAction(guid);
    }
}
