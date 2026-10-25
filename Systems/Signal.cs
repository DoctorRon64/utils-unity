using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Utility {
    public readonly struct SignalSubscription : IDisposable {
        private readonly Action m_unsubscribe;
        public SignalSubscription(Action _unsubscribe) => this.m_unsubscribe = _unsubscribe;
        public void Dispose() => m_unsubscribe?.Invoke();
    }

    /// <summary>
    /// Base class that stores delegates and provides core listener management.
    /// </summary>
    public abstract class SignalBase {
        protected readonly List<Delegate> listeners = new();
        private Delegate[] m_invokeBuffers;
        private int m_dispatchDepth;

        protected void Register(Delegate _listener) {
            if (_listener == null || listeners.Contains(_listener)) {
                return;
            }
            listeners.Add(_listener);
        }

        protected void Unregister(Delegate _listener) {
            if (_listener != null) {
                listeners.Remove(_listener);
            }
        }

        public void Clear() => listeners.Clear();
        
        protected void Invoke(Action<Delegate> call) {
            int count = listeners.Count;
            if (count == 0) {
                return;
            }
            Delegate[] buffer;
            if (m_dispatchDepth == 0) {
                if (m_invokeBuffers == null || m_invokeBuffers.Length < count) {
                    int size = m_invokeBuffers?.Length ?? 8;
                    while (size < count) {
                        size *= 2;
                    }
                    m_invokeBuffers = new Delegate[size];
                }
                buffer = m_invokeBuffers;
            } else {
                buffer = new Delegate[count];
            }
            listeners.CopyTo(0, buffer, 0, count);
            m_dispatchDepth++;
            try {
                for (int i = 0; i < count; i++) {
                    Delegate listener = buffer[i];
                    buffer[i] = null;
                    if (listener.Target is Object owner && owner == null) {
                        listeners.Remove(listener);
                        continue;
                    }
                    try {
                        call(listener);
                    } catch (Exception exception) {
                        Debug.LogException(exception);
                    }
                }
            } finally {
                m_dispatchDepth--;
                if (m_dispatchDepth == 0)
                    Array.Clear(m_invokeBuffers, 0, m_invokeBuffers.Length);
            }
        }
    }
    
    public class Signal : SignalBase {
        public void AddListener(Action _callback) {
            Register(_callback);
        }

        public void RemoveListener(Action _callback) {
            Unregister(_callback);
        }

        public void Invoke() {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                (listener as Action)?.Invoke();
            }
        }
    }

    public sealed class EventSignal<T> : SignalBase {
        public void AddListener(EventHandler<T> _callback) => Register(_callback);
        public void RemoveListener(EventHandler<T> _callback) => Unregister(_callback);
        public void Invoke(object _sender, T _arg) => Invoke(_listener => ((EventHandler<T>)_listener).Invoke(_sender, _arg));
    }

    public sealed class Signal<T> : SignalBase {
        public void AddListener(Action<T> _callback) => Register(_callback);
        public void RemoveListener(Action<T> _callback) => Unregister(_callback);
        public SignalSubscription Subscribe(Action<T> _callback) {
            AddListener(_callback);
            return new SignalSubscription(() => RemoveListener(_callback));
        }
        public void Invoke(T _arg) => Invoke(_listener => ((Action<T>)_listener).Invoke(_arg));
    }

    public class Signal<T, U> : SignalBase {
        public void AddListener(Action<T, U> _callback) {
            Register(_callback);
        }

        public void RemoveListener(Action<T, U> _callback) {
            Unregister(_callback);
        }
        
        public void Invoke(T _arg1, U _arg2) {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                (listener as Action<T, U>)?.Invoke(_arg1, _arg2);
            }
        }
    }
}
