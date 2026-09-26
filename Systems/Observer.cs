using System;
using System.Collections.Generic;

namespace Utility {
    public sealed class Observer {
        private readonly List<Action> removers = new();

        public void Add(Signal signal, Action callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add(SignalAsset signal, Action callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add(SignalSender signal, Action callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add(SignalSender signal, Action<object> callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add<T>(Signal<T> signal, Action<T> callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add<T>(SignalAsset<T> signal, Action<T> callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add<T>(Signal<T> signal, EventHandler<T> callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add<T, TU>(Signal<T, TU> signal, Action<T, TU> callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Add<T, TU>(DoubleDataSignalAsset<T, TU> signal, Action<T, TU> callback) {
            if (signal == null || callback == null) return;

            signal.AddListener(callback);
            removers.Add(() => signal.RemoveListener(callback));
        }

        public void Clear() {
            for (int i = removers.Count - 1; i >= 0; i--) {
                removers[i].Invoke();
            }

            removers.Clear();
        }
    }
}
