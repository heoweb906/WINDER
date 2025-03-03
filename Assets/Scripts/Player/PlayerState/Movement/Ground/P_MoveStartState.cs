
using UnityEngine;
public class P_MoveStartState : P_GroundState
{
    public P_MoveStartState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.MoveStartParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.MoveStartParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        if (machine.CheckCurrentState(machine.WalkStartState))
        {
            machine.OnStateChange(machine.WalkingState);

        }
        else if (machine.CheckCurrentState(machine.RunningState))
        {
            machine.OnStateChange(machine.RunningState);
        }
    }
}
