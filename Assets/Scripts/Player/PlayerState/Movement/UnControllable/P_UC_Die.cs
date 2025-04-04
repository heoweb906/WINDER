using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_UC_Die : P_UnControllable
{
    private List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
    
    public P_UC_Die(Player player, PlayerStateMachine machine) : base(player, machine) 
    {
    }

    private enum ColliderType
    {
        Box,
        Sphere,
        Capsule
    }

    private void AddRagdollComponents(Transform transform, ColliderType colliderType)
    {
        if (transform != null)
        {
            // Rigidbody 추가
            Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
            rb.mass = player.totalMass / 13f;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            ragdollRigidbodies.Add(rb);

            rb.gameObject.layer = LayerMask.NameToLayer("Player");

            // 랜덤한 방향과 힘 적용 (위쪽 방향 보정)
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y) + 0.5f; // 위쪽 방향 보정
            randomDirection.Normalize(); // 정규화
            float randomForce = Random.Range(2f, 4f); // 힘도 약간 증가
            rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);

            // Collider 추가
            switch (colliderType)
            {
                case ColliderType.Box:
                    BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(0.1f, 0.1f, 0.2f); // 발에 적합한 크기
                    break;
                case ColliderType.Sphere:
                    BoxCollider headCollider = transform.gameObject.AddComponent<BoxCollider>();
                    headCollider.size = new Vector3(0.2f, 0.2f, 0.2f); // 머리에 적합한 크기
                    break;
                case ColliderType.Capsule:
                    BoxCollider limbCollider = transform.gameObject.AddComponent<BoxCollider>();
                    limbCollider.size = new Vector3(0.08f, 0.08f, 0.08f); // 팔, 다리, 몸통에 적합한 크기
                    break;
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if(player.dieIndex == 0)
        {
            machine.StartAnimation(player.playerAnimationData.UC_DieParameterHash);
            // 모든 레그돌 트랜스폼에 Rigidbody와 Collider 추가
            AddRagdollComponents(player.pelvis, ColliderType.Capsule);
            AddRagdollComponents(player.leftHips, ColliderType.Capsule);
            AddRagdollComponents(player.rightHips, ColliderType.Capsule);
            AddRagdollComponents(player.leftArm, ColliderType.Capsule);
            AddRagdollComponents(player.rightArm, ColliderType.Capsule);
            AddRagdollComponents(player.middleSpine, ColliderType.Capsule);
            AddRagdollComponents(player.head, ColliderType.Capsule);
            AddRagdollComponents(player.leftElbow, ColliderType.Capsule);
            AddRagdollComponents(player.rightElbow, ColliderType.Capsule);
            AddRagdollComponents(player.leftKnee, ColliderType.Capsule);
            AddRagdollComponents(player.rightKnee, ColliderType.Capsule);
            AddRagdollComponents(player.leftFoot, ColliderType.Capsule);
            AddRagdollComponents(player.rightFoot, ColliderType.Capsule);
            // 레그돌 활성화
            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
            player.playerAnim.enabled = false;
        }
        else if(player.dieIndex == 1)
        {
            machine.StartAnimation(player.playerAnimationData.UC_Die_GrabParameterHash);
        }
        
    }

    public override void OnExit()
    {
        base.OnExit();
        if(player.dieIndex == 0)
        {
            machine.StopAnimation(player.playerAnimationData.UC_DieParameterHash);
        }
        else if(player.dieIndex == 1)
        {
            machine.StopAnimation(player.playerAnimationData.UC_Die_GrabParameterHash);
        }
        
        // 레그돌 비활성화
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_UC_Die : P_UnControllable
{
    private List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
    
    public P_UC_Die(Player player, PlayerStateMachine machine) : base(player, machine) 
    {
    }

    private enum ColliderType
    {
        Box,
        Sphere,
        Capsule
    }

    private void AddRagdollComponents(Transform transform, ColliderType colliderType)
    {
        if (transform != null)
        {
            // Rigidbody 추가
            Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
            rb.mass = player.totalMass / 13f;
            rb.useGravity = false;
            rb.isKinematic = true;
            ragdollRigidbodies.Add(rb);

            // Collider 추가
            switch (colliderType)
            {
                case ColliderType.Box:
                    BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(0.1f, 0.1f, 0.2f); // 발에 적합한 크기
                    break;
                case ColliderType.Sphere:
                    SphereCollider sphereCollider = transform.gameObject.AddComponent<SphereCollider>();
                    sphereCollider.radius = 0.1f; // 머리에 적합한 크기
                    break;
                case ColliderType.Capsule:
                    CapsuleCollider capsuleCollider = transform.gameObject.AddComponent<CapsuleCollider>();
                    capsuleCollider.radius = 0.1f; // 팔, 다리, 몸통에 적합한 크기
                    capsuleCollider.height = 0.3f;
                    break;
            }

            // CharacterJoint 추가 (pelvis 제외)
            if (transform != player.pelvis)
            {
                CharacterJoint joint = transform.gameObject.AddComponent<CharacterJoint>();
                ConfigureJoint(joint, transform);
            }
        }
    }

    private void ConfigureJoint(CharacterJoint joint, Transform transform)
    {
        // 기본 조인트 설정
        joint.enableProjection = true;
        joint.projectionDistance = 0.1f;
        joint.projectionAngle = 180f;
        joint.enablePreprocessing = false;
        joint.massScale = 1f;
        joint.connectedMassScale = 1f;

        // 각 본에 따른 특정 설정
        if (transform == player.head)
        {
            joint.connectedBody = player.middleSpine.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, -10f, 10f);
            SetupSwingLimit(joint, 10f, 0f);
        }
        else if (transform == player.middleSpine)
        {
            joint.connectedBody = player.pelvis.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, -20f, 20f);
            SetupSwingLimit(joint, 10f, 10f);
        }
        else if (transform == player.leftArm || transform == player.rightArm)
        {
            joint.connectedBody = player.pelvis.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, -20f, 20f);
            SetupSwingLimit(joint, 90f, 90f);
        }
        else if (transform == player.leftElbow || transform == player.rightElbow)
        {
            joint.connectedBody = (transform == player.leftElbow) ? 
                player.leftArm.GetComponent<Rigidbody>() : 
                player.rightArm.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, -90f, 0f);
            SetupSwingLimit(joint, 0f, 0f);
        }
        else if (transform == player.leftHips || transform == player.rightHips)
        {
            joint.connectedBody = player.pelvis.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, -20f, 20f);
            SetupSwingLimit(joint, 45f, 45f);
        }
        else if (transform == player.leftKnee || transform == player.rightKnee)
        {
            joint.connectedBody = (transform == player.leftKnee) ? 
                player.leftHips.GetComponent<Rigidbody>() : 
                player.rightHips.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, 0f, 0f);
            SetupSwingLimit(joint, 0f, 130f);
        }
        else if (transform == player.leftFoot || transform == player.rightFoot)
        {
            joint.connectedBody = (transform == player.leftFoot) ? 
                player.leftKnee.GetComponent<Rigidbody>() : 
                player.rightKnee.GetComponent<Rigidbody>();
            SetupTwistLimit(joint, -20f, 20f);
            SetupSwingLimit(joint, 20f, 20f);
        }

        // Break Force 설정`
        joint.breakForce = float.PositiveInfinity;
        joint.breakTorque = float.PositiveInfinity;
    }

    private void SetupTwistLimit(CharacterJoint joint, float low, float high)
    {
        SoftJointLimit lowTwist = new SoftJointLimit();
        lowTwist.limit = low;
        joint.lowTwistLimit = lowTwist;

        SoftJointLimit highTwist = new SoftJointLimit();
        highTwist.limit = high;
        joint.highTwistLimit = highTwist;
    }

    private void SetupSwingLimit(CharacterJoint joint, float swing1, float swing2)
    {
        SoftJointLimit swing1Limit = new SoftJointLimit();
        swing1Limit.limit = swing1;
        joint.swing1Limit = swing1Limit;

        SoftJointLimit swing2Limit = new SoftJointLimit();
        swing2Limit.limit = swing2;
        joint.swing2Limit = swing2Limit;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimationTrigger(player.playerAnimationData.UC_DieParameterHash);
        player.playerAnim.enabled = false;
        
        // 모든 레그돌 트랜스폼에 Rigidbody와 Collider 추가
        AddRagdollComponents(player.pelvis, ColliderType.Capsule);
        AddRagdollComponents(player.leftHips, ColliderType.Capsule);
        AddRagdollComponents(player.leftKnee, ColliderType.Capsule);
        AddRagdollComponents(player.leftFoot, ColliderType.Capsule);
        AddRagdollComponents(player.rightHips, ColliderType.Capsule);
        AddRagdollComponents(player.rightKnee, ColliderType.Capsule);
        AddRagdollComponents(player.rightFoot, ColliderType.Capsule);
        AddRagdollComponents(player.leftArm, ColliderType.Capsule);
        AddRagdollComponents(player.leftElbow, ColliderType.Capsule);
        AddRagdollComponents(player.rightArm, ColliderType.Capsule);
        AddRagdollComponents(player.rightElbow, ColliderType.Capsule);
        AddRagdollComponents(player.middleSpine, ColliderType.Capsule);
        AddRagdollComponents(player.head, ColliderType.Sphere);
        // 레그돌 활성화
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.UC_DieParameterHash);
        
        // 레그돌 비활성화
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
*/