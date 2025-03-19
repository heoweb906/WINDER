using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayPlayerCheckPoint_2 : MonoBehaviour
{
    public Transform transform_teleportTarget;
    public Transform[] transforms_Teleport;

    public Train_2 trains_2;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerInHierarchy(other.transform) && !SubWayAssist.Instance.bPlayerTakeTrain)
        {
            SubWayAssist.Instance.bPlayerTakeTrain = true;

            transform_teleportTarget.position = transforms_Teleport[SubWayAssist.Instance.iCrowedRanNum].position;

            Invoke("StartTrain_2", 6f);
        }
    }

    // 자식, 부모 모두 포함해서 "Player" 태그를 가진 오브젝트가 있는지 검사
    private bool IsPlayerInHierarchy(Transform current)
    {
        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                return true;
            }
            current = current.parent;
        }
        return false;
    }


    private void StartTrain_2()
    {
        trains_2.StartTrain();
    }

}
