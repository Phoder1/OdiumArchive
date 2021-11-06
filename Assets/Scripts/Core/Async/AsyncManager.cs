using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardParty.Async
{
    public delegate Task TaskFunc(CancellationToken token);
    public static class AsyncManager
    {
        private static TaskGroup GlobalTaskGroup = new TaskGroup();
        public static CancellationToken GlobalToken => GlobalTaskGroup.Token;
        private static TaskGroup SceneTaskGroup = new TaskGroup();
        public static CancellationToken SceneToken => SceneTaskGroup.Token;
        static AsyncManager()
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private static void SceneManager_sceneUnloaded(Scene arg0)
        {
            SceneTaskGroup.Dispose();
        }

        public static Task StartTaskGlobally(this TaskFunc taskFunc, CancellationToken? cancellationToken = null)
            => GlobalTaskGroup.StartTask(taskFunc, cancellationToken);
        public static Task StartTaskInScene(this TaskFunc taskFunc, CancellationToken? cancellationToken = null)
            => SceneTaskGroup.StartTask(taskFunc, cancellationToken);

        public static CancellationToken Combine(this CancellationToken lhs, CancellationToken rhs)
        {
            lhs.ThrowIfNotValid();

            rhs.ThrowIfNotValid();

            var newTokenSource = new CancellationTokenSource();
            var newToken = newTokenSource.Token;
            lhs.Register(TokenCancelled);
            rhs.Register(TokenCancelled);

            return newToken;

            void TokenCancelled()
            {
                newTokenSource.Cancel();
            }
        }
    }
}
