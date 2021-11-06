using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Phoder1.AsyncManagement;

namespace WizardParty.Input.Runtime
{
    public class LocalInputMB : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, InputDropdown(null)]
        private string _actionGuid;
        [Button]
        private void AddToList()
        {
            Array.Resize(ref _inputs, _inputs.Length + 1);
            _inputs[^1] = new InputCallbacks(_actionGuid);
        }
#endif

        [SerializeReference]
        [ListDrawerSettings(Expanded = true)]
        private IInputCallbacks[] _inputs;



        private void OnEnable()
        {
            Array.ForEach(_inputs, (x) => x.Enable());
        }
        private void Update()
        {
            Array.ForEach(_inputs, (x) => x.Update());
        }
        private void OnDisable()
        {
            Array.ForEach(_inputs, (x) => x.Disable());
        }

        
    }
    public readonly struct Void { }
    [Serializable]
    public class InputCallbacks : IInputCallbacks
    {
        public InputCallbacks()
        {

        }
        public InputCallbacks(string actionGuid)
        {
            _actionGuid = actionGuid ?? throw new ArgumentNullException(nameof(actionGuid));
        }

        [SerializeField, InputDropdown(typeof(Vector2))]
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

                return Action.bindings.ToDisplayName((x) => x.isComposite ? x.GetNameOfComposite() + ":" : x.ToDisplayString());
            }
        }
        private string Name => Action?.name ?? "Not assigned";
#endif
        [SerializeField, EventsGroup]
        private UnityEvent<InputAction.CallbackContext> OnStarted;
        [SerializeField, EventsGroup]
        private UnityEvent<InputAction.CallbackContext> OnPerformed;
        [SerializeField, EventsGroup]
        private UnityEvent OnUpdate;
        [SerializeField, EventsGroup]
        private UnityEvent<InputAction.CallbackContext> OnCanceled;

        [NonSerialized]
        private InputAction _action;

        public InputAction Action
        {
            get
            {
                if (_action == null)
                {
                    _action = InputManager.FindAction(Guid);

                    if (_action == null)
                        return null;
#if UNITY_EDITOR
                    Debug.Log($"Action {_action.name} of type {_action.expectedControlType} + {_action.GetReturnType()}");
#endif
                }

                return _action;
            }
        }
        public void Enable()
        {
            if (Action.IsValid())
            {
                if (!Action.enabled)
                    Action.Enable();

                Action.started += Action_started;
                Action.performed += Action_performed;
                Action.canceled += Action_canceled;

                TriggerCallback(Action.phase);
            }
        }
        public void Update()
        {
            if (Action.IsValidAndEnabled() && Action.inProgress)
                OnUpdate?.Invoke();
        }
        public void Disable()
        {
            if (Action.IsValid())
            {
                Action.started -= Action_started;
                Action.performed -= Action_performed;
                Action.canceled -= Action_canceled;
            }
        }

        public void TriggerCallback(InputActionPhase phase)
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
