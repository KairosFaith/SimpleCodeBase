using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class ISimpleFSM : MonoBehaviour
{
    Dictionary<StateType, ISimpleState> _states = new Dictionary<StateType, ISimpleState>();
    protected ISimpleState CurrentState { get; private set; }
    StateType _CurrentStateKey;
    protected void RegisterState(ISimpleState stateObj)
    {
        _states.Add(stateObj.ClassStateType, stateObj);
    }
    public void SetState(StateType stateToSet, params object[] args)
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
    public void SendControlMessage(MessageType messageType, params object[] args)
    {
        CurrentState.GetControlMessage(messageType, args);
    }
}
public enum StateType
{

}
public abstract class ISimpleState
{
    public abstract StateType ClassStateType { get; }
    public ISimpleState(ISimpleFSM host) { }
    public abstract void OnStateEnter(StateType previousState, params object[] args);
    public abstract void OnStateExit(StateType nextState, params object[] args);
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
    public abstract void GetControlMessage(MessageType messageType, params object[] args);
}
public enum MessageType
{

}