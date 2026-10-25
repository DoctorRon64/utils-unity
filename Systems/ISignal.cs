using System;

namespace Utility {
    public interface ISignal {
        void AddListener(Action _callback);
        void RemoveListener(Action _callback);
    }

    public interface ISignal<T> {
        void AddListener(Action<T> _callback);
        void RemoveListener(Action<T> _callback);
    }

    public interface ISignal<T, U> {
        void AddListener(Action<T, U> _callback);
        void RemoveListener(Action<T, U> _callback);
    }
}