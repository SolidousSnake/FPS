using System;
using System.Collections.Generic;

namespace _Project.Code.Runtime.Core.States
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _registeredStates;
        private IState _activeState;
        private IState _previousState;
        private IUpdateableState _updateableState;
        
        public StateMachine()
        {
            _registeredStates = new Dictionary<Type, IState>();
        }
        
        public void RegisterState(IState state) => _registeredStates.Add(state.GetType(), state);

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

        public void Update() => _updateableState?.Update();
        
        private void ChangeState(Type stateType)
        {
            _activeState?.Exit();

            IState newState = _registeredStates[stateType];
            _activeState = newState;
            _activeState.Enter();

            _updateableState = _activeState as IUpdateableState;
        }
        
        private T GetState<T>() where T : class, IState => _registeredStates[typeof(T)] as T;
    }
}