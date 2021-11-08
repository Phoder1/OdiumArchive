using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace WizardParty.Input.Runtime
{
    public class LocalInputMB : MonoBehaviour
    {
        #region Serielized
        [SerializeReference]
        [ListDrawerSettings(Expanded = true, HideAddButton = true, ListElementLabelName = "Name")]
        private List<BaseInputCallbacks> _inputs = new();
        #endregion
        #region Unity
        private void OnEnable()
        {
            _inputs.ForEach((x) => x.Enable());
        }
        private void Update()
        {
            _inputs.ForEach((x) => x.Update());
        }
        private void OnDisable()
        {
            _inputs.ForEach((x) => x.Disable());
        }
        #endregion
        #region Editor
#if UNITY_EDITOR
        [SerializeField]
        private WizardPartyInput _input;
        [Button]
        private void AddToList()
        {
            if (_inputs.Find((x) => x.Input.ID == _input.ID) != null)
                return;

            var callbacks = BaseInputCallbacks.GetCallbackOfType(_input);

            if (callbacks == null)
                return;

            _inputs.Add(callbacks);
            _input = default;
        }
#endif
        #endregion

    }
}
