using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrossWayAssist : MonoBehaviour
{
    [Header("각 4분면")]
    public GameObject quadrant_1_NPCS;
    public GameObject quadrant_2_NPCS;
    public GameObject quadrant_3_NPCS;
    public GameObject quadrant_4_NPCS;

    private NPC_Simple[] npcs_Q1;
    private NPC_Simple[] npcs_Q2;
    private NPC_Simple[] npcs_Q3;
    private NPC_Simple[] npcs_Q4;


    [Header("목표 위치")]
    public Transform[] quadrant_1_First;
    public Transform[] quadrant_1_Second;
    public Transform[] quadrant_2_First;
    public Transform[] quadrant_2_Second;
    public Transform[] quadrant_3_First;
    public Transform[] quadrant_3_Second;
    public Transform[] quadrant_4_First;
    public Transform[] quadrant_4_Second;

    private void Awake()
    {
        npcs_Q1 = GetNPCsInChildren(quadrant_1_NPCS);
        npcs_Q2 = GetNPCsInChildren(quadrant_2_NPCS);
        npcs_Q3 = GetNPCsInChildren(quadrant_3_NPCS);
        npcs_Q4 = GetNPCsInChildren(quadrant_4_NPCS);
    }




    private NPC_Simple[] GetNPCsInChildren(GameObject parent)
    {
        if (parent == null) return new NPC_Simple[0];
        return parent.GetComponentsInChildren<NPC_Simple>(true); // true: 비활성화 포함
    }

    public void AssignCrossWayNPCs()
    {
        StartCoroutine(DistributeNPCsWithDelay(npcs_Q1, 1));
        StartCoroutine(DistributeNPCsWithDelay(npcs_Q2, 2));
        StartCoroutine(DistributeNPCsWithDelay(npcs_Q3, 3));
        StartCoroutine(DistributeNPCsWithDelay(npcs_Q4, 4));
    }

    private IEnumerator DistributeNPCsWithDelay(NPC_Simple[] npcs, int fromQuadrant)
    {
        List<int> otherQuadrants = new List<int> { 1, 2, 3, 4 };
        otherQuadrants.Remove(fromQuadrant);

        int countPerQuadrant = npcs.Length / 3;
        int extra = npcs.Length % 3;
        int npcIndex = 0;

        foreach (int toQuadrant in otherQuadrants)
        {
            int assignCount = countPerQuadrant + (extra-- > 0 ? 1 : 0);

            for (int i = 0; i < assignCount && npcIndex < npcs.Length; i++)
            {
                Transform[] first = GetFirstTransforms(toQuadrant);
                Transform[] second = GetSecondTransforms(toQuadrant);

                Transform firstPos = first[Random.Range(0, first.Length)];
                Transform secondPos = second[Random.Range(0, second.Length)];

                NPC_Simple npc = npcs[npcIndex++];
                npc.bWalking = true;
                npc.checkPoints = new Transform[] { firstPos, secondPos };
                npc.GetAnimator().SetInteger("IDLE_Num", 0);
                npc.GetAnimator().SetBool("Bool_Walk", true);
                npc.machine.OnStateChange(npc.machine.WalkState);

                // 랜덤한 텀을 줌
                yield return new WaitForSeconds(Random.Range(0.1f, 0.8f));
            }
        }
    }

    private Transform[] GetFirstTransforms(int quadrant)
    {
        switch (quadrant)
        {
            case 1: return quadrant_1_First;
            case 2: return quadrant_2_First;
            case 3: return quadrant_3_First;
            case 4: return quadrant_4_First;
            default: return new Transform[0];
        }
    }

    private Transform[] GetSecondTransforms(int quadrant)
    {
        switch (quadrant)
        {
            case 1: return quadrant_1_Second;
            case 2: return quadrant_2_Second;
            case 3: return quadrant_3_Second;
            case 4: return quadrant_4_Second;
            default: return new Transform[0];
        }
    }
}

