using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBmachine : StateMachine
{
    public BulbBug_OnRoad bulBug;

    public BulBugOnRoad_IDLEState IDLEState { get; private set; }
    public BulBugOnRoad_RunState RunState { get; private set; }
    
    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }


    public BBBmachine(BulbBug_OnRoad _bulBugM)
    {
        bulBug = _bulBugM;
        StateInit();
    }
    private void StateInit()
    {
        IDLEState = new BulBugOnRoad_IDLEState(bulBug, this);
        RunState = new BulBugOnRoad_RunState(bulBug, this);
      
        CurrentState = IDLEState;

        CurrentState.OnEnter();
    }

    public override void OnStateUpdate()
    {
        CurrentState.OnUpdate();
    }

    public override void OnStateFixedUpdate()
    {
        CurrentState.OnFixedUpdate();
    }


    public void OnStateChange(BaseState _nextState)
    {
        if (CurrentState == _nextState)
        {
            return;
        }
        PreState = CurrentState;
        CurrentState.OnExit();
        CurrentState = _nextState;
        CurrentState.OnEnter();
    }

    public bool CheckCurrentState(BaseState _state)
    {
        return CurrentState == _state;
    }

    public bool CheckPreState(BaseState _state)
    {
        return PreState == _state;
    }








    public void StartAnimation(int _parametgerHash)
    {

    }

    public void StopAnimation(int _parametgerHash)
    {

    }

    public void OnAnimationEnterEvent()
    {
        CurrentState?.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        CurrentState?.OnAnimationExitEvent();
    }

    public void OnAnimationTransitionEvent()
    {
        CurrentState?.OnAnimationTransitionEvent();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        CurrentState?.OnTriggerEnter(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        CurrentState?.OnTriggerStay(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        CurrentState?.OnTriggerExit(other);
    }
}
