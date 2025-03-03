using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideClearChecker : MonoBehaviour
{
    public bool bIsObject; // ������Ʈ�� �ƴѰ�쿡�� Checker�ν� ����

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

        Debug.Log("�������� Ŭ����");
    }








}
