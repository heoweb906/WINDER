using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class WayPoints
{
    public Transform[] points;
}

public class Create_WanderingNPC : MonoBehaviour
{
    public GameObject[] NPC_Wandering;
    public float spawnInterval = 0.3f;

    public bool bReverse;
    public WayPoints[] positionArray;
    

    private int iRandNum;

    void Start()
    {
        if(positionArray != null)  
            StartCoroutine(SpawnerNPCs_1());
    }

    IEnumerator SpawnerNPCs_1()
    {
        while (true)
        {
            Debug.Log("NPC를 생성하였습니다.");

            if (!bReverse)
            { 
                float randomValue = Random.value; // 0.0 이상 1.0 미만의 난수
                int fRandomNPC = (randomValue < 0.95f) ? 0 : 1;

                int randomIndex = Random.Range(0, positionArray[0].points.Length);
                Transform spawnPosition = positionArray[0].points[randomIndex];
                

                GameObject npc = Instantiate(NPC_Wandering[fRandomNPC], spawnPosition.position, Quaternion.identity);
                npc.transform.SetParent(transform);
                NPC_Simple nPC_Wanderring = npc.GetComponent<NPC_Simple>();

                // 리스트를 사용하여 목표 지점들을 추가
                List<Transform> tempCheckpoints = new List<Transform>();

                for (int i = 1; i < positionArray.Length; i++)
                {
                    iRandNum = Random.Range(0, positionArray[i].points.Length);
                    tempCheckpoints.Add(positionArray[i].points[iRandNum]);
                }

                nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();
            }
            else
            {
                // 반대 방향에서 NPC를 생성
                float randomValue = Random.value; // 0.0 이상 1.0 미만의 난수
                int fRandomNPC = (randomValue < 0.95f) ? 0 : 1;

                int lastIndex = positionArray.Length - 1;
                int randomIndex = Random.Range(0, positionArray[lastIndex].points.Length);
                Transform spawnPosition = positionArray[lastIndex].points[randomIndex];

                GameObject npc = Instantiate(NPC_Wandering[fRandomNPC], spawnPosition.position, Quaternion.identity);
                npc.transform.SetParent(transform);
                NPC_Simple nPC_Wanderring = npc.GetComponent<NPC_Simple>();

                List<Transform> tempCheckpoints = new List<Transform>();
                for (int i = lastIndex - 1; i >= 0; i--)
                {
                    iRandNum = Random.Range(0, positionArray[i].points.Length);
                    tempCheckpoints.Add(positionArray[i].points[iRandNum]);
                }

                nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();
            }



            yield return new WaitForSeconds(spawnInterval);
        }
    }

   


}
