using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 이 클래스는 더 이상 실제 이동 로직을 포함하지 않습니다.
// 기존 코드는 Event_BeforeStation_Controller로 이동되었습니다.
public class NPC_Event_BeforeStation : MonoBehaviour
{
    public NPC_Simple npc;

    private void Start()
    {
        // 자동으로 NPC_Simple 컴포넌트를 가져옵니다.
        if (npc == null)
        {
            npc = GetComponent<NPC_Simple>();
        }
    }

    public void GoToDropItem()
    {
    }

    public void GoToDropBox()
    {
    }
}
