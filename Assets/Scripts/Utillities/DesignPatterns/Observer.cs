using System;
using System.Collections.Generic;

namespace WizardParty.Patterns
{
    public interface IObserver<T>
    {
        void DoAction(Action<T> action);
        void Subscribe(T subscriber);
        void Unsubscribe(T subscriber);
    }

    public class Observer<T> : IObserver<T>
    {
        List<T> _subscribers = new List<T>();
        Queue<(T subscriber, bool state)> _subChanges = new Queue<(T subscriber, bool state)>();
        TokenMachine _doingAction;

        Observer()
        {
            _doingAction = new(OnRelease: UpdateSubChanges);
        }

        #region Interface
        public void Subscribe(T subscriber)
        {
            if (_doingAction.Locked)
                _subChanges.Enqueue((subscriber, true));
            else
                Add(subscriber);
        }
        public void Unsubscribe(T subscriber)
        {
            if (_doingAction.Locked)
                _subChanges.Enqueue((subscriber, false));
            else
                Remove(subscriber);
        }
        public void DoAction(Action<T> action)
        {
            if (action == null)
                return;

            var token = _doingAction.GetToken();

            foreach (var subscriber in _subscribers)
                action.Invoke(subscriber);

            token.Release();
        }

        #endregion
        #region Internal

        private void UpdateSubChanges()
        {
            while (_subChanges.Count > 0)
            {
                var subChange = _subChanges.Dequeue();

                if (subChange.state)
                    Add(subChange.subscriber);
                else
                    Remove(subChange.subscriber);
            }
        }
        private void Remove(T subscriber)
        {
            _subscribers.Remove(subscriber);
        }
        private void Add(T subscriber)
        {
            if (!_subscribers.Contains(subscriber))
                _subscribers.Add(subscriber);
        }
        #endregion
    }
}
