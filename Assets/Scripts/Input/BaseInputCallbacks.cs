using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WizardParty.Input
{
    [Serializable]
    public abstract class BaseInputCallbacks
    {
        #region State
        [SerializeField]
        private WizardPartyInput _input;
        #endregion
        #region Properties
        public WizardPartyInput Input => _input;
        public InputAction Action => Input?.InputAction;
        #endregion
        #region Constructors
        public BaseInputCallbacks(WizardPartyInput input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
        }
        #endregion
        #region Public
        public void Enable()
        {
            if (Input.IsValid())
            {
                if (!Action.enabled)
                    Action.Enable();

                Action.started += Action_started;
                Action.performed += Action_performed;
                Action.canceled += Action_canceled;

                TriggerCallback(Action.phase);
            }
            else
                throw new NullReferenceException();
        }
        public void Update()
        {
            if (Action.IsValidAndEnabled() && Action.inProgress)
                Action_update();
        }
        public void Disable()
        {
            if (Action.IsValid())
            {
                Action.started -= Action_started;
                Action.performed -= Action_performed;
                Action.canceled -= Action_canceled;
            }
            else
                throw new NullReferenceException();
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
        public static BaseInputCallbacks GetCallbackOfType(WizardPartyInput input)
        {
            if (!input.IsValid())
                return null;

            Type type = input.InputAction.GetReturnType();

            if (type == null)
                return null;

            if (type == typeof(void))
                return new ButtonInputCallbacks(input);

            if (type == typeof(Vector2))
                return new InputCallbacksVector2(input);

            if (type == typeof(int))
                return new InputCallbacksInt(input);

            if (type == typeof(float))
                return new InputCallbacksFloat(input);

            throw new NotImplementedException();
        }
        #endregion
        #region Interface
        protected abstract void Action_started(InputAction.CallbackContext ctx);
        protected abstract void Action_performed(InputAction.CallbackContext ctx);
        protected abstract void Action_update();
        protected abstract void Action_canceled(InputAction.CallbackContext ctx);
        #endregion
        #region Editor

#if UNITY_EDITOR
        [ShowInInspector]
        private string Name => Action?.name ?? "Not assigned";
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
#endif
        #endregion
    }
    [Serializable]
    public class ButtonInputCallbacks : BaseInputCallbacks
    {
        #region Serielized
        [SerializeField, EventsGroup]
        private UnityEvent OnStarted;
        [SerializeField, EventsGroup]
        private UnityEvent OnPerformed;
        [SerializeField, EventsGroup]
        private UnityEvent OnUpdate;
        [SerializeField, EventsGroup]
        private UnityEvent OnCanceled;
        #endregion
        #region Constructors
        public ButtonInputCallbacks(WizardPartyInput input) : base(input) { }
        #endregion
        #region Interface
        protected override void Action_started(InputAction.CallbackContext ctx) => OnStarted?.Invoke();
        protected override void Action_performed(InputAction.CallbackContext ctx) => OnPerformed?.Invoke();
        protected override void Action_update() => OnUpdate?.Invoke();
        protected override void Action_canceled(InputAction.CallbackContext ctx) => OnCanceled?.Invoke();
        #endregion
    }
    [Serializable]
    public class InputCallbacks<T> : BaseInputCallbacks where T : struct
    {
        #region Serielized
        [SerializeField, EventsGroup]
        private UnityEvent<T> OnStarted;
        [SerializeField, EventsGroup]
        private UnityEvent<T> OnPerformed;
        [SerializeField, EventsGroup]
        private UnityEvent<T> OnUpdate;
        [SerializeField, EventsGroup]
        private UnityEvent<T> OnCanceled;
        #endregion
        #region Constructors
        public InputCallbacks(WizardPartyInput input) : base(input) { }
        #endregion
        #region Interface
        protected override void Action_started(InputAction.CallbackContext ctx) => OnStarted?.Invoke(ctx.ReadValue<T>());
        protected override void Action_performed(InputAction.CallbackContext ctx) => OnPerformed?.Invoke(ctx.ReadValue<T>());
        protected override void Action_update()
        {
            if (Action == null)
                return;

            OnUpdate?.Invoke(Action.ReadValue<T>());
        }
        protected override void Action_canceled(InputAction.CallbackContext ctx) => OnCanceled?.Invoke(ctx.ReadValue<T>());
        #endregion
    }
    [Serializable]
    public class InputCallbacksInt : InputCallbacks<int>
    {
        public InputCallbacksInt(WizardPartyInput input) : base(input) { }
    }
    [Serializable]
    public class InputCallbacksVector2 : InputCallbacks<Vector2>
    {
        public InputCallbacksVector2(WizardPartyInput input) : base(input) { }
    }
    [Serializable]
    public class InputCallbacksFloat : InputCallbacks<float>
    {
        public InputCallbacksFloat(WizardPartyInput input) : base(input) { }
    }
}
