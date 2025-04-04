using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideClearChecker : MonoBehaviour
{
    public bool bIsObject; // 오브젝트가 아닌경우에만 Checker로써 동작

    private bool bInObject;
    private bool bInPlayer;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InsideClearChecker>() != null)
        {
            InsideClearChecker _checker = other.GetComponent<InsideClearChecker>();
            if(_checker.bIsObject == true && !bInObject)
            {
                bInObject = true;
                DirectMapChange();
            }
            
        }

        if (other.CompareTag("Player") && !bInPlayer)
        {
            bInPlayer = true;
            DirectMapChange();
        }
    }

    
    private void DirectMapChange()
    {
        if (bInObject == false || bInPlayer == false) return;

        Debug.Log("스테이지 클리어");
    }








}
