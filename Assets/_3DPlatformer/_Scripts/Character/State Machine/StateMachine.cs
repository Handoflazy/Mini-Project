using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancePlayerController.State_Machine
{
    public class StateMachine
    {
        private StateNode _current;
        private Dictionary<Type, StateNode> _nodes = new();
        HashSet<ITransition> anyTransitions = new();
        public IState CurrentState => _current.State;

        public event Action<IState> OnStateChange;

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            _current.State?.Update();
        }

        public void FixedUpdate()
        {
            _current.State?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            _current = _nodes[state.GetType()];
            _current.State?.OnEnter();
        }
        

        void ChangeState(IState state)
        {
            if (state == _current.State)
                return;
            var previousState = _current.State;
            var nextState = _nodes[state.GetType()].State;
            
            previousState?.OnExit();
            nextState?.OnEnter();
            _current = _nodes[state.GetType()];
            OnStateChange?.Invoke(_current.State);
        }
        ITransition GetTransition() {
            foreach (var transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in _current.Transitions) {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        private StateNode GetOrAddNode(IState state)
        {
            if (!_nodes.ContainsKey(state.GetType()))
                _nodes.Add(state.GetType(),new StateNode(state));

            return _nodes[state.GetType()];
        }

        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }
            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }
            public void AddTransition(IState to, IPredicate condition) => Transitions.Add((new Transition(to, condition)));
        }
    }

}