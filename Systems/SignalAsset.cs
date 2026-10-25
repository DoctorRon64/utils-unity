using System;
using UnityEngine;

namespace Utility {
    public class SignalAsset : DataAsset {
        private Signal m_signal;
        private Signal signal => m_signal ??= new Signal();

        public void AddListener(Action _callback) => signal.AddListener(_callback);
        public void RemoveListener(Action _callback) => signal.RemoveListener(_callback);
        public void Clear() => m_signal?.Clear();
        public void Invoke() => m_signal?.Invoke();

        protected void OnDisable() => Clear();
    }

    public abstract class SignalAsset<T> : DataAsset, ISignal<T> {
        private Signal<T> m_signal;
        private Signal<T> Signal => m_signal ??= new Signal<T>();

        public void AddListener(Action<T> _callback) => Signal.AddListener(_callback);
        public void RemoveListener(Action<T> _callback) => m_signal?.RemoveListener(_callback);
        public void Clear() => m_signal?.Clear();
        public void Invoke(T _value) => m_signal?.Invoke(_value);

        protected void OnDisable() => Clear();
    }
}