using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class ISimpleFSM : MonoBehaviour
{
    protected enum StateType
    {
        //Project Specific
    }
    public enum MessageType
    {
        //Project Specific
    }
    Dictionary<StateType, ISimpleState> _states = new Dictionary<StateType, ISimpleState>();
    protected ISimpleState CurrentState { get; private set; }
    StateType _CurrentStateKey;
    protected void RegisterState(ISimpleState stateObj)
    {
        _states.Add(stateObj.ClassStateType, stateObj);
    }
    protected void SetState(StateType stateToSet, params object[] args)
    {
        if (_states.TryGetValue(stateToSet, out ISimpleState stateObj))
        {
            if (CurrentState != null)
                CurrentState.OnStateExit(stateToSet, args);
            stateObj.OnStateEnter(_CurrentStateKey, args);
            CurrentState = stateObj;
            _CurrentStateKey = stateToSet;
        }
        else
            throw new Exception(stateToSet + " state class not found!");
    }
    public virtual void SendControlMessage(MessageType messageType, params object[] args)
    {
        CurrentState.GetControlMessage(messageType, args);
    }
    protected abstract class ISimpleState
    {
        protected ISimpleState(ISimpleFSM host) { }
        public abstract StateType ClassStateType { get; }
        public abstract void OnStateEnter(StateType previousState, params object[] args);
        public abstract void OnStateExit(StateType nextState, params object[] args);
        public abstract void OnStateUpdate();
        public abstract void OnStateFixedUpdate();
        public abstract void GetControlMessage(MessageType messageType, params object[] args);
    }
}