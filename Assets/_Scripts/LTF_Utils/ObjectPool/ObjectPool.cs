using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTF.ObjectPool
{
    [Serializable]
    public class ObjectPool<T> : IDisposable where T : Component
    {
        [SerializeField] private T _object;
        [SerializeField] private int _initialPoolSize;
        private Transform _poolParent;
        private readonly Queue<T> _inactiveObjects = new();

        public HashSet<T> Objects { get; private set; } = new();

        public Action<T> ObjectCreated { get; set; }

        public T Object => _object;
        public Transform PoolParent => _poolParent;

        public ObjectPool(T object_, int initialPoolSize)
        {
            _object = object_;
            _initialPoolSize = initialPoolSize;
        }

        public ObjectPool(T object_) : this(object_, 10) { }

        public ObjectPool(int initialPoolSize) : this(null, initialPoolSize) { }

        public ObjectPool() { }

        ~ObjectPool()
        {
            Dispose();
        }

        public void InitPool(int size, bool spawnActive = false)
        {
            if (_poolParent != null)
                return;

            var name = _object.name;

            _poolParent = new GameObject($"Pool_{name}").transform;

            _object = UnityEngine.Object.Instantiate(_object);
            _object.gameObject.SetActive(spawnActive);
            _object.name = name;
            _object.gameObject.hideFlags = HideFlags.HideAndDontSave;

            for (int i = 0; i < size; i++)
                _inactiveObjects.Enqueue(Instantiate());
        }

        public void InitPool(bool spawnActive = false) => InitPool(_initialPoolSize, spawnActive);  

        public T GetObject()
        {
            if (_inactiveObjects.Count > 0)
                return _inactiveObjects.Dequeue();

            return Instantiate();
        }

        public void ReturnObject(T object_)
        {
            _inactiveObjects.Enqueue(object_);
        }

        public void Dispose()
        {
            Objects.Clear();
            UnityEngine.Object.Destroy(_poolParent.gameObject);
        }

        private T Instantiate()
        {
            T object_ = UnityEngine.Object.Instantiate(_object, _poolParent);
            object_.name = $"{_object.name}_{Objects.Count}";
            Objects.Add(object_);
            ObjectCreated?.Invoke(object_);
            return object_;
        }
    }
}