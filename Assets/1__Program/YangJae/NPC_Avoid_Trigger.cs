using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Avoid_Trigger : MonoBehaviour
{
    public List<NPC_Simple> npcList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var npc in npcList)
            {
                npc.SetAvoidState();
            }
        }
    }
    
}
