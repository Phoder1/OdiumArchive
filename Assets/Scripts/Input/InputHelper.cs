using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WizardParty.Input
{
    public static class InputHelper
    {
        #region Extension methods
        public static bool IsValid(this InputAction action)
            => action != null && action.bindings.Count != 0;
        public static bool IsValidAndEnabled(this InputAction action)
            => action.IsValid() && action.enabled;
        public static bool IsValidAndEnabled(this InputActionMap map)
            => map.IsValid() && map.enabled;
        public static bool IsValid(this InputActionMap map)
            => map != null && map.actions.Count != 0;
        public static bool IsValid(this WizardPartyInput input)
            => input != null && input.InputAction.IsValid();
        public static Type GetReturnType(this InputAction action)
        {
            switch (action.type)
            {

                case InputActionType.Value:
                    switch (action.expectedControlType)
                    {
                        case "Vector2":
                            return typeof(Vector2);
                        case "Integer":
                            return typeof(int);
                        case "":
                            return typeof(void);
                        default:
                            throw new NotImplementedException($"New control type has been added of type: {action.expectedControlType}");
                    }

                case InputActionType.Button:
                    return typeof(void);

                case InputActionType.PassThrough:
                    return typeof(void);

                default:
                    return typeof(void);
            }
        }
        public static Guid ToGuid(this string guid) => (!string.IsNullOrWhiteSpace(guid) && Guid.TryParse(guid, out var output)) ? output : Guid.Empty;
        public static InputAction FindAction(this Guid guid) => guid == Guid.Empty ? null : InputManager.Controls.asset.FindAction(guid);
        #endregion
    }
}
