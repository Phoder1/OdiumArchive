using Sirenix.OdinInspector;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardParty.Async
{
    public class AsyncTests : AsyncBehaviour
    {
        private CancellationTokenSource _source = new();
        // Start is called before the first frame update
        [Button]
        void StartCount()
        {
            _ = AsyncManager.StartTaskGlobally(LogSeconds, _source.Token);
        }
        [Button]
        private void StopCount()
        {
            using (_source)
            {
                _source.Cancel();
            }
            _source = new();
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
