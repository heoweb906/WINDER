using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkAssist : MonoBehaviour
{
    public NPC_Simple npc;
    public ClockWork clockwork;


    private void Awake()
    {
        npc.ClockWork = clockwork;
    }

   
}
