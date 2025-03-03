using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyungsoo_StateMachine : StateMachine
{
    public Kyungsoo kyungsoo;

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    public Kyungsoo_IDLEState IDLEState { get; private set; }
    public Kyungsoo_WALKState WALKState { get; private set; }



    public Kyungsoo_StateMachine(Kyungsoo _kyungsoo)
    {
        kyungsoo = _kyungsoo;
        StateInit();
    }
    private void StateInit()
    {
        IDLEState = new Kyungsoo_IDLEState(kyungsoo, this);
        WALKState = new Kyungsoo_WALKState(kyungsoo, this);


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
