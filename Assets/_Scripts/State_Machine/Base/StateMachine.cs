using UnityEngine;
using LTF;

namespace StateMachineCore
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected State _currentState;

        public State CurrentState => _currentState;
        public Vector2 Position => transform.position;

        /// <summary>
        /// Set a new state, can be used by the state machine or by the a state
        /// </summary>
        protected void Set(State nextState, bool _override = false)
        {
            if (_currentState != nextState || _override)
            {
                _currentState?.Exit();
                _currentState = nextState;
                _currentState?.Enter();
            }
        }

        protected virtual void Update()
        {
            if (_currentState != null)
            {
                if (_currentState.Completed)
                    SetNextState();

                _currentState?.Do();
            }
            else
                SetNextState();
        }

        protected virtual void FixedUpdate() => _currentState?.FixedDo();

        /// <summary>
        /// Called on base update
        /// If current state not null and current state completed it will call SetNextState
        /// </summary>
        public virtual void SetNextState() { }
    }
}
