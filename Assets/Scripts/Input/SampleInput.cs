using DG.Tweening;
using UnityEngine;

namespace WizardParty.Input
{
    public class SampleInput : DebugBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.DOMove(transform.position + Vector3.up * 3, 1).SetLoops(-1, LoopType.Yoyo);

#if UNITY_EDITOR
            if (_debuggingEnabled)
            {
                LogToFilter("Test", "Test");
            }
#endif
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (_debuggingEnabled)
            {
                Watch("Test", transform.position.y.ToString());
            }
#endif
        }
    }
}
