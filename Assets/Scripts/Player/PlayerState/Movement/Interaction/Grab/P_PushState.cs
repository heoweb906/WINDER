using UnityEngine;

public class P_PushState : P_GrabState
{
    public P_PushState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.PushParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.PushParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.curDirection == Vector3.zero)
            machine.OnStateChange(machine.GrabIdleState);
    }

    public override void PlayerRotationControll()
    {
        if (player.curDirection != Vector3.zero)
        {
            // 밀 때는 이동 방향을 바라보도록
            Quaternion targetRotation = Quaternion.LookRotation(player.curDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.rotateLerpSpeed * Time.fixedDeltaTime);
        }
    }
}
