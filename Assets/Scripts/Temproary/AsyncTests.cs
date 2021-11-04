using Phoder1.AsyncManagement;
using Sirenix.OdinInspector;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardParty
{
    public class AsyncTests : AsyncBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            await AsyncManager.GlobalTaskGroup.StartTask(LogSeconds);
            TaskHelper.DebugAsyncLog("Finished!");
        }
        [Button]
        private void Destroy()
            => Destroy(gameObject);
        private async Task LogSeconds(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfNotValid();

            int time = 0;

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
                e.DebugAsyncLog();
            }
        }
    }
}
