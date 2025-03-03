using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_RemovePartsState : P_InteractionState
{
    private Sequence moveSequence;
    
    public P_RemovePartsState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        if(player.partsArea.PartsAreaType == PartsAreaType.Wall)
        {
            machine.StartAnimation(player.playerAnimationData.RemovePartsParameterHash);
        }
        else if(player.partsArea.PartsAreaType == PartsAreaType.Floor)
        {
            machine.StartAnimation(player.playerAnimationData.PickUpParameterHash);
        }
        
        player.isCarryObject = true;
        player.curCarriedObject.rigid.isKinematic = true;
        player.curInteractableObject.canInteract = true;
        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, true);
        player.angle = 0;
        player.isSetAngleZero = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        if(player.partsArea.PartsAreaType == PartsAreaType.Wall)
        {
            machine.StopAnimation(player.playerAnimationData.RemovePartsParameterHash);
        }
        else if(player.partsArea.PartsAreaType == PartsAreaType.Floor)
        {
            machine.StopAnimation(player.playerAnimationData.PickUpParameterHash);
        }
    }

    public override void SetDirection()
    {
        player.curDirection = player.partsArea.PartsTransform.position - player.transform.position;
    }

    public override void OnAnimationTransitionEvent()
    {
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

        player.partsArea.RemoveParts();

        player.isHandIK = true;
    }

    public override void OnAnimationExitEvent()
    {
        player.playerAnim.SetLayerWeight(1, 1);
        machine.OnStateChange(machine.IdleState);
    }


}
