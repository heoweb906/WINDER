using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPoolManager : MonoBehaviour
{
    public static NPCPoolManager Instance { get; private set; }

    public GameObject[] NPC_Wandering; // NPC 프리팹들
    private Queue<GameObject> npcPool = new Queue<GameObject>(); // NPC 오브젝트 풀
    public int poolSize = 200; // 초기 풀 크기

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeNPCPool();
    }

    private void InitializeNPCPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int randomType = Random.Range(0, NPC_Wandering.Length);
            GameObject npc = Instantiate(NPC_Wandering[randomType]);
            npc.transform.SetParent(transform); // 부모 설정 (오브젝트 풀 정리)


            NPC_Simple npc_Simple = npc.GetComponent<NPC_Simple>();
            npc_Simple.machine.OnStateChange(npc_Simple.machine.WalkState);



            npc.SetActive(false);
            npcPool.Enqueue(npc);
        }
    }

    // 기존 GetPooledNPC 메서드 이름을 GetNPC로 변경
    public GameObject GetNPC(int npcType, Vector3 position)
    {
        GameObject npc;
        if (npcPool.Count > 0)
        {
            npc = npcPool.Dequeue();
        }
        else
        {
            npc = Instantiate(NPC_Wandering[npcType]); // 부족하면 새로 생성
            npc.transform.SetParent(transform); // 부모 설정
        }

        npc.transform.position = position; // 위치 설정
        npc.SetActive(true);
        return npc;
    }

    public void ReturnNPCToPool(GameObject npc)
    {
        npc.SetActive(false);
        npc.transform.SetParent(transform); // 부모 재설정
        npcPool.Enqueue(npc);
    }
}
