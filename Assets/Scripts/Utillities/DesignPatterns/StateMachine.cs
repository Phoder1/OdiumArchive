using System;
using System.Collections.Generic;

namespace WizardParty.Patterns
{
    public class StateMachine<T> : IEquatable<StateMachine<T>>, IState where T : IState
    {
        private T _State;

        public StateMachine(T currentState)
        {
            _State = currentState;
        }

        public T State
        {
            get => _State;
            set
            {
                if (_State.Equals(value))
                    return;

                if (_State != null)
                    _State.OnExit();

                _State = value;

                if (_State != null)
                    _State.OnEnter();
            }
        }

        public void OnEnter()
        {
            if (_State != null)
                _State.OnEnter();
        }

        public void OnExit()
        {
            if (_State != null)
                _State.OnExit();
        }
        public bool IsState(T state) => state.Equals(State);
        public static implicit operator T(StateMachine<T> machine) => machine.State;
        #region Equality
        public override bool Equals(object obj)
            => obj != null && obj is StateMachine<T> _machine && Equals(_machine);
        public bool Equals(StateMachine<T> other)
            => other != null && EqualityComparer<T>.Default.Equals(_State, other._State);
        public override int GetHashCode() =>  _State.GetHashCode();
        public static bool operator ==(StateMachine<T> left, StateMachine<T> right)
            => EqualityComparer<StateMachine<T>>.Default.Equals(left, right);
        public static bool operator !=(StateMachine<T> left, StateMachine<T> right)
            => !(left == right);
        #endregion
    }

    public interface IState
    {
        void OnEnter();
        void OnExit();
    }
}
