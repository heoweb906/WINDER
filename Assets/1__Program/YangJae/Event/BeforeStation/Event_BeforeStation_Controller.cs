using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class Event_BeforeStation_Controller : MonoBehaviour
{
    [SerializeField]
    private List<NPC_Simple> npcs = new List<NPC_Simple>();

    [SerializeField]
    private List<Transform> targets_1 = new List<Transform>();
    [SerializeField]
    private List<Transform> targets_2 = new List<Transform>();

    [SerializeField]
    private NPC_Simple mainNPC;

    [SerializeField]
    private Transform mainNPCTarget;

    [SerializeField]
    private float npcMoveSpeed = 2f; // 일반 NPC 이동 속도

    [SerializeField] 
    private float mainNPCMoveSpeed = 2f; // 메인 NPC 이동 속도


    [SerializeField]
    private GameObject originDropBox;
    [SerializeField]
    private Transform originDropBoxTransform;
    [SerializeField]
    private GameObject replacedDropBox;

    [SerializeField]
    private GameObject[] dropBoxItems;

    private void Start(){
        mainNPC.GetAnimator().SetLayerWeight(1,1);
    }

    public void StartEvent()
    {
        // 일반 NPC 이동 처리
        int count = Mathf.Min(npcs.Count, targets_1.Count);
        for(int i = 0; i < count; i++)
        {
            NPC_Simple npc = npcs[i];
            Transform target = targets_1[i];
            
            if(npc == null || target == null) continue;
            
            npc.gameObject.SetActive(true);
            MoveNPCToTarget(npc, target, npcMoveSpeed, () => npc.SetAvoidState());
        }

        // 메인 NPC 이동 처리
        if(mainNPC != null && mainNPCTarget != null)
        {
            MoveNPCToTarget(mainNPC, mainNPCTarget, mainNPCMoveSpeed, () => {
                mainNPC.SetDropState();
                mainNPC.GetAnimator().SetBool("Bool_Walk", false);
                DOTween.To(() => mainNPC.GetAnimator().GetLayerWeight(1), x => mainNPC.GetAnimator().SetLayerWeight(1, x), 0, 0.3f);
            });
        }
    }

    public void StartPickUpEvent()
    {
        StartCoroutine(PickUpEventCoroutine());
    }

    public IEnumerator PickUpEventCoroutine()
    {
        GoToPickUp(12);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        GoToPickUp(7);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        GoToPickUp(5);
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        GoToPickUp(4);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        GoToPickUp(8);
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        GoToPickUp(9);
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        GoToPickUp(10);

        /*
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        GoToPickUp(0);
        GoToPickUp(11);
        GoToPickUp(4);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        GoToPickUp(2);
        GoToPickUp(8);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        GoToPickUp(7);
        GoToPickUp(6);
        GoToPickUp(10);
        */
        
    }

    public void GoToPickUp(int index){
        NPC_Simple npc = npcs[index-1];
        npc.bWalking = true;
        npc.GetAnimator().SetBool("Bool_Walk", npc.bWalking);
        MoveNPCToTarget(npc, targets_2[index-1], npcMoveSpeed,()=>{npc.GetAnimator().SetTrigger("doPickUp"); npc.GetAnimator().SetBool("Bool_Walk", false);});
    }



    private void MoveNPCToTarget(NPC_Simple npc, Transform target, float speed, System.Action onComplete = null)
    {
        Transform npcTransform = npc.transform;

        // 타겟 방향을 계산하여 y축만 회전
        Vector3 direction = target.position - npcTransform.position;
        direction.y = 0; // y축 높이 차이 무시
        
        if(direction != Vector3.zero) 
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            npcTransform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0); // y축만 회전 적용
        }
        
        // 현재 위치에서 목표 위치까지의 거리 계산 (y축 제외)
        Vector3 currentPos = npcTransform.position;
        Vector3 targetPos = new Vector3(target.position.x, currentPos.y, target.position.z); // y값 유지
        float distance = Vector3.Distance(
            new Vector3(currentPos.x, 0, currentPos.z), 
            new Vector3(targetPos.x, 0, targetPos.z)
        );
        
        // 속도에 기반한 시간 계산
        float duration = distance / speed;
        
        // 속도 기반으로 이동 (y축 제외)
        npcTransform.DOMove(targetPos, duration).SetEase(Ease.Linear).OnComplete(() => {
            if(onComplete != null) onComplete();
        });
    }

    public void ReplaceDropBox(){
        replacedDropBox.SetActive(true);
        replacedDropBox.transform.position = originDropBoxTransform.position;
        // replacedDropBox.transform.rotation = originDropBox.transform.rotation;
        originDropBox.SetActive(false);
    }
    public void AddForceDropItems(){
        for(int i = 0; i < dropBoxItems.Length; i++){
            GameObject item = dropBoxItems[i];
            item.transform.parent = null;
            item.GetComponent<Rigidbody>().isKinematic = false;
            
            // 인덱스가 증가할수록 힘이 점점 감소하도록 계수 적용
            float forceFactor = 1.0f - (i / (float)dropBoxItems.Length * 0.9f);
            item.GetComponent<Rigidbody>().AddForce(
                (Vector3.left * 40 + Vector3.back * 80 + Vector3.up * 10) * forceFactor, 
                ForceMode.Impulse
            );
        }
    }

    private int currentHelperCount = 0;
    [SerializeField]
    private List<NPC_Event_BeforeStation> helperNPC;
    public void ActiveHelperNPC()
    {
        currentHelperCount++;
    }


    public int itemPickUpCount = 0;
    public void ItemPickUp(){
        itemPickUpCount++;
        if(itemPickUpCount >= 3){
            StartPickUpEvent();
        }
    }

    public void AnimationOnTransition(){
        AddForceDropItems();
    }

    public void AnimationOnExit(){
        ReplaceDropBox();
    }
}
