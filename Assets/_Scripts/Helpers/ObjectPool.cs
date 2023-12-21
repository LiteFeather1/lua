using System.Collections.Generic;
using UnityEngine;

namespace LTFUtils
{
    [System.Serializable]
    public class ObjectPool<T> where T : Component
    {
        [SerializeField] private T _object;
        [SerializeField] private int _initialPoolSize;
        private GameObject _poolParent;
        private bool _spawnActive = false;
        private readonly Queue<T> _inactiveObjects = new();
        public HashSet<T> Objects { get; private set; } = new();

        public T Object => _object;
        public GameObject PoolParent => _poolParent;

        public System.Action<T> ObjectCreated { get; set; }

        public ObjectPool(T object_, int initialPoolSize)
        {
            _object = object_;
            _initialPoolSize = initialPoolSize;
        }

        public ObjectPool(T object_) : this(object_, 10) { }

        public ObjectPool(int initialPoolSize) : this(null, initialPoolSize) { }

        public ObjectPool() { }

        public void InitPool(int size, bool spawnActive = false)
        {
            if (_poolParent != null)
                return;

            _spawnActive = spawnActive;
            _poolParent = new($"Pool_{_object.name}");
            _object.gameObject.SetActive(_spawnActive); 
            for (int i = 0; i < size; i++)
            {
                T t = Instantiate();
                _inactiveObjects.Enqueue(t);
            }
            _object.gameObject.SetActive(true);
        }

        public void InitPool(bool spawnActive = false) => InitPool(_initialPoolSize, spawnActive);  

        public T GetObject()
        {
            T object_;

            if (_inactiveObjects.Count > 0)
            {
                object_ = _inactiveObjects.Dequeue();
            }
            else
            {
                _object.gameObject.SetActive(_spawnActive);
                object_ = Instantiate();
                _object.gameObject.SetActive(true);
            }

            return object_;
        }

        private T Instantiate()
        {
            T object_ = UnityEngine.Object.Instantiate(_object);
            object_.transform.SetParent(_poolParent.transform);
            object_.name += _inactiveObjects.Count;
            Objects.Add(object_);
            ObjectCreated?.Invoke(object_);
            return object_;
        }

        public void ReturnObject(T object_)
        {
            _inactiveObjects.Enqueue(object_);
        }

        ~ObjectPool()
        {
            Destroy();
        }

        public void Destroy()
        {
            Objects.Clear();
            UnityEngine.Object.Destroy(_poolParent);
        }
    }
}