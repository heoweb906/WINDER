using UnityEngine;
public class P_GroundState : PlayerMovementState
{
    public P_GroundState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GroundParameterHash);
        player.moveLerpSpeed = player.playerMoveLerpSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GroundParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (GetCurDirection() != Vector3.zero && (machine.CheckCurrentState(machine.IdleState) || machine.CheckCurrentState(machine.SoftLandingState)
            || machine.CheckCurrentState(machine.SoftStopState) || machine.CheckCurrentState(machine.HardStopState)
            || machine.CheckCurrentState(machine.RunningState) || machine.CheckCurrentState(machine.WalkingState)))
        {
            if (player.isRun)
                machine.OnStateChange(machine.RunningState);
            else
                machine.OnStateChange(machine.WalkingState);
        }
        else if (GetCurDirection() == Vector3.zero)
        {
            if (machine.CheckCurrentState(machine.WalkingState))
                machine.OnStateChange(machine.IdleState);
            else if (machine.CheckCurrentState(machine.RunningState))
                machine.OnStateChange(machine.IdleState);

        }


        CheckInputJump();
        InteractWithObject();


        CheckPutDownObject();
    }

    public void CheckInputJump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (!player.isCarryObject)
            {
                if (GetCurDirection() == Vector3.zero)
                {
                    machine.OnStateChange(machine.JumpStartIdleState);
                }
                else
                {
                    machine.OnStateChange(machine.JumpStartMoveState);
                }
            }
            else 
            {
                bool temp = FindClosestPartsParent();
                if (FindClosestPartsParent() && player.curCarriedObject.isParts && player.partsArea.PartOwnertype == player.curCarriedObject.partOwnerType)
                {
                    player.isGoToTarget = true;
                    Debug.Log("파츠 찾음");
                }
                else
                {
                    if(player.curCarriedObject.carriedObjectType == CarriedObjectType.Normal)
                    {
                        machine.OnStateChange(machine.ThrowState);
                    }
                    else if(player.curCarriedObject.carriedObjectType == CarriedObjectType.Guitar)
                    {
                        machine.OnStateChange(machine.GuitarBrokenState);
                    }
                    Debug.Log("파츠 못찾음");
                }
            }
        }
        else if (Input.GetButton("Jump") && player.curInteractableObject != null&& !player.playerAnim.IsInTransition(0) && player.partsArea != null)
        {
            if (Vector3.Distance(new Vector3(player.targetPos.x, 0, player.targetPos.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < 0.03f)
            {
                machine.OnStateChange(machine.PutPartsState);
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            player.isGoToTarget = false;
            
            player.partsArea = null;
            
        }
    }

   

    public void CheckPutDownObject()
    {
        if (player.isCarryObject && machine.CurrentState is not P_MoveStopState && !Input.GetButton("Fire1") && !player.playerAnim.IsInTransition(0))
        {
            machine.OnStateChange(machine.PutDownState);
        }
    }

    public void InteractWithObject()
    {
        if (player.isCarryObject)
            return;

        if (Input.GetButtonDown("Fire1")) // 좌클릭
        {
            if (!FindClosestInteractableObject())
                return;

            if (player.curInteractableObject.type == InteractableType.ClockWork)
            {
                player.curClockWork = player.curInteractableObject.GetComponent<ClockWork>();

                if (player.curClockWork.GetClockWorkType() == ClockWorkType.Floor)
                {
                    float angle = player.curClockWork.transform.eulerAngles.y * Mathf.Deg2Rad;
                    Vector3 pos1 = player.curClockWork.transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized * player.clockWorkInteractionDistance_Floor;
                    Vector3 pos2 = player.curClockWork.transform.position + (-new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized * player.clockWorkInteractionDistance_Floor);
                    //Debug.Log((pos1 - player.transform.position).magnitude + "//" + (pos2 - player.transform.position).magnitude);

                    player.targetPos = (pos1 - player.transform.position).magnitude >= (pos2 - player.transform.position).magnitude ? pos2 : pos1;
                    Debug.Log(player.targetPos);

                    //player.targetPos = player.curClockWork.transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized * player.clockWorkInteractionDistance_Floor;
                }
                else if (player.curClockWork.GetClockWorkType() == ClockWorkType.Wall)
                {
                    float angle = player.curClockWork.transform.eulerAngles.y * Mathf.Deg2Rad;
                    player.targetPos = player.curClockWork.transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized * player.clockWorkInteractionDistance_Wall;
                }
            }
            else if (player.curInteractableObject.type == InteractableType.Carrried && player.partsArea == null)
            {
                player.curCarriedObject = player.curInteractableObject.GetComponent<CarriedObject>();
                player.targetPos = player.curCarriedObject.transform.position + (player.transform.position - player.curCarriedObject.transform.position).normalized * player.carriedObjectInteractionDistance;

            }
            else if (player.curInteractableObject.type == InteractableType.Carrried && player.partsArea != null)
            {
                player.curCarriedObject = player.curInteractableObject.GetComponent<CarriedObject>();
                player.targetPos = player.partsArea.PartsInteractTransform.position;
            }
            else if (player.curInteractableObject.type == InteractableType.Grab)
            {
                player.curGrabObject = player.curInteractableObject.GetComponent<GrabObject>();
                player.grabPos = player.curGrabObject.GetClosestPosition(player.transform);
                player.targetPos = player.grabPos.position +
                    player.grabPos.forward * player.grabObjectInteractionDistance;
            }
            else if (player.curInteractableObject.type == InteractableType.SingleEvent)
            {
                float angle = player.curInteractableObject.transform.eulerAngles.y * Mathf.Deg2Rad;
                player.targetPos = player.curInteractableObject.transform.position - new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized * player.clockWorkInteractionDistance_Wall;
            }

        }
        else if (Input.GetButton("Fire1") && player.curInteractableObject != null && machine.CurrentState is not P_MoveStopState) // 좌클릭을 누르고 있는 동안
        {
            player.isGoToTarget = true;
            if (Vector3.Distance(new Vector3(player.targetPos.x, 0, player.targetPos.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < 0.03f)
            {
                if (player.curInteractableObject.type == InteractableType.ClockWork)
                {
                    if (player.curClockWork.GetClockWorkType() == ClockWorkType.Wall)
                        machine.OnStateChange(machine.SpinClockWorkWallState);
                    else if (player.curClockWork.GetClockWorkType() == ClockWorkType.Floor)
                        machine.OnStateChange(machine.SpinClockWorkFloorState);
                }
                else if (player.curInteractableObject.type == InteractableType.Carrried && player.partsArea == null)
                    machine.OnStateChange(machine.PickUpState);
                else if (player.curInteractableObject.type == InteractableType.Grab)
                    machine.OnStateChange(machine.GrabIdleState);
                else if (player.curInteractableObject.type == InteractableType.SingleEvent)
                {
                    player.curInteractableObject.ActiveEvent();
                    player.curInteractableObject = null;
                    player.isGoToTarget = false;
                }
                else if (player.curInteractableObject.type == InteractableType.Carrried && player.partsArea != null)
                {
                    machine.OnStateChange(machine.RemovePartsState);
                }
            }

            //player.closestClockWork.ChargingBattery(); // OnClockWork 함수 호출
        }
        else if (!Input.GetButton("Fire1")) // 마우스를 떼면
        {
            player.curClockWork = null; // 가장 가까운 ClockWork 참조 초기화
            player.curCarriedObject = null;
            player.curGrabObject = null;
            player.curInteractableObject = null;
            player.isCarryObject = false;
            player.isGoToTarget = false;
            player.partsArea = null;
        }
    }

    public override void SetDirection()
    {
        if (!player.isGoToTarget)
            base.SetDirection();
        else
        {
            player.curDirection = player.targetPos - player.transform.position;
        }
    }

    public bool FindClosestInteractableObject()
    {
        int interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, player.detectionRadius, interactableLayer);
        player.curInteractableObject = null;
        player.partsArea = null;

        player.curClockWork = null; // 이전 참조 초기화
        player.curCarriedObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            InteractableObject detectedObject = collider.GetComponent<InteractableObject>();
            if (detectedObject != null && detectedObject.canInteract)
            {
                float distance = Vector3.Distance(player.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    player.curInteractableObject = detectedObject; 
                }
            }
        }

        foreach (Collider collider in hitColliders)
        {
            PartsArea detectedObject = collider.GetComponent<PartsArea>();
            if (detectedObject != null && detectedObject.Parts != null)
            {
                float distance = Vector3.Distance(player.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    player.partsArea = detectedObject;
                    player.curInteractableObject = detectedObject.Parts.GetComponent<InteractableObject>();
                }
            }
        }

        if (player.curInteractableObject != null)
        {
            if (player.partsArea == null)
                Debug.Log("!!!!!!!!!!");
            return true;
            // 여기에서 추가적인 로직을 구현할 수 있습니다.
        }
        else
        {
            return false;
        }
    }

    public bool FindClosestPartsParent()
    {
        int interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, player.detectionRadius, interactableLayer);
        player.partsArea = null;

        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            PartsArea detectedObject = collider.GetComponent<PartsArea>();
            if (detectedObject != null && detectedObject.Parts == null)
            {
                float distance = Vector3.Distance(player.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    player.partsArea = detectedObject; // 가장 가까운 ClockWork 참조 저장
                }
            }
        }

        if (player.partsArea != null)
        {
            player.targetPos = player.partsArea.PartsInteractTransform.position;
            return true;
        }
        else
        {
            return false;
        }
    }


}


