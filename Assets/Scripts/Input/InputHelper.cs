using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WizardParty.Input
{
    public static class InputHelper
    {
        public static bool IsValid(this InputAction action)
            => action != null && action.bindings.Count != 0;
        public static bool IsValidAndEnabled(this InputAction action)
            => action.IsValid() && action.enabled;
        public static bool IsValidAndEnabled(this InputActionMap map)
            => map.IsValid() && map.enabled;
        public static bool IsValid(this InputActionMap map)
            => map != null && map.actions.Count != 0;
        public static Type GetReturnType(this InputAction action)
        {
            switch (action.type)
            {

                case InputActionType.Value:
                    switch (action.expectedControlType)
                    {
                        case "Vector2":
                            return typeof(Vector2);
                        default:
                            Debug.LogError($"New control type has been added of type: {action.expectedControlType}");
                            return typeof(void);
                    }

                case InputActionType.Button:
                    return typeof(void);

                case InputActionType.PassThrough:
                    return typeof(void);

                default:
                    return typeof(void);
            }


        }
    }
}
