using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WizardParty.Animations
{
    [System.Serializable]
    public class RigObjectTrackOverride : AnimationRiggingOverride
    {
        #region Inspector

        [BoxGroup("Serialized")]
        [SerializeField] private GameObject _trackedObject;


        #endregion

        #region ClassData

        private Transform _aimTarget;

        #endregion

        #region Interface
        public void SetNewAimTarget(Transform newAimTarget)
        {
            _aimTarget = newAimTarget;
        }

        public void Init(Transform newAimTarget)
        {
            SetNewAimTarget(newAimTarget);
              
        }

        public void UpdateOverride()
        {
            _trackedObject.transform.position = _aimTarget.position;
        }

        #endregion

        #region Process



        #endregion

    }
}
