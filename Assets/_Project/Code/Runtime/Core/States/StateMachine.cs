using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Code.Runtime.Core.States
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _registeredStates;
        private IState _activeState;
        private IState _previousState;
        private IDefaultState _defaultState;
        private IUpdateableState _updateableState;
        
        public StateMachine()
        {
            _registeredStates = new Dictionary<Type, IState>();
        }

        public IState ActiveState => _activeState;
        
        public void RegisterState(IState state)
        {
            if (state is IDefaultState defaultState) 
                _defaultState = defaultState;

            _registeredStates.Add(state.GetType(), state);
        }

        public void Enter<T>() where T : class, IState
        {
            if (_activeState is T)
                return;

            _previousState = _activeState;
            ChangeState(typeof(T));
        }

        public void EnterPreviousState()
        {
            if (_previousState == null)
                return;

            ChangeState(_previousState.GetType());
        }

        public void EnterDefaultState()
        {
            if (_defaultState == null)
                return;

            ChangeState(_defaultState.GetType());
        }

        public void Update() => _updateableState?.Update();
        
        private void ChangeState(Type stateType)
        {
            _activeState?.Exit();

            IState newState = _registeredStates[stateType];
            _activeState = newState;
            _activeState.Enter();
            _updateableState = _activeState as IUpdateableState;
        }
    }
}