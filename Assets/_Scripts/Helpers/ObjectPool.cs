using System.Collections.Generic;
using UnityEngine;

namespace LTFUtils
{
    [System.Serializable]
    public class ObjectPool<T> where T : MonoBehaviour
    {
        [SerializeField] private T _object;
        [SerializeField] private int _initialPoolSize;
        private readonly Queue<T> _inactiveObjects = new();
        public HashSet<T> Objects { get; private set; } = new();
        private GameObject _poolParent;

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

        public void InitPool(int size)
        {
            if (_poolParent != null)
                return;

            _poolParent = new("Pool_" + _object.name);

            for (int i = 0; i < size; i++)
            {
                T t = Instantiate();
                _inactiveObjects.Enqueue(t);
            }
        }

        public void InitPool() => InitPool(_initialPoolSize);

        public T GetObject()
        {
            T object_;

            if (_inactiveObjects.Count > 0)
            {
                object_ = _inactiveObjects.Dequeue();
            }
            else
            {
                object_ = Instantiate();
            }

            //object_.gameObject.SetActive(true);
            return object_;
        }

        private T Instantiate()
        {
            T object_ = GameObject.Instantiate(_object);
            object_.gameObject.SetActive(false);
            object_.transform.SetParent(_poolParent.transform);
            ObjectCreated?.Invoke(object_);
            object_.name += _inactiveObjects.Count;
            Objects.Add(object_);
            return object_;
        }

        public void ReturnObject(T object_)
        {
            //object_.gameObject.SetActive(false);
            _inactiveObjects.Enqueue(object_);
        }

        ~ObjectPool()
        {
            Destroy();
        }

        public void Destroy()
        {
            Objects.Clear();
            GameObject.Destroy(_poolParent);
        }
    }
}