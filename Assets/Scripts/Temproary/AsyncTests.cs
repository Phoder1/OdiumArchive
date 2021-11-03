using Sirenix.OdinInspector;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardParty.Async
{
    public class AsyncTests : AsyncBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            await StartTask(LogSeconds);
            Debug.Log("Finished!");
        }
        [Button]
        private void Destroy()
            => Destroy(gameObject);
        private async Task LogSeconds(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfNotValid();

            int time = 0;
            Debug.Log("Started logging seconds!");

            try
            {
                while (true)
                {
                    await Task.Delay(1000, cancellationToken);
                    time++;
                    Debug.Log(time);
                }
            }
            catch (OperationCanceledException e) when (cancellationToken.IsCancellationRequested)
            {
                Debug.LogException(e);
            }
        }
    }
}
