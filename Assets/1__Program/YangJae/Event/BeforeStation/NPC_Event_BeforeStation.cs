using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 이 클래스는 더 이상 실제 이동 로직을 포함하지 않습니다.
// 기존 코드는 Event_BeforeStation_Controller로 이동되었습니다.
public class NPC_Event_BeforeStation : MonoBehaviour
{
    public NPC_Simple npc;

    private CarriedObject item;
    private ParsArea_DropBox dropBox;
    [SerializeField]
    private Transform itemPos;

    private void Start()
    {
        // 자동으로 NPC_Simple 컴포넌트를 가져옵니다.
        if (npc == null)
        {
            npc = GetComponent<NPC_Simple>();
        }
    }

    public void SetItem(CarriedObject item, ParsArea_DropBox dropBox){
        this.item = item;
        this.dropBox = dropBox;
    }

    public void PickUpItem_OnTransition()
    {
        item.transform.parent = itemPos;
        item.transform.DOLocalMove(Vector3.zero, 0.3f);
    }

    public void PickUpItem_OnExit()
    {
        DOTween.To(() => npc.GetAnimator().GetLayerWeight(1), x => npc.GetAnimator().SetLayerWeight(1, x), 1, 0.3f);
    }

    public void PutDownItem_OnTransition()
    {   
        item.transform.parent = dropBox.transform;
        DOTween.To(() => npc.GetAnimator().GetLayerWeight(1), x => npc.GetAnimator().SetLayerWeight(1, x), 0, 0.3f);
    }

    public void PutDownItem_OnExit()
    {
        item.transform.DOLocalMove(Vector3.zero, 0.3f);
    }

}
