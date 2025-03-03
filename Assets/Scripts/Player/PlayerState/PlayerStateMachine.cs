using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    protected Player player;

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    public P_GroundState GroundState { get; private set; }
    public P_OnAirState OnAirState { get; private set; }
    public P_InteractionState InteractionState { get; private set; }
    public P_ClimbingState ClimbingState { get; private set; }
    public P_UnControllable UnControllableState { get; private set; }
    public P_UC_Idle UC_IdleState { get; private set; }
    public P_UC_Die UC_DieState { get; private set; }

    public P_IdleState IdleState { get; private set; }
    public P_SoftLandingState SoftLandingState { get; private set; }
    public P_MoveLandingState MoveLandingState { get; private set; }
    public P_WalkStartState WalkStartState { get; private set; }
    public P_WalkingState WalkingState { get; private set; }
    public P_RunStartState RunStartState { get; private set; }
    public P_RunningState RunningState { get; private set; }
    public P_SoftStopState SoftStopState { get; private set; }
    public P_HardStopState HardStopState { get; private set; }

    public P_FallingState FallingState { get; private set; }
    public P_JumpStartState JumpStartState { get; private set; }

    public P_JumpStartIdleState JumpStartIdleState { get; private set; }
    public P_JumpStartMoveState JumpStartMoveState { get; private set; }
    public P_FallingIdleState FallingIdleState { get; private set; }
    public P_FallingMoveState FallingMoveState { get; private set; }

    public P_HangingState HangingState { get; private set; }
    public P_ClimbingToTopState ClimbingToTopState { get; private set; }

    public P_SpinClockWorkState SpinClockWorkState { get; private set; }
    public P_SpinClockWorkWallState SpinClockWorkWallState { get; private set; }
    public P_SpinClockWorkFloorState SpinClockWorkFloorState { get; private set; }
    public P_PickUpState PickUpState { get; private set; }
    public P_PutDownState PutDownState { get; private set; }
    public P_RemovePartsState RemovePartsState { get; private set; }
    public P_ThrowState ThrowState { get; private set; }
    public P_GuitarBrokenState GuitarBrokenState { get; private set; }
    public P_GrabState GrabState { get; private set; }
    public P_GrabIdleState GrabIdleState { get; private set; }
    public P_PushState PushState { get; private set; }
    public P_PullState PullState { get; private set; }

    public P_PutPartsState PutPartsState { get; private set; }

    public PlayerStateMachine(Player _player)
    {
        player = _player;
        StateInit();
    }

    private void StateInit()
    {
        GroundState = new P_GroundState(player, this);
        OnAirState = new P_OnAirState(player, this);
        InteractionState = new P_InteractionState(player, this);
        ClimbingState = new P_ClimbingState(player, this);
        UnControllableState = new P_UnControllable(player, this);
        UC_IdleState = new P_UC_Idle(player, this);
        UC_DieState = new P_UC_Die(player, this);

        IdleState = new P_IdleState(player, this);
        SoftLandingState = new P_SoftLandingState(player, this);
        MoveLandingState = new P_MoveLandingState(player, this);
        WalkStartState = new P_WalkStartState(player, this);
        WalkingState = new P_WalkingState(player, this);
        RunStartState = new P_RunStartState(player, this);
        RunningState = new P_RunningState(player, this);
        SoftStopState = new P_SoftStopState(player, this);
        HardStopState = new P_HardStopState(player, this);
        FallingState = new P_FallingState(player, this);
        JumpStartState = new P_JumpStartState(player, this);

        JumpStartIdleState = new P_JumpStartIdleState(player, this);
        JumpStartMoveState = new P_JumpStartMoveState(player, this);
        FallingIdleState = new P_FallingIdleState(player, this);
        FallingMoveState = new P_FallingMoveState(player, this);

        HangingState = new P_HangingState(player, this);
        ClimbingToTopState = new P_ClimbingToTopState(player, this);

        SpinClockWorkState = new P_SpinClockWorkState(player, this);
        SpinClockWorkWallState = new P_SpinClockWorkWallState(player, this);
        SpinClockWorkFloorState = new P_SpinClockWorkFloorState(player, this);
        PickUpState = new P_PickUpState(player, this);
        PutDownState = new P_PutDownState(player, this);
        RemovePartsState = new P_RemovePartsState(player, this);
        ThrowState = new P_ThrowState(player, this);
        GuitarBrokenState = new P_GuitarBrokenState(player, this);
        GrabState = new P_GrabState(player, this);
        GrabIdleState = new P_GrabIdleState(player, this);
        PushState = new P_PushState(player, this);
        PullState = new P_PullState(player, this);

        PutPartsState = new P_PutPartsState(player, this);

        CurrentState = IdleState;
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
        if(CurrentState == _nextState)
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
        player.playerAnim.SetBool(_parametgerHash, true);
    }

    public void StopAnimation(int _parametgerHash)
    {
        player.playerAnim.SetBool(_parametgerHash, false);
    }

    public void StartAnimationTrigger(int _parametgerHash)
    {
        player.playerAnim.SetTrigger(_parametgerHash);
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
