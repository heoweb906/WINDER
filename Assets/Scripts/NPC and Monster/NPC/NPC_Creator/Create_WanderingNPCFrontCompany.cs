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
    public WayPoints[] positionArray;       // �߰� ��ȸ ������
    public WayPoints positionLast;          // ������ ��ȸ ������



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
        } while (previousRandNums.Contains(randNum));  // ������ ������ ���� ����Ʈ�� ������ �ٽ� ���� ����

        previousRandNums.Add(randNum);  // ���� ������ �������� ����Ʈ�� �߰�

        // ����Ʈ�� ����� ���� �ʹ� ������ ������ �� ����
        if (previousRandNums.Count > 2)  // �ռ� 2���� ���� ����Ѵٰ� ����
        {
            previousRandNums.RemoveAt(0);  // ���� ������ �� ����
        }

        return randNum;
    }
}
