using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WizardParty.Input.Runtime;

namespace WizardParty.Input
{
    [Serializable, InlineProperty]
    public class WizardPartyInput
    {
        [SerializeField, InputDropdown, HideLabel, OnValueChanged(nameof(Reset))]
        private string _input;
        public string GuidString => _input;

        private Guid _guid;
        public Guid ID
        {
            get
            {
                if(_guid == Guid.Empty)
                {
                    if (string.IsNullOrWhiteSpace(GuidString))
                        return Guid.Empty;

                    _guid = GuidString.ToGuid();
                }

                return _guid;
            }
        }
        private InputAction _inputAction;
        public InputAction InputAction
        {
            get
            {
                if(_inputAction == null || _inputAction.actionMap == null)
                {
                    if (ID == Guid.Empty)
                        return null;

                    _inputAction = ID.FindAction();
                }

                return _inputAction;
            }
        }
#if UNITY_EDITOR
        private void Reset()
        {
            _guid = Guid.Empty;
            _inputAction = null;
        }
#endif
    }
}
