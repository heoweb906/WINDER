using UnityEngine;
using DG.Tweening;

public class P_GuitarBrokenState : P_InteractionState
{
    public P_GuitarBrokenState(Player player, PlayerStateMachine machine) : base(player, machine) { }
    
    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GuitarBrokenParameterHash);
        player.playerAnim.SetLayerWeight(1, 0);
    }
    
    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GuitarBrokenParameterHash);
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        player.playerAnim.SetLayerWeight(1, player.carryWeight);
    }
    public override void OnAnimationTransitionEvent()
    {
        
        // 주변 오브젝트 검사를 위한 박스 설정
        Vector3 boxSize = new Vector3(1.5f, 1f, 1f);
        Vector3 boxCenter = player.transform.position + player.transform.forward * (boxSize.z / 2f);
        
        // 디버그용 박스 그리기
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, -boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, -boxSize.y/2, -boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, -boxSize.y/2, boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, -boxSize.y/2, boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, -boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(-boxSize.x/2, -boxSize.y/2, boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(boxSize.x/2, -boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, -boxSize.y/2, boxSize.z/2), Color.red);
        
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, boxSize.y/2, -boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, boxSize.y/2, boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, boxSize.y/2, boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(-boxSize.x/2, boxSize.y/2, boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(boxSize.x/2, boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, boxSize.y/2, boxSize.z/2), Color.red);
        
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, -boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(-boxSize.x/2, boxSize.y/2, -boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(boxSize.x/2, -boxSize.y/2, -boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, boxSize.y/2, -boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(-boxSize.x/2, -boxSize.y/2, boxSize.z/2), boxCenter + new Vector3(-boxSize.x/2, boxSize.y/2, boxSize.z/2), Color.red);
        Debug.DrawLine(boxCenter + new Vector3(boxSize.x/2, -boxSize.y/2, boxSize.z/2), boxCenter + new Vector3(boxSize.x/2, boxSize.y/2, boxSize.z/2), Color.red);
        
        // 박스 범위 내의 모든 콜라이더 가져오기
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize/2f, player.transform.rotation);
        
        // 플레이어 전방 방향
        Vector3 playerForward = player.transform.forward;
        
        foreach (Collider collider in colliders)
        {
            // 플레이어와 오브젝트 사이의 방향 벡터
            Vector3 directionToObject = (collider.transform.position - player.transform.position).normalized;
            
            // 전방 60도 각도 내에 있는지 확인 (dot product 0.5는 약 60도)

                GlassWall glassWall = collider.gameObject.GetComponent<GlassWall>();
                if (glassWall != null)
                {
                    glassWall.CrashGlassWall();
                    collider.enabled = false;
                    break; // 첫 번째 유리벽을 찾으면 종료
                }
        }
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.IdleState);
        player.playerAnim.SetLayerWeight(1, 1);
    }

    
}

