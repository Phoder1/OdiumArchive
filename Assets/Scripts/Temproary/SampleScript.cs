using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace WizardParty
{
    [Serializable]
    public class SampleScript : ILogic
    {
        [SerializeField]
        private string m_Name = "NICE";
    }
    public interface ILogic
    {
    }
}
