using System;
using System.Collections.Generic;

namespace Utility {
    /// <summary>
    /// Base class that stores delegates and provides core listener management.
    /// </summary>
    public abstract class SignalBase {
        protected readonly List<Delegate> listeners = new();

        internal void AddListener(Delegate listener) {
            if (listener != null && !listeners.Contains(listener)) {
                listeners.Add(listener);
            }
        }

        internal void RemoveListener(Delegate listener) {
            if (listener != null) {
                listeners.Remove(listener);
            }
        }

        public void Clear() => listeners.Clear();
    }

    public class Signal : SignalBase {
        public void AddListener(Action callback) {
            AddListener((Delegate)callback);
        }

        public void RemoveListener(Action callback) {
            RemoveListener((Delegate)callback);
        }

        public void Invoke() {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                (listener as Action)?.Invoke();
            }
        }
    }

    public class SignalSender : SignalBase {
        public void AddListener(Action callback) => AddListener((Delegate)callback);

        public void AddListener(Action<object> callback) => AddListener((Delegate)callback);

        public void RemoveListener(Action callback) => RemoveListener((Delegate)callback);

        public void RemoveListener(Action<object> callback) => RemoveListener((Delegate)callback);

        public void Invoke(object sender) {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                switch (listener) {
                    case Action action:
                        action.Invoke();
                        break;
                    case Action<object> senderAction:
                        senderAction.Invoke(sender);
                        break;
                }
            }
        }
    }

    public class Signal<T> : SignalBase {
        public void AddListener(Action<T> callback) {
            AddListener((Delegate)callback);
        }

        public void AddListener(EventHandler<T> callback) {
            AddListener((Delegate)callback);
        }

        public void RemoveListener(Action<T> callback) {
            RemoveListener((Delegate)callback);
        }

        public void RemoveListener(EventHandler<T> callback) {
            RemoveListener((Delegate)callback);
        }

        public void Invoke(T arg) {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                (listener as Action<T>)?.Invoke(arg);
            }
        }

        public void Invoke(object sender, T arg) {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                (listener as EventHandler<T>)?.Invoke(sender, arg);
            }
        }
    }

    public class Signal<T, U> : SignalBase {
        public void AddListener(Action<T, U> callback) => AddListener((Delegate)callback);

        public void RemoveListener(Action<T, U> callback) => RemoveListener((Delegate)callback);

        public void Invoke(T arg1, U arg2) {
            Delegate[] snapshot = listeners.ToArray();
            foreach (Delegate listener in snapshot) {
                (listener as Action<T, U>)?.Invoke(arg1, arg2);
            }
        }
    }
}
