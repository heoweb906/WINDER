using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartNPC_PositionSetting : MonoBehaviour
{
    public GameObject NPC;
    public Transform transformTarget;

    private void Start()
    {
        NPC.transform.position = transformTarget.position;  
    }

    private void OnEnable()
    {
        NPC.transform.position = transformTarget.position;
    }

}
