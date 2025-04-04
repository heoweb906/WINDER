using UnityEngine;
using DG.Tweening;
public class P_PickUpState : P_InteractionState
{
    private Sequence moveSequence;

    public P_PickUpState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.PickUpParameterHash);
        player.isCarryObject = true;
        player.curCarriedObject.rigid.isKinematic = true;
        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, true);
        player.angle = 0;
        player.isSetAngleZero = false;

        if (player.curCarriedObject.bPickUpEvent)
        {
            player.SetLockCarryObject(true);
            player.curCarriedObject.PickUpEvent();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if (moveSequence != null)
        {
            moveSequence.Kill();
        }
        player.KillArmAngleTween();
        machine.StopAnimation(player.playerAnimationData.PickUpParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        CheckPutDownObject();
    }

    public override void SetDirection()
    {
        if (player.isHandIK)
            base.SetDirection();
        else
            player.curDirection = player.curInteractableObject.transform.position - player.transform.position;
    }

    public void CheckPutDownObject()
    {
        if (player.isCarryObject && !Input.GetButton("Fire1") && !player.playerAnim.IsInTransition(0) && !player.isLockCarryObject)
        {

            player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, false);
            player.playerAnim.SetLayerWeight(1, 0);
            machine.OnStateChange(machine.IdleState);
            player.isCarryObject = false;
            player.curInteractableObject = null;
            player.curCarriedObject.transform.parent = null;
            player.curCarriedObject.rigid.isKinematic = false;
            player.isHandIK = false;
        }
    }

    public override void OnAnimationTransitionEvent()
    {
        if (moveSequence != null)
        {
            moveSequence.Kill();
        }

        player.curCarriedObject.transform.parent = player.CarriedObjectPos;
        BoxCollider _col = player.curCarriedObject.GetComponent<BoxCollider>();
        Vector3 targetPosition = Vector3.zero;
        targetPosition -= new Vector3(Vector3.Scale(_col.size * 0.5f, _col.transform.localScale).y, 0, 0);
        targetPosition += player.curCarriedObject.holdPositionOffset;

        Quaternion targetRotation = Quaternion.Euler
            (new Vector3(player.curCarriedObject.holdRotationOffset.x,
            player.curCarriedObject.holdRotationOffset.y,
            player.curCarriedObject.holdRotationOffset.z));

        moveSequence = DOTween.Sequence();
        moveSequence.Join(player.curCarriedObject.transform.DOLocalMove(targetPosition, 0.5f));
        moveSequence.Join(player.curCarriedObject.transform.DORotate(targetRotation.eulerAngles, 0.5f));
        
        if(player.curCarriedObject.carriedObjectType == CarriedObjectType.Normal)
        {
            player.isHandIK = true;
        }
        else if(player.curCarriedObject.carriedObjectType == CarriedObjectType.Guitar)
        {
            player.isHandIK = false;
            DOTween.To(() => player.playerAnim.GetLayerWeight(1), x => player.playerAnim.SetLayerWeight(1, x), 1, 0.2f);
        }
    }

    public override void OnAnimationExitEvent()
    {
        if(player.curCarriedObject.carriedObjectType == CarriedObjectType.Normal)
        {
            player.playerAnim.SetBool(player.playerAnimationData.CarryNormalParameterHash, true);
            player.playerAnim.SetBool(player.playerAnimationData.CarryGuitarParameterHash, false);
        }
        else if(player.curCarriedObject.carriedObjectType == CarriedObjectType.Guitar)
        {
            player.playerAnim.SetBool(player.playerAnimationData.CarryNormalParameterHash, false);
            player.playerAnim.SetBool(player.playerAnimationData.CarryGuitarParameterHash, true);
        }
        DOTween.To(() => player.playerAnim.GetLayerWeight(1), x => player.playerAnim.SetLayerWeight(1, x), 1, 0.2f);
        machine.OnStateChange(machine.IdleState);
    }

}

