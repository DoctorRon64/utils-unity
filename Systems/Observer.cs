using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public sealed class Observer {
    private readonly List<Action> m_removers = new();

    public void Add(ISignal _signal, Action _callback) =>
        Listen(_signal, _callback, static (_signal, _callback) => _signal.AddListener(_callback), static (_signal, _callback) => _signal.RemoveListener(_callback));

    public void Add<T>(ISignal<T> _signal, Action<T> _callback) =>
        Listen(_signal, _callback, static (_signal, _callback) => _signal.AddListener(_callback), static (_signal, _callback) => _signal.RemoveListener(_callback));

    public void Add<T, U>(ISignal<T, U> _signal, Action<T, U> _callback) =>
        Listen(_signal, _callback, static (_signal, _callback) => _signal.AddListener(_callback), static (_signal, _callback) => _signal.RemoveListener(_callback));

    public void Clear() {
        if (m_removers.Count == 0)
            return;

        Action[] snapshot = m_removers.ToArray();
        m_removers.Clear();

        for (int i = snapshot.Length - 1; i >= 0; i--) {
            try {
                snapshot[i].Invoke();
            } catch (Exception exception) {
                Debug.LogException(exception);
            }
        }
    }

    private void Listen<TSignal, TCallback>(
        TSignal _signal,
        TCallback _callback,
        Action<TSignal, TCallback> _add,
        Action<TSignal, TCallback> _remove) {
        if (_signal == null || _callback == null)
            return;

        _add(_signal, _callback);
        m_removers.Add(() => _remove(_signal, _callback));
    }
}