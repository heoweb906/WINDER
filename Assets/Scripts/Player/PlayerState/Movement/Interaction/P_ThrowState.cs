using UnityEngine;
using DG.Tweening;

public class P_ThrowState : P_InteractionState
{
    public P_ThrowState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.ThrowParameterHash);

        player.SetCarryWeight();
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.ThrowParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.playerAnim.SetLayerWeight(1, player.carryWeight);
    }

    public override void OnAnimationTransitionEvent()
    {
        Rigidbody _rigid = player.curCarriedObject.GetComponent<Rigidbody>();

        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, false);
        player.isCarryObject = false;
        player.curCarriedObject.transform.parent = null;
        player.curCarriedObject.rigid.isKinematic = false;
        player.isSetAngleZero = true;
        _rigid.AddForce((new Vector3(player.transform.forward.x, 0, player.transform.forward.z).normalized + Vector3.up) * player.throwPower, ForceMode.Impulse);
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.IdleState);
        player.curInteractableObject = null;
        player.curCarriedObject = null;
        player.isHandIK = false;
    }

}
