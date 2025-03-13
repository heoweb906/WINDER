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
    private bool isSpawning = true;

    private List<int> previousRandNums = new List<int>();

    void Start()
    {
        isSpawning = true;
        if (positionArray != null) StartCoroutine(SpawnerNPCs_1());

    }

    IEnumerator SpawnerNPCs_1()
    {
        while (isSpawning)
        {
            float randomValue = Random.value; // 0.0 �̻� 1.0 �̸��� ����
            int fRandomNPC = (randomValue < 0.95f) ? 0 : 1;

            int randomIndex = Random.Range(0, positionArray[0].points.Length);
            Transform spawnPosition = positionArray[0].points[randomIndex];


            GameObject npc = Instantiate(NPC_Wandering[fRandomNPC], spawnPosition.position, Quaternion.identity);
            npc.transform.SetParent(transform);
            NPC_Simple nPC_Wanderring = npc.GetComponent<NPC_Simple>();

            // ����Ʈ�� ����Ͽ� ��ǥ �������� �߰�
            List<Transform> tempCheckpoints = new List<Transform>();

            int newRandNum = GetUniqueRandom(positionArray[2].points.Length);

            for (int i = 1; i < positionArray.Length; i++)
            {
                tempCheckpoints.Add(positionArray[i].points[newRandNum]);
            }

            randomIndex = Random.Range(0, positionLast.points.Length);
            tempCheckpoints.Add(positionLast.points[randomIndex]);

            nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();
            //int lastPointIndex = Random.Range(0, positionLast.points.Length);
            //tempCheckpoints.Add(positionLast.points[lastPointIndex]);

            //nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();

            // �ռ���

            yield return new WaitForSeconds(spawnInterval);
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
