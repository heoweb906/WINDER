using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("ThisComponent")]
    [SerializeField]
    private Rigidbody playerRigid;
    [SerializeField]
    private Animator playerAnim;

    [Header("이동속도")]
    [SerializeField, Range(0,60)]
    private float playerMoveSpeed;
    [SerializeField, Range(0,60)]
    private float playerMoveLerpSpeed;
    [SerializeField, Range(0,60)]
    private float playerRotateLerpSpeed;
    [Header("점프")]
    [SerializeField, Range(0,20)]
    private float firstJumpPower;
    [SerializeField, Range(0,100)]
    private float fallingPower;
    [SerializeField, Range(0,20)]
    private float maxFallingSpeed;

    private Vector3 curDirection = Vector3.zero;
    private Vector3 preDirection = Vector3.zero;
    
    private int jumpCount;

    [Header("경사로")]
    [SerializeField,Range(0,2)]
    private float rayDistance = 1f;
    private RaycastHit slopeHit;
    private int groundLayer;
    [SerializeField, Range(0, 90)]
    private int maxSlopeAngle;
    [SerializeField]
    private GameObject raycastOrigin;

    [Header("Foot IK")]
    [SerializeField, Range(0, 1f)]
    private float distanceGround;

    private void Start()
    {
        AddMoveAction();
        InputManager.instance.FixedKeyaction += ControllGravity;
        groundLayer = ~(1 << LayerMask.NameToLayer("Player"));
    }

    // 플레이어 빠르게 떨어지도록
    private void ControllGravity()
    {
        if(playerRigid.velocity.y < 3 && groundList.Count == 0)
        {
            if (playerRigid.velocity.y > -maxFallingSpeed)
                playerRigid.velocity -= new Vector3(0, fallingPower * Time.fixedDeltaTime, 0);
            else playerRigid.velocity = new Vector3(playerRigid.velocity.x, -maxFallingSpeed, playerRigid.velocity.z);
        }

    }

    public Vector3 platformVelocity;
    public float platformVelocityLerp;

    // 플레이어 기본 이동
    private void PlayerWalk()
    {
        float _horizontal = Input.GetAxisRaw("Horizontal");
        float _vertical = Input.GetAxisRaw("Vertical");

        curDirection = new Vector3(_horizontal, 0, _vertical);

        // Rotation 이동 방향으로 조절
        if(curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(curDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotateLerpSpeed * Time.fixedDeltaTime);
        }

        // Rigid의 속도 조절로 이동, 보간 사용
        curDirection = Vector3.Lerp(preDirection, curDirection, playerMoveLerpSpeed * Time.fixedDeltaTime);
        playerAnim.SetFloat("moveSpeed", curDirection.magnitude); // 애니메이터 moveSpeed값 세팅

        Vector3 velocity = CalculateNextFrameGroundAngle(playerMoveSpeed) < maxSlopeAngle ? curDirection : Vector3.zero;
        Vector3 gravity;
        

        if (IsOnSlope()) // 경사로라면 경사에 맞춰서 방향값 세팅
        {
            velocity = AdjustDirectionToSlope(curDirection);
            gravity = Vector3.zero;
            playerRigid.useGravity = false;
        }
        else
        {
            gravity = new Vector3(0, playerRigid.velocity.y, 0);
            playerRigid.useGravity = true;
        }

        playerRigid.velocity = velocity * playerMoveSpeed + gravity;

        if (curMovingPlatform != null)
        {
            platformVelocity = curMovingPlatform.GetPlatformVelocity();
        }
        else
        {
            if (groundList.Count == 0)
            {
                platformVelocity = Vector3.Lerp(platformVelocity, Vector3.zero, platformVelocityLerp);

            }
            else
                platformVelocity = Vector3.zero;
        }
        //if(playerRigid.velocity.x > 0)
        //{
        //    if (platformVelocity.x < 0)
        //        platformVelocity.x = 0;
        //}
        //else if(playerRigid.velocity.x < 0)
        //{
        //    if (platformVelocity.x > 0)
        //        platformVelocity.x = 0;
        //}
        //if (playerRigid.velocity.z > 0)
        //{
        //    if (platformVelocity.z < 0)
        //        platformVelocity.z = 0;
        //}
        //else if (playerRigid.velocity.z < 0)
        //{
        //    if (platformVelocity.z > 0)
        //        platformVelocity.z = 0;
        //}
        playerRigid.velocity += platformVelocity;
        preDirection = curDirection;
    }

    // 현재 밟고 있는 땅 경사 체크
    private bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, Vector3.down * rayDistance, Color.red);
        if(Physics.Raycast(ray,out slopeHit, rayDistance, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle && jumpCount == 0;
        }
        return false;
    }

    // 밟고 있는 땅 기준으로 방향 재설정
    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }

    private float CalculateNextFrameGroundAngle(float _moveSpeed)
    {
        var nextFramePlayerPosition = raycastOrigin.transform.position + curDirection * _moveSpeed * Time.fixedDeltaTime;

        if(Physics.Raycast(nextFramePlayerPosition,Vector3.down,out RaycastHit hitInfo, 1f))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount == 0)
        {
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
            playerRigid.AddForce(new Vector3(0, firstJumpPower, 0), ForceMode.VelocityChange);
            jumpCount = 1;
            playerAnim.SetInteger("jumpCount", jumpCount);
        }
    }


    public void AddMoveAction()
    {
        InputManager.instance.keyaction += PlayerJump;
        InputManager.instance.FixedKeyaction += PlayerWalk;
    }

    public void DeleteMoveAction()
    {
        InputManager.instance.keyaction -= PlayerJump;
        InputManager.instance.FixedKeyaction -= PlayerWalk;
    }

    [SerializeField]
    private List<GameObject> groundList = new List<GameObject>();
    public MovingPlatform curMovingPlatform;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            groundList.Add(other.gameObject);
            jumpCount = 0;
            playerAnim.SetInteger("jumpCount", jumpCount);
            if (other.CompareTag("MovingPlatform"))
                curMovingPlatform = other.GetComponent<MovingPlatform>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && playerRigid.velocity.y < 0)
        {
            jumpCount = 0;
            playerAnim.SetInteger("jumpCount", jumpCount);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (groundList.Contains(other.gameObject))
                groundList.Remove(other.gameObject);
            if (groundList.Count > 0) return;
            jumpCount = 1;
            playerAnim.SetInteger("jumpCount", jumpCount);

            if (other.CompareTag("MovingPlatform"))
                curMovingPlatform = null;
        }
    }
}
