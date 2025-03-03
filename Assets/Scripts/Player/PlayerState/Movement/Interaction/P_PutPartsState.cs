using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_PutPartsState : P_InteractionState
{
    public P_PutPartsState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        if(player.partsArea.PartsAreaType == PartsAreaType.Wall)
        {
            machine.StartAnimation(player.playerAnimationData.PutPartsParameterHash);
        }
        else if(player.partsArea.PartsAreaType == PartsAreaType.Floor)
        {
            machine.StartAnimation(player.playerAnimationData.PutDownParameterHash);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if(player.partsArea.PartsAreaType == PartsAreaType.Wall)
        {
            machine.StopAnimation(player.playerAnimationData.PutPartsParameterHash);
        }
        else if(player.partsArea.PartsAreaType == PartsAreaType.Floor)
        {
            machine.StopAnimation(player.playerAnimationData.PutDownParameterHash);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.playerAnim.SetLayerWeight(1, player.carryWeight);
    }

    public override void OnAnimationTransitionEvent()
    {
        player.curCarriedObject.transform.parent = player.partsArea.PartsTransform;
        player.isSetAngleZero = true;
        player.partsArea.InsertParts(player.curCarriedObject.gameObject);
        player.SetCarryWeight();
        
        Vector3 targetPosition = Vector3.zero + player.curCarriedObject.putPartsPositionOffset;
        Vector3 targetRotation = Vector3.zero + player.curCarriedObject.putPartsRotationOffset;
        
        player.curCarriedObject.transform.DOLocalRotate(targetRotation, 0.5f);
        player.curCarriedObject.transform.DOLocalMove(targetPosition, 0.5f);
        player.curCarriedObject.canInteract = false;
    }

    public override void OnAnimationExitEvent()
    {
        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, false);
        player.isCarryObject = false;
        player.curInteractableObject = null;
        player.curCarriedObject = null;
        player.isHandIK = false;
        machine.OnStateChange(machine.IdleState);

    }

    public override void SetDirection()
    {
        player.curDirection = player.partsArea.PartsTransform.position - player.transform.position;
    }

    public override void PlayerRotationControll()
    {
        Quaternion targetRotation = Quaternion.LookRotation(player.curDirection);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.rotateLerpSpeed * Time.fixedDeltaTime);

    }
}

