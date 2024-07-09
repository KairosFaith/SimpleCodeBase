using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class ISimpleFSM : MonoBehaviour
{
    Dictionary<StateKey, ISimpleState> _states = new Dictionary<StateKey, ISimpleState>();
    protected ISimpleState CurrentState { get; private set; }
    StateKey _CurrentStateKey = StateKey.Null;
    public StateKey State
    {
        get { return _CurrentStateKey; }
    }
    protected void RegisterState(ISimpleState stateObj)
    {
        StateKey keyToRegister = stateObj.Key;
        _states.Add(keyToRegister, stateObj);
        //bool keyIsValid = keyToRegister != StateKey.Null;
        //if (keyIsValid && _states.TryAdd(keyToRegister, stateObj))
        //    return;
        //else
        //    throw new Exception(keyToRegister + " already added!");
    }
    protected void SetState(StateKey stateKeyToSet, params object[] args)
    {
        if (stateKeyToSet != StateKey.Null)
        {
            if (_states.TryGetValue(stateKeyToSet, out ISimpleState stateObj))
            {
                if (CurrentState != null)
                    CurrentState.OnStateExit(stateKeyToSet, args);
                stateObj.OnStateEnter(_CurrentStateKey, args);
                CurrentState = stateObj;
                _CurrentStateKey = stateKeyToSet;
            }
            else
                throw new Exception(stateKeyToSet + " not found!");
        }
        else 
            throw new Exception("Do not change the state to Null");
    }
    public void SendMessage(MessageType messageType, params object[] args)
    {
        CurrentState.GetMessage(messageType, args);
    }
}
public enum StateKey
{
    Null,
    Roll,
    Walk,
    Interact,
    Dead
}
public abstract class ISimpleState
{
    public abstract StateKey Key { get; }
    public ISimpleFSM Host;
    public ISimpleState(ISimpleFSM host)
    {
        Host = host;
    }
    public abstract void OnStateEnter(StateKey previousState, params object[] args);
    public abstract void OnStateExit(StateKey nextState, params object[] args);
    public abstract void OnStateUpdate();
    public abstract void GetMessage(MessageType messageType, params object[] args);
}
public enum MessageType
{
    Move,
    Jump,
    Interact
}