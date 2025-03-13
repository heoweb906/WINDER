using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Create_WanderingNPC : MonoBehaviour
{
    // NPC 프리팹들 (원래 NPC_Wandering)
    public GameObject[] npcPrefabs;
    public float spawnInterval = 0.3f;
    public bool bReverse;
    public WayPoints[] positionArray;
    public int poolSize = 30;
    private bool bPollFinish = false;

    // 로컬 풀: Create_WanderingNPC가 관리하는 NPC 오브젝트 풀
    private Queue<GameObject> npcPool = new Queue<GameObject>();
    private int iRandNum;

    void Start()
    {
        if (positionArray != null)
        {
            InitializeNPCPool();
            StartCoroutine(SpawnerNPCs());
        }
    }

    // 풀 초기화: 미리 일정 개수의 NPC를 생성해둠
    private void InitializeNPCPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int randomType = Random.Range(0, npcPrefabs.Length);
            GameObject npc = Instantiate(npcPrefabs[randomType]);
            npc.SetActive(false);
            npc.transform.SetParent(transform); // 풀 관리용 자식으로 설정

            npcPool.Enqueue(npc);
        }

        bPollFinish = true;
    }

    // 풀에서 NPC를 가져오거나, 풀에 없으면 새로 생성함
    private GameObject GetPooledNPC(int npcType, Vector3 position)
    {
        GameObject npc;
        if (npcPool.Count > 0)
        {
            npc = npcPool.Dequeue();
        }
        else
        {
            npc = Instantiate(npcPrefabs[npcType]);
            npc.transform.SetParent(transform);
        }
        npc.transform.position = position;
        npc.SetActive(true);

        // NavMeshAgent가 NavMesh 위에 있도록 강제로 이동
        NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(position);
        }

        return npc;
    }

    // NPC를 사용 후 풀로 반환 (다시 비활성화 후 Queue에 넣음)
    public void ReturnNPCToPool(GameObject npc)
    {
        npc.SetActive(false);
        npcPool.Enqueue(npc);
    }

    IEnumerator SpawnerNPCs()
    {
        while (bPollFinish)
        {
            if (!bReverse)
            {
                float randomValue = Random.value;
                int fRandomNPC = (randomValue < 0.95f) ? 0 : 1;
                int randomIndex = Random.Range(0, positionArray[0].points.Length);
                Transform spawnPosition = positionArray[0].points[randomIndex];

                // 풀에서 NPC 가져오기
                GameObject npc = GetPooledNPC(fRandomNPC, spawnPosition.position);
                npc.transform.SetParent(transform);

                NPC_Simple npcScript = npc.GetComponent<NPC_Simple>();

                // 목표 지점 설정
                List<Transform> tempCheckpoints = new List<Transform>();
                for (int i = 1; i < positionArray.Length; i++)
                {
                    iRandNum = Random.Range(0, positionArray[i].points.Length);
                    tempCheckpoints.Add(positionArray[i].points[iRandNum]);
                }
                npcScript.checkPoints = tempCheckpoints.ToArray();
            }
            else
            {
                float randomValue = Random.value;
                int fRandomNPC = (randomValue < 0.95f) ? 0 : 1;
                int lastIndex = positionArray.Length - 1;
                int randomIndex = Random.Range(0, positionArray[lastIndex].points.Length);
                Transform spawnPosition = positionArray[lastIndex].points[randomIndex];

                GameObject npc = GetPooledNPC(fRandomNPC, spawnPosition.position);
                npc.transform.SetParent(transform);

                NPC_Simple npcScript = npc.GetComponent<NPC_Simple>();

                List<Transform> tempCheckpoints = new List<Transform>();
                for (int i = lastIndex - 1; i >= 0; i--)
                {
                    iRandNum = Random.Range(0, positionArray[i].points.Length);
                    tempCheckpoints.Add(positionArray[i].points[iRandNum]);
                }
                npcScript.checkPoints = tempCheckpoints.ToArray();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
