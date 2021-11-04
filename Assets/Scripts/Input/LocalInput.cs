using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WizardParty.Input.Runtime
{
    public class LocalInput : MonoBehaviour
    {
        [SerializeField, InputDropdown]
        private string _actionGuid;
        public Guid Guid => (!string.IsNullOrWhiteSpace(_actionGuid) && Guid.TryParse(_actionGuid, out var guid)) ? guid : Guid.Empty;

#if UNITY_EDITOR
        [ShowInInspector]
        private string ActionBindings
        {
            get
            {
                if (!Action.IsValid())
                    return null;

                return Action.Action.bindings.ToDisplayName((x) => x.isComposite ? x.GetNameOfComposite() + ":" : x.ToDisplayString());
            }
        }
#endif
        [SerializeField, EventsGroup]
        private UnityEvent<InputAction.CallbackContext> OnStarted;
        [SerializeField, EventsGroup]
        private UnityEvent<InputAction.CallbackContext> OnPerformed;
        [SerializeField, EventsGroup]
        private UnityEvent<InputAction.CallbackContext> OnCanceled;

        [NonSerialized]
        private InputCallbacks _action;
        public InputCallbacks Action
        {
            get
            {
                if (_action == null)
                {
                    _action = InputManager.FindAction(Guid);

                    if (_action == null)
                        return null;
                }

                return _action;
            }
        }
        private void OnEnable()
        {
            if(Action != null)
            {
                Action.Started += Action_started;
                Action.Performed += Action_performed;
                Action.Canceled += Action_canceled;
            }
        }
        private void OnDisable()
        {
            if (Action != null)
            {
                Action.Started -= Action_started;
                Action.Performed -= Action_performed;
                Action.Canceled -= Action_canceled;
            }
        }

        private void TriggerCallback(InputActionPhase phase)
        {
            switch (phase)
            {
                case InputActionPhase.Started:
                    break;
                case InputActionPhase.Performed:
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }
        private void Action_started(InputAction.CallbackContext ctx) => OnStarted?.Invoke(ctx);
        private void Action_performed(InputAction.CallbackContext ctz) => OnPerformed?.Invoke(ctz);
        private void Action_canceled(InputAction.CallbackContext ctx) => OnCanceled?.Invoke(ctx);
    }
}
