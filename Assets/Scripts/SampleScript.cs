using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WizardParty
{
    public class SampleScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.DOMove(transform.position + Vector3.up * 3, 1);
        }
    }
}
