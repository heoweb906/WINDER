using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMStateMachine : StateMachine
{
    public GuardM guardM;

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    public GM_ReadyState ReadyState { get; private set; }
    public GM_ChaseState ChaseState { get; private set; }
    public GM_AttackState AttackState { get; private set; }
    public GM_BackHomeState BackHomeState { get; private set; }
    public GM_WanderingState WanderingState { get; private set; }
    


    public GuardMStateMachine(GuardM _guardM)
    {
        guardM = _guardM;
        StateInit();
    }
    private void StateInit()
    {
        ReadyState = new GM_ReadyState(guardM, this);
        ChaseState = new GM_ChaseState(guardM, this);
        AttackState = new GM_AttackState(guardM, this);
        BackHomeState = new GM_BackHomeState(guardM, this);
        WanderingState = new GM_WanderingState(guardM, this);


        // 경비병의 상태에 따라 초기 상태를 결정
        if (guardM.guardMType == GuardMType.Wandering) CurrentState = WanderingState;
        else CurrentState = ReadyState;

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
