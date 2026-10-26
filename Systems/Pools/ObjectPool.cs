using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Utility {
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable {
        private readonly List<IPoolable> m_activePool = new();
        private readonly List<IPoolable> m_inactivePool = new();
        private readonly List<T> m_prefabs;
        private readonly Transform m_parent;
        private readonly bool m_expandWhenEmpty;

        public int ActiveCount => m_activePool.Count;
        public int InactiveCount => m_inactivePool.Count;
        public int Count => ActiveCount + InactiveCount;

        public ObjectPool(T prefab, int initialSize = 0, bool expandWhenEmpty = true, Transform parent = null)
            : this(new List<T> { prefab }, initialSize, expandWhenEmpty, parent) {
        }

        public ObjectPool(List<T> prefabs, int initialSize = 0, bool expandWhenEmpty = true, Transform parent = null) {
            if (prefabs == null || prefabs.Count == 0) {
                throw new ArgumentException("ObjectPool requires at least one prefab.", nameof(prefabs));
            }

            m_prefabs = prefabs;
            m_parent = parent;
            m_expandWhenEmpty = expandWhenEmpty;

            Prewarm(initialSize);
        }

        public void Prewarm(int count) {
            for (int i = 0; i < count; i++) {
                AddNewItemToPool();
            }
        }

        public IPoolable RequestObject(Vector2 position) {
            if (m_inactivePool.Count <= 0 && m_expandWhenEmpty) {
                AddNewItemToPool();
            }

            if (m_inactivePool.Count <= 0) {
                Debug.LogWarning("ObjectPool is empty. Increase the initial size or enable expandWhenEmpty.");
                return null;
            }

            IPoolable currentPool = m_inactivePool[0];
            currentPool.SetPosition(position);
            ActivateItem(currentPool);
            return currentPool;
        }

        public IPoolable AddNewItemToPool() {
            T prefab = m_prefabs[Random.Range(0, m_prefabs.Count)];
            T instance = Object.Instantiate(prefab, m_parent);
            instance.gameObject.SetActive(false);
            m_inactivePool.Add(instance);
            return instance;
        }

        public void DeactivateItem(IPoolable item) {
            if (item == null) return;

            m_activePool.Remove(item);

            if (!m_inactivePool.Contains(item)) {
                item.DisablePoolable();
                item.active = false;
                m_inactivePool.Add(item);
            }
        }

        private IPoolable ActivateItem(IPoolable item) {
            item.EnablePoolable();
            item.active = true;
            m_inactivePool.Remove(item);

            if (!m_activePool.Contains(item)) {
                m_activePool.Add(item);
            }

            return item;
        }
    }
}
