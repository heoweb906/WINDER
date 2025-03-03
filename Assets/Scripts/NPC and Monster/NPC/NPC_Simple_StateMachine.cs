using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_StateMachine : StateMachine
{
    public NPC_Simple npc;

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    public NPC_Simple_IDLEState IDLEState { get; private set; }
    public NPC_Simple_WalkState WalkState { get; private set; }
    public NPC_Simple_GrappedState GrappedState { get; private set; }
    public NPC_Simple_ReactionThankState ThankState { get; private set; }


    // #. 특정한 행동을 취하고 있는 NPC들
    public NPC_Simple_ActionEvent ActionEventState { get; private set; }

    public NPC_Simple_ActionEventSpinTaeYub SpinTaeYubState { get; private set; }
    public NPC_Simple_Action_TextingSmartPhone TextingSmartPhoneState { get; private set; }




    public NPC_Simple_StateMachine(NPC_Simple _npc)
    {
        npc = _npc;
        StateInit();
    }
    private void StateInit()
    {
        IDLEState = new NPC_Simple_IDLEState(npc, this);
        WalkState = new NPC_Simple_WalkState(npc, this);
        GrappedState = new NPC_Simple_GrappedState(npc, this);
        ThankState = new NPC_Simple_ReactionThankState(npc, this);

        ActionEventState = new NPC_Simple_ActionEvent(npc, this);
        SpinTaeYubState = new NPC_Simple_ActionEventSpinTaeYub(npc, this);
        TextingSmartPhoneState = new NPC_Simple_Action_TextingSmartPhone(npc, this);


        CurrentState = npc.bWalking ?  WalkState : IDLEState;

        if(npc.bActionEventNPC)
        {
            CurrentState = ActionEventState;
        }
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
