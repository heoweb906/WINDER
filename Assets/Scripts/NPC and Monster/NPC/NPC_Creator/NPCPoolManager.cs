using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPoolManager : MonoBehaviour
{
    public static NPCPoolManager Instance { get; private set; }

    public GameObject[] NPC_Wandering; // NPC �����յ�
    private Queue<GameObject> npcPool = new Queue<GameObject>(); // NPC ������Ʈ Ǯ
    public int poolSize = 200; // �ʱ� Ǯ ũ��

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
            npc.transform.SetParent(transform); // �θ� ���� (������Ʈ Ǯ ����)


            NPC_Simple npc_Simple = npc.GetComponent<NPC_Simple>();
            npc_Simple.machine.OnStateChange(npc_Simple.machine.WalkState);



            npc.SetActive(false);
            npcPool.Enqueue(npc);
        }
    }

    // ���� GetPooledNPC �޼��� �̸��� GetNPC�� ����
    public GameObject GetNPC(int npcType, Vector3 position)
    {
        GameObject npc;
        if (npcPool.Count > 0)
        {
            npc = npcPool.Dequeue();
        }
        else
        {
            npc = Instantiate(NPC_Wandering[npcType]); // �����ϸ� ���� ����
            npc.transform.SetParent(transform); // �θ� ����
        }

        npc.transform.position = position; // ��ġ ����
        npc.SetActive(true);
        return npc;
    }

    public void ReturnNPCToPool(GameObject npc)
    {
        npc.SetActive(false);
        npc.transform.SetParent(transform); // �θ� �缳��
        npcPool.Enqueue(npc);
    }
}
