using System;
using System.Collections.Generic;

namespace WizardParty.Patterns
{
    public class TokenMachine
    {
        private readonly List<Token> _tokens = new();
        private readonly ObjectPool<Token> _tokenPool = new(() => new());
        public Token CurrentToken
        {
            get
            {
                if (_tokens.Count == 0)
                    return null;

                var token = _tokens[^1];
                if (!IsValid(token))
                {
                    _tokens.RemoveAt(_tokens.Count - 1);
                    return CurrentToken;
                }

                return token;
            }
        }
        public int TokenCount => _tokens.Count;
        public bool Locked => TokenCount > 0;
        public bool Released => TokenCount == 0;

        public TokenMachine(Action OnLock = null, Action OnRelease = null)
        {
            if (OnLock != null)
                this.OnLock += OnLock;

            if (OnRelease != null)
                this.OnRelease += OnRelease;
        }

#pragma warning disable
        public event Action OnLock;
        public event Action OnRelease;
#pragma warning enable

        public Token GetToken()
        {
            Token newToken = _tokenPool.Get();
            AddToken(newToken);
            return newToken;
        }
        private static bool IsValid(Token token)
            => token != null && !token.Released;
        private void AddToken(Token newToken)
        {
            if (_tokens.Contains(newToken))
                return;

            _tokens.Add(newToken);

            if (_tokens.Count == 1)
                OnLock?.Invoke();

            newToken.OnRelease += TokenReleased;

            void TokenReleased(Token releasedToken)
            {
                int index = _tokens.FindIndex((x) => x == releasedToken);

                if (index == -1)
                    return;

                _tokens.RemoveAt(index);

                _tokenPool.Discard(releasedToken);

                if (_tokens.Count == 0)
                    OnRelease?.Invoke();
            }
        }
    }
    public sealed class Token : IDisposable, IPoolable
    {
        private bool _released = false;
        public bool Released => _released;
        /// <summary>
        /// When the Check-In count reaches 0.
        /// </summary>
        public event Action<Token> OnRelease = default;
        ~Token()
        {
            Dispose();
        }
        public void Release()
        {
            if (_released)
                return;

            _released = true;
            OnRelease?.Invoke(this);
        }
        public void Dispose()
        {
            Release();
            OnRelease = null;
        }

        public void Recycle()
        {
            _released = false;
        }

        public void EnterPool()
        {
            Dispose();
        }
    }
}
