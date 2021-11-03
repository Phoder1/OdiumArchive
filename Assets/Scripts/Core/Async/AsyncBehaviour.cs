using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardParty.Async
{
    public abstract class AsyncBehaviour : MonoBehaviour
    {
        protected TaskGroup taskGroup = new TaskGroup();
        public CancellationToken Token => taskGroup.Token;
        public void CancelAllTasks() => taskGroup.CancelAllTasks();
        public Task StartTask(TaskFunc taskFunc, CancellationToken? cancellationToken = null)
            => taskGroup.StartTask(taskFunc, cancellationToken);
        private void OnDestroy()
        {
            taskGroup.Dispose();
        }
    }
}
