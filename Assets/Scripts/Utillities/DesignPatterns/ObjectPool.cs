using System;
using System.Collections.Concurrent;

namespace WizardParty.Patterns
{
    public interface IPoolable : IRecycleable
    {
        void EnterPool();
    }
    public interface IRecycleable
    {
        void Recycle();
    }
    public class ObjectPool<T> where T : IPoolable
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _constructor;
        public ObjectPool(Func<T> constructor)
        {
            _constructor = constructor ?? throw new NullReferenceException();
            _objects = new ConcurrentBag<T>();
        }

        public T Get()
            => _objects.TryTake(out T item) ? item : _constructor();

        public void Discard(T item)
        {
            item.Recycle();
            _objects.Add(item);
        }
    }
}
