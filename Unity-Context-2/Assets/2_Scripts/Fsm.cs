using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// The generic is the owner/blackboard
public class Fsm<T>
{
    public BaseState<T> currentState;
    
    private Dictionary<System.Type, BaseState<T>> states = new Dictionary<System.Type, BaseState<T>>(); 

    public Fsm(T owner, params BaseState<T>[] newStates)
    {
        foreach (BaseState<T> state in newStates)
        {
            state.SetOwner(this, owner);
            states.Add(state.GetType(), state);
            state.OnAwake();
        }
    }

    public void SwitchState(System.Type newState)
    {
        currentState?.OnExit();
        currentState = states[newState];
        currentState?.OnStart();
    }

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }
}

public abstract class BaseState<T>
{
    protected Fsm<T> fsm;
    protected T owner;

    public void SetOwner(Fsm<T> fsm, T npc)
    {
        this.fsm = fsm;
        this.owner = npc;
    }
    public virtual void OnAwake() {}
    public virtual void OnStart() {}
    public virtual void OnUpdate() {}
    public virtual void OnExit() {}
}