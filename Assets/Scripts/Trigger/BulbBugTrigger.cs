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
            Debug.Log("�ƹ� ������ �������� �ʽ��ϴ�");
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
            Debug.Log("�ƹ� ������ �������� �ʽ��ϴ�");
            return;
        }
        for(int i = 0; i < BulbBugs.Length; ++i)
        {
            BulbBugs[i].transform.SetParent(null);
            BulbBugs[i].machine.OnStateChange(BulbBugs[i].machine.RunState);
        }


        


    }
}
