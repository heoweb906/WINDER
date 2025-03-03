using UnityEngine;
public class P_OnAirState : PlayerMovementState
{
    public P_OnAirState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.OnAirParameterHash);
        player.moveLerpSpeed = player.playerMoveLerpSpeedOnJump;
        player.isGoToTarget = false;
        player.curClockWork = null;
        player.isCarryObject = false;
        player.curInteractableObject = null;
        player.curCarriedObject = null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        CheckHanging();
        if (Input.GetKeyDown(KeyCode.Q))
            machine.OnStateChange(machine.FallingIdleState);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        ControllGravity();
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.OnAirParameterHash);
    }

    public void CheckHanging()
    {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(player.transform.position + new Vector3(0, player.hangingPosOffset_Height, 0), player.transform.forward);
            Debug.DrawRay(ray.origin, player.transform.forward * player.cliffCheckRayDistance, Color.red);
            if (Physics.Raycast(ray, out player.cliffRayHit, player.cliffCheckRayDistance, player.cliffLayer))
            {
                BoxCollider _col = player.cliffRayHit.collider.GetComponent<BoxCollider>();
                Vector3 cliffPos = Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale) + player.cliffRayHit.transform.position;
                if ((cliffPos.y - player.cliffRayHit.point.y) < 0.1f && (cliffPos.y - player.cliffRayHit.point.y) > -0.1f)
                {
                    Debug.Log((cliffPos.y - player.cliffRayHit.point.y));
                    player.hangingPos = new Vector3(player.cliffRayHit.point.x, cliffPos.y, player.cliffRayHit.point.z);
                    //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cliffPos.y - 0.6f, player.transform.position.z); 
                   
                    machine.OnStateChange(machine.HangingState);
                }
            }
        }
    }

    public void ControllGravity()
    {
        if (player.rigid.velocity.y < 3 && player.groundList.Count == 0)
        {
            if (player.rigid.velocity.y > -player.maxFallingSpeed)
                player.rigid.velocity -= new Vector3(0, player.fallingPower * Time.fixedDeltaTime, 0);
            else player.rigid.velocity = new Vector3(player.rigid.velocity.x, -player.maxFallingSpeed, player.rigid.velocity.z);
        }

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (machine.CheckCurrentState(machine.JumpStartIdleState) || machine.CheckCurrentState(machine.FallingIdleState))
                machine.OnStateChange(machine.SoftLandingState);
            else if (machine.CheckCurrentState(machine.JumpStartMoveState) || machine.CheckCurrentState(machine.FallingMoveState))
                machine.OnStateChange(machine.MoveLandingState);
        }
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
