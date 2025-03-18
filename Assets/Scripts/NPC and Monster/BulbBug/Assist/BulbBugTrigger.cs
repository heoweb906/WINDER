using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbBugTrigger : MonoBehaviour
{
    public BulbBug_OnRoad[] BulbBugs;
    public Transform[] transforms_Target;

    private bool bTrigger = false;


    private void Awake()
    {
        BulbBugs = GetComponentsInChildren<BulbBug_OnRoad>();

        if (BulbBugs == null)
        {
            Debug.Log("아무 벌레도 감지되지 않습니다");
            return;
        }
        for (int i = 0; i < BulbBugs.Length; ++i)
        {
            BulbBugs[i].checkPoints = transforms_Target;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BulbBugsRunEventOn();
        }
    }


    private void BulbBugsRunEventOn()
    {
        if (bTrigger) return;

        bTrigger = true;
        if (BulbBugs == null)
        {
            Debug.Log("아무 벌레도 감지되지 않습니다");
            return;
        }
        for(int i = 0; i < BulbBugs.Length; ++i)
        {
            BulbBugs[i].transform.SetParent(null);
            BulbBugs[i].machine.OnStateChange(BulbBugs[i].machine.RunState);
        }


        


    }
}
