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
    private GameObject originDropBox_1;
    [SerializeField]
    private GameObject originDropBox_2;
    [SerializeField]
    private GameObject originDropBox_3;
    [SerializeField]
    private ParsArea_DropBox replacedDropBox_1;
    [SerializeField]
    private ParsArea_DropBox replacedDropBox_2;
    [SerializeField]
    private ParsArea_DropBox replacedDropBox_3;

    private void Start(){
        mainNPC.GetAnimator().SetLayerWeight(1,1);
    }
    public void StartEvent()
    {
        foreach(NPC_Simple npc in npcs){
            npc.gameObject.SetActive(true);
        }
        // 일반 NPC 이동 처리
        int count = Mathf.Min(npcs.Count, targets_1.Count);
        for(int i = 0; i < count; i++)
        {
            NPC_Simple npc = npcs[i];
            Transform target = targets_1[i];
            
            if(npc == null || target == null) continue;
            
            npc.gameObject.SetActive(true);
            MoveNPCToTarget(npc, target.position, npcMoveSpeed, () => npc.SetAvoidState());
        }

        // 박스들 위아래로 움직이기
        if (originDropBox_1 != null)
        {
            Vector3 originalPos = originDropBox_1.transform.position;
            originDropBox_1.transform.DOMoveY(originalPos.y + 0.1f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        
        if (originDropBox_2 != null)
        {
            Vector3 originalPos = originDropBox_2.transform.position;
            originDropBox_2.transform.DOMoveY(originalPos.y + 0.1f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        
        if (originDropBox_3 != null)
        {
            Vector3 originalPos = originDropBox_3.transform.position;
            originDropBox_3.transform.DOMoveY(originalPos.y + 0.1f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        // 메인 NPC 이동 처리
        if(mainNPC != null && mainNPCTarget != null)
        {
            MoveNPCToTarget(mainNPC, mainNPCTarget.position, mainNPCMoveSpeed, () => {
                // 박스 움직임 중지
                DOTween.Kill(originDropBox_1.transform);
                DOTween.Kill(originDropBox_2.transform);
                DOTween.Kill(originDropBox_3.transform);
                originDropBox_1.transform.localPosition = new Vector3(-0.08f,0,0);
                originDropBox_2.transform.localPosition = new Vector3(-0.08f,0,0);
                originDropBox_3.transform.localPosition = new Vector3(-0.08f,0,0);
                
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
        
        yield return new WaitForSeconds(1f);

        GoToPickUp(npcs[0], replacedDropBox_1);

        yield return new WaitForSeconds(2f);

        GoToPickUp(npcs[6], replacedDropBox_2);

        yield return new WaitForSeconds(2f);

        GoToPickUp(npcs[10], replacedDropBox_3);

        yield return new WaitForSeconds(2f);

        GoToPickUp(npcs[9], replacedDropBox_2);

        yield return new WaitForSeconds(2f);

        GoToPickUp(npcs[11], replacedDropBox_3);
    }




    private void MoveNPCToTarget(NPC_Simple npc, Vector3 target, float speed, System.Action onComplete = null)
    {
        npc.GetAnimator().SetBool("Bool_Walk", true);
        Transform npcTransform = npc.transform;

        // 타겟 방향을 계산하여 y축만 회전
        Vector3 direction = target - npcTransform.position;
        direction.y = 0; // y축 높이 차이 무시
        
        if(direction != Vector3.zero) 
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            npcTransform.DORotate(new Vector3(0, rotation.eulerAngles.y, 0), 0.3f);
        }
        
        // 현재 위치에서 목표 위치까지의 거리 계산 (y축 제외)
        Vector3 currentPos = npcTransform.position;
        Vector3 targetPos = new Vector3(target.x, currentPos.y, target.z); // y값 유지
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
        replacedDropBox_1.gameObject.SetActive(true);
        replacedDropBox_2.gameObject.SetActive(true);
        replacedDropBox_3.gameObject.SetActive(true);

        replacedDropBox_1.transform.position = originDropBox_1.transform.position;
        replacedDropBox_1.transform.rotation = originDropBox_1.transform.rotation;
        replacedDropBox_2.transform.position = originDropBox_2.transform.position;
        replacedDropBox_2.transform.rotation = originDropBox_2.transform.rotation;
        replacedDropBox_3.transform.position = originDropBox_3.transform.position;
        replacedDropBox_3.transform.rotation = originDropBox_3.transform.rotation;
        
        originDropBox_1.SetActive(false);
        originDropBox_2.SetActive(false);
        originDropBox_3.SetActive(false);
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

    public CarriedObject GetClosestItem(NPC_Simple npc,ParsArea_DropBox dropBox){
        List<CarriedObject> items = dropBox.GetItems();

        if(items.Count == 0) return null;

        float closestDistance = 100000;

        CarriedObject closestItem = null;

        foreach(CarriedObject item in items){
            float distance = Vector3.Distance(item.transform.position , npc.transform.position);
            if(distance < closestDistance){
                closestDistance = distance;
                closestItem = item;
            }
        }

        if(closestItem != null){
            return closestItem;
        }



        return null;
    }

    private bool endPickUp = false;

    public void EndPickUpEvent(){
        endPickUp = true;
        Debug.Log("EndPickUpEvent");
        StartCoroutine(C_EndPickUpEvent());
    }

    public IEnumerator C_EndPickUpEvent(){
        yield return new WaitForSeconds(1f);
        Vector3 targetRotation = replacedDropBox_1.transform.position - mainNPC.transform.position;
        mainNPC.transform.DORotate(new Vector3(0,Quaternion.LookRotation(targetRotation).eulerAngles.y,0), 1f).onComplete = () => {
            mainNPC.GetAnimator().SetTrigger("doThankyouAction");
        };

        yield return new WaitForSeconds(2f);
        
        targetRotation = replacedDropBox_2.transform.position - mainNPC.transform.position;
        mainNPC.transform.DORotate(new Vector3(0,Quaternion.LookRotation(targetRotation).eulerAngles.y,0), 1f).onComplete = () => {
            mainNPC.GetAnimator().SetTrigger("doThankyouAction");
        };

        yield return new WaitForSeconds(2f);

        targetRotation = replacedDropBox_3.transform.position - mainNPC.transform.position;
        mainNPC.transform.DORotate(new Vector3(0,Quaternion.LookRotation(targetRotation).eulerAngles.y,0), 1f).onComplete = () => {
            mainNPC.GetAnimator().SetTrigger("doThankyouAction");
        };



        
        yield return new WaitForSeconds(2f);
        targetRotation = GameAssistManager.Instance.GetPlayer().transform.position - mainNPC.transform.position;
        mainNPC.transform.DORotate(new Vector3(0,Quaternion.LookRotation(targetRotation).eulerAngles.y,0), 1f).onComplete = () => {
            mainNPC.RotatePlayerTaeyub();
        };

    }


    
    public void GoToPickUp(NPC_Simple npc, ParsArea_DropBox dropBox){
        CarriedObject item = GetClosestItem(npc, dropBox);

        if(item == null){
            if(replacedDropBox_1.GetItems().Count == 0 && replacedDropBox_2.GetItems().Count == 0 && replacedDropBox_3.GetItems().Count == 0 && endPickUp == false){
                EndPickUpEvent();
                GoHomeEventCoroutine();
            }
            return;
        } 

        npc.GetAnimator().SetBool("Bool_Walk", true);

        dropBox.RemoveItem(item);
        npc.GetComponent<NPC_Event_BeforeStation>().SetItem(item, dropBox);

        Vector3 targetPos = item.transform.position + (npc.transform.position - item.transform.position).normalized * 0.5f;

        Vector3 targetRotation = Quaternion.LookRotation(item.transform.position - targetPos).eulerAngles;

        MoveNPCToTarget(npc, targetPos, npcMoveSpeed, () => {
            item.GetComponent<Rigidbody>().isKinematic = true;
            npc.GetAnimator().SetBool("Bool_Walk", false);
            npc.GetAnimator().SetTrigger("doPickUp");
            npc.transform.DORotate(new Vector3(0,targetRotation.y,0), 0.3f);

            DOVirtual.DelayedCall(1.5f, () => {
                npc.GetAnimator().SetBool("Bool_Walk", true);

                targetPos = dropBox.transform.position + (npc.transform.position - dropBox.transform.position).normalized * 0.5f;
                MoveNPCToTarget(npc, targetPos, npcMoveSpeed, () => {
                    npc.GetAnimator().SetBool("Bool_Walk", false);
                    npc.GetAnimator().SetTrigger("doPutDown");
                    DOVirtual.DelayedCall(1.5f, () => {GoToPickUp(npc, dropBox);});
                });
            });
        });


        
    }

    public IEnumerator GoHomeEventCoroutine()
    {

        yield return new WaitForSeconds(3f);

        MoveNPCToTarget(npcs[0], targets_2[0].position, npcMoveSpeed, () => {
            npcs[0].gameObject.SetActive(false);
        });
        MoveNPCToTarget(npcs[3], targets_2[3].position, npcMoveSpeed, () => {
            npcs[3].gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(2f);

        MoveNPCToTarget(npcs[6], targets_2[6].position, npcMoveSpeed, () => {
            npcs[6].gameObject.SetActive(false);
        MoveNPCToTarget(npcs[8], targets_2[8].position, npcMoveSpeed, () => {
            npcs[8].gameObject.SetActive(false);
        });
        });

        yield return new WaitForSeconds(2f);

        MoveNPCToTarget(npcs[10], targets_2[10].position, npcMoveSpeed, () => {
            npcs[10].gameObject.SetActive(false);
        MoveNPCToTarget(npcs[4], targets_2[4].position, npcMoveSpeed, () => {
            npcs[4].gameObject.SetActive(false);
        });
        });

        yield return new WaitForSeconds(2f);

        MoveNPCToTarget(npcs[9], targets_2[9].position, npcMoveSpeed, () => {
            npcs[9].gameObject.SetActive(false);
        MoveNPCToTarget(npcs[7], targets_2[7].position, npcMoveSpeed, () => {
            npcs[7].gameObject.SetActive(false);
        });
        MoveNPCToTarget(npcs[1], targets_2[1].position, npcMoveSpeed, () => {
            npcs[1].gameObject.SetActive(false);
        });
        });

        yield return new WaitForSeconds(2f);

        MoveNPCToTarget(npcs[11], targets_2[11].position, npcMoveSpeed, () => {
            npcs[11].gameObject.SetActive(false);
        MoveNPCToTarget(npcs[12], targets_2[12].position, npcMoveSpeed, () => {
            npcs[12].gameObject.SetActive(false);
        });
        MoveNPCToTarget(npcs[5], targets_2[5].position, npcMoveSpeed, () => {
            npcs[5].gameObject.SetActive(false);
        });
        MoveNPCToTarget(npcs[2], targets_2[2].position, npcMoveSpeed, () => {
            npcs[2].gameObject.SetActive(false);
        });
        });
    }


    public void RemoveItems(CarriedObject item){
        if(replacedDropBox_1.GetItems().Contains(item)){
            replacedDropBox_1.RemoveItem(item);
        }
        else if(replacedDropBox_2.GetItems().Contains(item)){
            replacedDropBox_2.RemoveItem(item);
        }
        else if(replacedDropBox_3.GetItems().Contains(item)){
            replacedDropBox_3.RemoveItem(item);
        }
    }
    public void AddForecItems(){
        StartCoroutine(AddForceItemsSequentially());
    }
    
    private IEnumerator AddForceItemsSequentially(){
        // 1번 박스 아이템들에 힘 적용
        List<CarriedObject> items = replacedDropBox_1.GetItems();
        
        float zForce = 0f;
        float xForce = 0f;
        foreach(CarriedObject item in items){
            // 아이템의 Rigidbody 설정 변경
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if(rb != null){
                rb.isKinematic = false;
                
                // 부모 해제
                item.transform.parent = null;
                
                // -x, y, z 방향으로 힘 적용 (z축은 점점 감소)
                rb.AddForce(new Vector3(-30f + xForce, 15f, 50f + zForce), ForceMode.Impulse);
                
                // 다음 아이템은 z축 힘을 조금 더 감소시킴
                zForce -= 15f;
                xForce -= 5f;
            }
        }
        
        // 0.5초 대기
        yield return new WaitForSeconds(0.05f);
        
        // 2번 박스 아이템들에 힘 적용
        items = replacedDropBox_2.GetItems();
        
        zForce = 0f;
        xForce = 0f;
        foreach(CarriedObject item in items){
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if(rb != null){
                rb.isKinematic = false;
                item.transform.parent = null;
                rb.AddForce(new Vector3(-32f + xForce, 20f, 20f + zForce), ForceMode.Impulse);
                zForce -= 15f;
                xForce -= 3f;
            }
        }
        
        // 0.5초 대기
        yield return new WaitForSeconds(0.05f);
        
        // 3번 박스 아이템들에 힘 적용
        items = replacedDropBox_3.GetItems();
        
        zForce = 0f;
        xForce = 0f;
        foreach(CarriedObject item in items){
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if(rb != null){
                rb.isKinematic = false;
                item.transform.parent = null;
                rb.AddForce(new Vector3(-25f + xForce, 18f, 0f + zForce), ForceMode.Impulse);
                zForce -= 12f;
                xForce -= 8f;
            }
        }
    }

    public void AnimationOnTransition(){
        AddForecItems();
    }

    public void AnimationOnExit(){
        ReplaceDropBox();
    }
}
