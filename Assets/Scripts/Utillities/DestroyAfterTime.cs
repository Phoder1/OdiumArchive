using System.Threading.Tasks;
using UnityEngine;
using WizardParty.AddressablesManagement;
using WizardParty.Async;

namespace WizardParty
{
    public class DestroyAfterTime : AsyncBehaviour
    {
        [SerializeField]
        private float _delay;

        private async void Awake()
        {
            try
            {
                await StartTask((x) => Task.Delay(Mathf.RoundToInt(_delay * 1000), x));
                Destroy(gameObject);
            }
            catch (TaskCanceledException) { }
        }

        protected override void OnDestroy()
        {
            gameObject.ReleaseInstance();
        }
    }
}
