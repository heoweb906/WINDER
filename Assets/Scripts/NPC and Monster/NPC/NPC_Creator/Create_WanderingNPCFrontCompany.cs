using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WayPoints
{
    public Transform[] points;
}

public class Create_WanderingNPCFrontCompany : MonoBehaviour
{
    public GameObject[] NPC_Wandering;
    public float spawnInterval = 0.3f;

    public bool bReverse;
    public WayPoints[] positionArray;       // 중간 순회 지점들
    public WayPoints positionLast;          // 마지막 순회 지점들



    private int iRandNum;
    public bool isSpawning;

    private List<int> previousRandNums = new List<int>();

    void Start()
    {
        if (positionArray != null) StartCoroutine(SpawnerNPCs_1());

    }
    IEnumerator SpawnerNPCs_1()
    {
        while (isSpawning)
        {
            float randomValue = Random.value;
            int fRandomNPC = (randomValue < 0.95f) ? 0 : 1;

            int randomIndex = Random.Range(0, positionArray[0].points.Length);
            Transform spawnPosition = positionArray[0].points[randomIndex];

            GameObject npc = Instantiate(NPC_Wandering[fRandomNPC], spawnPosition.position, Quaternion.identity);
            npc.transform.SetParent(transform);

            NPC_Simple nPC_Wandering = npc.GetComponent<NPC_Simple>();

            List<Transform> tempCheckpoints = new List<Transform>();

            int newRandNum = GetUniqueRandom(positionArray[2].points.Length);

            for (int i = 1; i < positionArray.Length; i++)
            {
                tempCheckpoints.Add(positionArray[i].points[newRandNum]);
            }

            randomIndex = Random.Range(0, positionLast.points.Length);
            tempCheckpoints.Add(positionLast.points[randomIndex]);

            nPC_Wandering.checkPoints = tempCheckpoints.ToArray();

            yield return new WaitForSeconds(spawnInterval);
        }
    }



    public void NPCCreatOff_Sacred()
    {
        isSpawning = false;

        foreach (NPC_Simple npcScript in GetComponentsInChildren<NPC_Simple>())
        {
            npcScript.machine.OnStateChange(npcScript.machine.ScaredState);
        }

    }

    public void NPCCreatOn()
    {
        isSpawning = true;

        foreach (NPC_Simple npcScript in GetComponentsInChildren<NPC_Simple>())
        {
            npcScript.machine.OnStateChange(npcScript.machine.IDLEState);
        }

    }
        

  

    int GetUniqueRandom(int maxValue)
    {
        int randNum;
        do
        {
            randNum = Random.Range(0, maxValue);
        } while (previousRandNums.Contains(randNum));  // 이전에 생성된 값이 리스트에 있으면 다시 랜덤 생성

        previousRandNums.Add(randNum);  // 새로 생성된 랜덤값을 리스트에 추가

        // 리스트에 저장된 값이 너무 많으면 오래된 값 삭제
        if (previousRandNums.Count > 2)  // 앞서 2번의 값을 기억한다고 가정
        {
            previousRandNums.RemoveAt(0);  // 가장 오래된 값 제거
        }

        return randNum;
    }
}
