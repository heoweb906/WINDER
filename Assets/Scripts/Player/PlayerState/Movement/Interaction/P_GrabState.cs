using System.Collections;
using UnityEngine;

public class P_GrabState : P_InteractionState
{
    public P_GrabState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GrabParameterHash);
        player.playerMoveSpeed = player.playerGrapMoveSpeed;
        player.SetPlayerPhysicsIgnore(player.curGrabObject.col, true);
        player.rigid.velocity = Vector3.zero;
        player.rotateLerpSpeed = player.playerGrapRotateLerpSpeed;
        Vector3 temp = player.grabPos.position - player.transform.position;
        temp.y = 0;
        player.transform.rotation = Quaternion.LookRotation(temp);
        player.curGrabObject.AddJoint(player.grabPos);
        player.curGrabObject.joint.connectedBody = player.rigid;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GrabParameterHash);
        //player.curGrabObject.transform.parent = null;
        //player.curGrabObject.rigid.isKinematic = false;
        player.SetPlayerPhysicsIgnore(player.curGrabObject.col, false);
        player.curGrabObject.joint.connectedBody = null;
        player.curGrabObject.DeleteJoint();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!Input.GetButton("Fire1"))
        {
            machine.OnStateChange(machine.IdleState);
        }
        else if (player.curDirection != Vector3.zero)
        {
            // 플레이어와 물체 사이의 방향
            Vector3 toObject = (player.grabPos.position - player.transform.position).normalized;
            // 이동하려는 방향
            Vector3 moveDirection = player.curDirection.normalized;
            
            float dotProduct = Vector3.Dot(toObject, moveDirection);
            
            if (dotProduct > 0) // 물체 방향으로 이동 = 밀기
                machine.OnStateChange(machine.PushState);
            else // 물체 반대 방향으로 이동 = 당기기
                machine.OnStateChange(machine.PullState);
        }
    }

    public override void SetDirection()
    {
        if (player.isWorldAxis)
        {
            Quaternion rotation = Quaternion.Euler(0, player.yAxis, 0);
            player.curDirection = rotation * Vector3.right * _horizontal + rotation * Vector3.forward * _vertical;
        }
        else
        {
            player.curDirection = player.camTransform.right * _horizontal + player.camTransform.forward * _vertical;
        }
        
        if (_horizontal == 0 && _vertical == 0)
            player.curDirection = Vector3.zero;
    }

    public override void PlayerRotationControll()
    {
        // Rotation 이동 방향으로 조절
        if (player.curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-player.curDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.rotateLerpSpeed * Time.fixedDeltaTime);
        }
    }

}
