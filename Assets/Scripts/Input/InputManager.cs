using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace WizardParty.Input
{
    public static class InputManager
    {
        private static readonly WizardPartyControls _controls = new();
        public static WizardPartyControls Controls => _controls;
        private static readonly List<InputCallbacks> _callbacks = new();
        public static List<InputCallbacks> Callbacks => _callbacks;
        static InputManager()
        {
            _controls.Enable();
            CreateCallbacks();
        }
        private static void CreateCallbacks()
        {
            foreach (var map in Controls.asset.actionMaps)
            {
                if (!map.IsValid())
                    continue;

                foreach (var action in map.actions)
                {
                    if (!action.IsValid())
                        continue;

                    InputCallbacks callback = InputCallbacks.Create(action);

                    if (callback != null)
                    {
                        Callbacks.Add(callback);
                        callback.Enable();
                    }
                }
            }
        }

        public static InputCallbacks FindAction(Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            return Callbacks.Find((x) => x.Guid == guid);
        }
    }
    public class InputCallbacks
    {
        private readonly InputAction _action;
        public InputAction Action => _action;

        private InputActionPhase _phase = InputActionPhase.Disabled;
        public InputActionPhase Phase 
        { 
            get => _phase;
            set
            {
                if(value == _phase)
                    return;

                _phase = value;
                OnPhaseChanged?.Invoke(_phase);
            }
        }
        public static InputCallbacks Create(InputAction action)
            => action.IsValid() ? new(action) : null;
        protected InputCallbacks(InputAction action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }
        public event Action<InputAction.CallbackContext> Started;
        public event Action<InputAction.CallbackContext> Performed;
        public event Action<InputAction.CallbackContext> Canceled;
        public event Action<InputActionPhase> OnPhaseChanged;
        public Guid Guid => _action.id;


        public void Enable()
        {
            if (!_action.enabled)
                _action.Enable();

            _action.started += Action_started;
            _action.performed += Action_performed;
            _action.canceled += Action_canceled;

            Phase = _action.phase;
        }
        public void Disable()
        {
            _action.started -= Action_started;
            _action.performed -= Action_performed;
            _action.canceled -= Action_canceled;
            Phase = InputActionPhase.Disabled;
        }
        private void Action_started(InputAction.CallbackContext obj)
        {
            Started?.Invoke(obj);
            UpdatePhase(obj);
        }
        private void Action_performed(InputAction.CallbackContext obj)
        {
            Performed?.Invoke(obj);
            UpdatePhase(obj);
        }
        private void Action_canceled(InputAction.CallbackContext obj)
        {
            Canceled?.Invoke(obj);
            UpdatePhase(obj);
            Phase = InputActionPhase.Waiting;
        }
        private void UpdatePhase(InputAction.CallbackContext context)
        {
            Phase = context.phase;
        }
    }
}
