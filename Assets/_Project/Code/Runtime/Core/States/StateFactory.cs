using Zenject;

namespace _Project.Code.Runtime.Core.States
{
    public class StateFactory
    {
        [Inject] private IInstantiator _container;

        public TState Create<TState>() where TState : IState => 
            _container.Instantiate<TState>();   
    }
}