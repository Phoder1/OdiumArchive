using System;
using System.Threading;
using System.Threading.Tasks;

namespace WizardParty.Async
{
    public static class TaskHelper
    {
        public static async Task WaitUntil(Func<bool> predicate, int checkDelayInMilliSeconds, CancellationToken token)
        {
            if (predicate == null || checkDelayInMilliSeconds == 0 || token == null)
                throw new NullReferenceException();

            while (!predicate.Invoke())
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(checkDelayInMilliSeconds);
            }
        }

        public static void ThrowIfNotValid(this CancellationToken token)
        {
            if (token == null)
                throw new NullReferenceException();

            token.ThrowIfCancellationRequested();
        }
    }
}
