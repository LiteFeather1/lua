using System;
using UnityEngine;

namespace StateMachine
{
    public abstract class State : MonoBehaviour
    {
        [Header("Base State")]
        [SerializeField] protected StateMachine _core;
        protected bool _completed;
        [ReadOnly][SerializeField] protected string _completedMessage;
        protected float _startTime;
        [ReadOnly][SerializeField] protected State _parentState;
        [ReadOnly][SerializeField] protected State _childState;

        public StateMachine Core => _core;
        protected float StateTime => Time.time - _startTime;
        public bool Completed => _completed;

        public Action OnStateEnter { get; set; }
        public Action OnStateComplete { get; set; }
        public Action OnStateExit { get; set; }

        /// <summary>
        /// Called when entering a new state, sets complete to false, updates start time and erases complete Message if state has a child state it initializes it
        /// </summary>
        public virtual void Enter()
        {
            _completed = false;
            _startTime = Time.time;
            _completedMessage = "";
            OnStateEnter?.Invoke();
        }

        /// <summary>
        /// Called by the state machine on update, runs child Do if child not null
        /// </summary>
        public virtual void Do() => _childState?.Do();

        /// <summary>
        /// Called by the state machine on Fixed Update,  runs child FixedDo if child not null
        /// </summary>
        public virtual void FixedDo() => _childState?.FixedDo();

        public virtual void Exit()
        {
            OnStateExit?.Invoke();
            DisableChildState("Parent Exited");
        }

        /// <summary>
        /// Called when a parent state completes it state if it has a child state
        /// </summary>
        protected void DisableChildState(string message)
        {
            if (_childState == null)
                return;

            _childState.Exit();
            _childState.CompleteState(message);
            _childState = null;
        }

        /// <summary>
        /// Called  when a parent state sets a child state
        /// </summary>
        protected void SetParentState(State newParentState) => _parentState = newParentState;

        /// <summary>
        /// Called to set a child state, also sets self to parent of set child
        /// </summary>
        /// <param name="childState"></param>
        protected void SetChildState(State childState)
        {
            _childState?.Exit();
            _childState = childState;
            if (_childState == null)
                return;

            _childState.Enter();
            _childState.SetParentState(this);
        }

        /// <summary>
        /// Used to exit a state, completing it and giving a complete message, also completes a child state if not null
        /// </summary>
        /// <param name="completeMessage"></param>
        public void CompleteState(string completeMessage)
        {
            if (_completed)
                return;
                
            _completed = true;
            _completedMessage = completeMessage;
            OnStateComplete?.Invoke();
            DisableChildState("Parent state completed");
        }
    }
}
