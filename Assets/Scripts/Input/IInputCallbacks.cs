using System;
using UnityEngine.InputSystem;

namespace WizardParty.Input.Runtime
{
    public interface IInputCallbacks
    {
        InputAction Action { get; }
        Guid Guid { get; }

        void Disable();
        void Enable();
        void TriggerCallback(InputActionPhase phase);
        void Update();
    }
}