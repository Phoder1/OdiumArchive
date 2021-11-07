using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardParty.Animations
{
    public class AnimationHandler : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private RigObjectTrackOverride _rigHeadAim;

        [SerializeField] private Transform attentionObject;


        #endregion

        #region Interface

        #endregion

        #region Process

        private void Start()
        {
            _rigHeadAim.Init(attentionObject);

        }

        private void Update()
        {
            _rigHeadAim.UpdateOverride();
        }

        #endregion


    }
}
