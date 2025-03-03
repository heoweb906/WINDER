using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : BaseState
{

    protected Player player;
    protected PlayerStateMachine machine;

    public PlayerMovementState(Player _player, PlayerStateMachine _machine)
    {
        player = _player;
        machine = _machine;
    }

    public virtual void OnEnter()
    {
        if (player.stateChangeDebug)
            Debug.Log("State: " + GetType().Name);

        player.rotateLerpSpeed = player.playerDefaultRotateLerpSpeed;

    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        player.isRun = Input.GetButton("Run");
        SetDirection();
    }

    public virtual void OnFixedUpdate()
    {
        PlayerVelocityControll();
    }


    public virtual void OnAnimationEnterEvent() { }
    public virtual void OnAnimationExitEvent() { }
    public virtual void OnAnimationTransitionEvent() { }







    public float _horizontal = 0;
    public float _vertical = 0;

    public Vector3 GetCurDirection() { return player.curDirection; }

    public virtual void SetDirection()
    {
        if (player.isWorldAxis)
        {
            Quaternion rotation = Quaternion.Euler(0, player.yAxis, 0);
            player.curDirection = rotation * Vector3.right * _horizontal + rotation * Vector3.forward * _vertical;

        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0, player.yAxis, 0);
            player.curDirection = rotation * player.camTransform.right * _horizontal + rotation * player.camTransform.forward * _vertical;
        }

        if (_horizontal == 0 && _vertical == 0)
            player.curDirection = Vector3.zero;
    }

    // 플레이어 기본 이동
    public virtual void PlayerVelocityControll()
    {

        player.curDirection.y = 0;
        player.curDirection = player.curDirection.normalized;



        PlayerRotationControll();

        // Rigid의 속도 조절로 이동, 보간 사용
        player.curDirection = Vector3.Lerp(player.preDirection, player.curDirection, player.moveLerpSpeed * Time.fixedDeltaTime);

        Vector3 velocity = CalculateNextFrameGroundAngle(player.playerMoveSpeed) < player.maxSlopeAngle ? player.curDirection : Vector3.zero;
        Vector3 gravity;


        if (IsOnSlope()) // 경사로라면 경사에 맞춰서 방향값 세팅
        {
            velocity = AdjustDirectionToSlope(player.curDirection);
            gravity = Vector3.zero;
            player.rigid.useGravity = false;
        }
        else if (machine.CurrentState is not P_ClimbingState)
        {
            gravity = new Vector3(0, player.rigid.velocity.y, 0);
            player.rigid.useGravity = true;
        }
        else
        {
            gravity = Vector3.zero;
        }

        player.rigid.velocity = velocity * player.playerMoveSpeed + gravity;

        if (player.curMovingPlatform != null)
        {
            player.platformVelocity = player.curMovingPlatform.GetPlatformVelocity();
        }
        else
        {
            if (player.groundList.Count == 0)
            {
                player.platformVelocity = Vector3.Lerp(player.platformVelocity, Vector3.zero, player.platformVelocityLerp);

            }
            else
                player.platformVelocity = Vector3.zero;
        }
        player.rigid.velocity += player.platformVelocity;
        player.preDirection = player.curDirection;
        //Debug.Log(player.rigid.velocity);
    }

    public virtual void PlayerRotationControll()
    {
        // Rotation 이동 방향으로 조절
        if (player.curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.curDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.rotateLerpSpeed * Time.fixedDeltaTime);
        }
    }



    // 현재 밟고 있는 땅 경사 체크
    private bool IsOnSlope()
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, Vector3.down * player.rayDistance, Color.red);
        if (Physics.Raycast(ray, out player.slopeHit, player.rayDistance, player.groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, player.slopeHit.normal);
            return angle != 0f && angle < player.maxSlopeAngle;
        }
        return false;
    }

    // 밟고 있는 땅 기준으로 방향 재설정
    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, player.slopeHit.normal);
    }
    private float CalculateNextFrameGroundAngle(float _moveSpeed)
    {
        var nextFramePlayerPosition = player.raycastOrigin.transform.position + player.curDirection * _moveSpeed * Time.fixedDeltaTime;

        int layerMask = ~((1 << LayerMask.NameToLayer("Carry")) | (1 << LayerMask.NameToLayer("Ignore Raycast")));
        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, 1f, layerMask))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }

    public void PlayerJump()
    {
        //player.rigid.velocity = new Vector3(player.rigid.velocity.x, 0, player.rigid.velocity.z);
        player.rigid.AddForce(new Vector3(0, player.firstJumpPower, 0), ForceMode.VelocityChange);
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WalkableGround") || other.CompareTag("MovingPlatform"))
        {
            player.groundList.Add(other.gameObject);

            if (other.CompareTag("MovingPlatform"))
            {
                player.curMovingPlatform = other.GetComponent<MovingPlatform>();
            }
        }
    }

    public virtual void OnTriggerStay(Collider other) { }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WalkableGround") || other.CompareTag("MovingPlatform"))
        {
            if (player.groundList.Contains(other.gameObject))
                player.groundList.Remove(other.gameObject);

            if (machine.CurrentState is not P_JumpStartState
                && machine.CurrentState is not P_ClimbingState
                && player.groundList.Count <= 0)
                machine.OnStateChange(machine.FallingMoveState);

            if (other.CompareTag("MovingPlatform"))
            {
                player.curMovingPlatform = null;
            }

            if (player.groundList.Count > 0) return;


        }
    }



}
