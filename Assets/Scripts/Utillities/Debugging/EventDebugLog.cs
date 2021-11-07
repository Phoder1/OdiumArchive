using Sirenix.OdinInspector;
using UnityEngine;

namespace WizardParty.Debugging
{
    [HideMonoScript]
    public class EventDebugLog : MonoBehaviour
    {
        public void DebugLog(string message) => Debug.Log(message);
    }
}
