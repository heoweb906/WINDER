using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbBugStateMachine : StateMachine
{
    public BulbBug bulBug;

    public BulBug_IDLEState IDLEState { get; private set; }
    public BulBug_WanderingState WanderingState { get; private set; }
    public BulBug_SleepState SleepState { get; private set; }
    public BulBug_StandUpState StandUpState { get; private set; }

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    



    public BulbBugStateMachine(BulbBug _bulBugM)
    {
        bulBug = _bulBugM;
        StateInit();
    }
    private void StateInit()
    {
        IDLEState = new BulBug_IDLEState(bulBug, this);
        WanderingState = new BulBug_WanderingState(bulBug, this);
        SleepState = new BulBug_SleepState(bulBug, this);
        StandUpState = new BulBug_StandUpState(bulBug, this);


        CurrentState = WanderingState;

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
