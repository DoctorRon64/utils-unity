using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {
    /// <summary>
    /// Lightweight finite state machine with a shared blackboard object.
    /// </summary>
    public class StateMachine<T> {
        private readonly Dictionary<Type, IState<T>> allStates = new();

        public event Action<IState<T>> StateChanged;

        public IState<T> CurrentState { get; private set; }

        public T Blackboard { get; }

        public StateMachine(T blackboard) {
            Blackboard = blackboard;
        }

        public void OnFixedUpdate() => CurrentState?.OnFixedUpdate();

        public void OnUpdate() => CurrentState?.OnUpdate();

        public void Quit() {
            CurrentState?.OnExit();
            CurrentState = null;
        }

        public void Switch<TState>() where TState : IState<T> {
            TrySwitch<TState>();
        }

        public bool TrySwitch<TState>() where TState : IState<T> {
            if (!allStates.TryGetValue(typeof(TState), out IState<T> next)) {
                Debug.LogWarning($"State {typeof(TState).Name} has not been added to this state machine.");
                return false;
            }

            if (ReferenceEquals(CurrentState, next)) {
                return true;
            }

            CurrentState?.OnExit();
            CurrentState = next;
            StateChanged?.Invoke(CurrentState);
            next.OnEnter();
            return true;
        }

        public TState Add<TState>() where TState : IState<T>, new() {
            TState state = new();
            Add(state);
            return state;
        }

        public void Add<TState>(TState state) where TState : IState<T> {
            if (state == null) {
                throw new ArgumentNullException(nameof(state));
            }

            Type type = typeof(TState);
            if (allStates.ContainsKey(type)) {
                return;
            }

            allStates.Add(type, state);
            state.SetOwner(this);
        }

        public bool RemoveState<TState>() where TState : IState<T> {
            return allStates.Remove(typeof(TState));
        }
    }

    public interface IState<T> {
        void SetOwner(StateMachine<T> owner);
        void OnEnter();
        void OnExit();
        void OnUpdate();
        void OnFixedUpdate();
    }

    public abstract class BaseState<T> : IState<T> {
        public StateMachine<T> Owner { get; private set; }

        protected T Blackboard => Owner.Blackboard;

        public void SetOwner(StateMachine<T> owner) {
            Owner = owner;
        }

        public virtual void OnEnter() {
        }

        public virtual void OnExit() {
        }

        public virtual void OnUpdate() {
        }

        public virtual void OnFixedUpdate() {
        }
    }
}
