using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCutSseneAssist_1_1 : MonoBehaviour
{
    public GameObject GyungSoo;


    public Create_WanderingNPC createNpc_1;
    public Create_WanderingNPC createNpc_2;

    private bool bFlag = false;


    private void Start()
    {
        if (SaveData_Manager.Instance.GetIntInside() == 3)
        {
            Debug.Log("이거 왜 작동안하냐");
            GyungSoo.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bFlag)
        {
            SaveData_Manager.Instance.SetIntInside(3);

            bFlag = true;


            createNpc_1.SetBoolCantCreateNpc(true);
            createNpc_2.SetBoolCantCreateNpc(true);


            createNpc_1.ReplaceLastTwoCheckpoints();
        }
    }


}