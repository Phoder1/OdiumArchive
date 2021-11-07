using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WizardParty
{
    public class RigHeadAim 
    {
        #region Inspector

        [BoxGroup("Properties")]
        [SerializeField] private GameObject _headTrackedObject;

        [BoxGroup("Properties")]
        [SerializeField] private Transform _aimTarget;
        // Update is called once per frame

        #endregion

        #region Interface
        public void SetAimTarget(Transform newAimTarget)
        {
            _aimTarget = newAimTarget;
        }
        #endregion

        #region Process
        
        #endregion


    }
}
