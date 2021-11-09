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
        #region Serielized
        [SerializeField, InputDropdown, HideLabel, OnValueChanged("Reset")]
        private string _input;
        #endregion
        #region Class Data
        private Guid _guid;
        private InputAction _inputAction;
        #endregion
        #region Properties
        public string GuidString => _input;
        public Guid ID
        {
            get
            {
                if (_guid == Guid.Empty)
                {
                    if (string.IsNullOrWhiteSpace(GuidString))
                        return Guid.Empty;

                    _guid = GuidString.ToGuid();
                }

                return _guid;
            }
        }
        public InputAction InputAction
        {
            get
            {
                if (_inputAction == null || _inputAction.actionMap == null)
                {
                    if (ID == Guid.Empty)
                        return null;

                    _inputAction = ID.FindAction();
                }

                return _inputAction;
            }
        }
        #endregion
        #region Editor
#if UNITY_EDITOR
        private void Reset()
        {
            _guid = Guid.Empty;
            _inputAction = null;
        }
#endif
        #endregion
    }
}
