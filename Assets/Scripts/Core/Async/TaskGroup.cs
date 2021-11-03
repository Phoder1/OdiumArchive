using System;
using System.Threading;
using System.Threading.Tasks;

namespace WizardParty.Async
{
    public class TaskGroup : IDisposable
    {
        private static event Action OnDisposeAll;
        private CancellationTokenSource _tokensSource = new CancellationTokenSource();
        public CancellationToken Token => _tokensSource.Token;

        bool _disposed = false;

        public TaskGroup()
        {
            OnDisposeAll += Dispose;
        }
        public static void DisposeAll()
        {
            OnDisposeAll?.Invoke();
        }
        public Task StartTask(TaskFunc taskFunc, CancellationToken? cancellationToken = null)
        {
            if (taskFunc == null)
                return null;

            CancellationToken token;
            if (cancellationToken.HasValue)
            {
                cancellationToken.Value.ThrowIfCancellationRequested();
                token = Token.Combine(cancellationToken.Value);
            }
            else
            {
                token = Token;
            }

            return taskFunc.Invoke(token);
        }

        public void CancelAllTasks()
        {
            using (_tokensSource)
            {
                _tokensSource.Cancel();
            }
            _tokensSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            using (_tokensSource)
            {
                _tokensSource.Cancel();
            }
            _tokensSource = null;
            _disposed = true;
        }

        ~TaskGroup()
        {
            Dispose();
        }
    }
}
