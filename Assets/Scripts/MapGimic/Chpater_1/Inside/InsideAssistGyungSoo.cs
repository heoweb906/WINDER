using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InsideAssistGyungSoo : MonoBehaviour
{
    public GameObject[] Stage_1;
    public GameObject[] Stage_2;
    public GameObject[] Stage_3;
    public GameObject[] Stage_4;

    public static InsideAssistGyungSoo Instance { get; private set; }

    private void Awake()
    {
        Instance = this; // 인스턴스 생성
     
    }

    public void StageSetActiveFalse(int iIndex)
    {
        GameObject[] targetArray = GetStageArray(iIndex);
        if (targetArray == null) return;

        foreach (GameObject obj in targetArray)
        {
            if (obj != null)
            {
                // DOTween 애니메이션 정지 (자식 포함)
                DOTween.Kill(obj, true);
                obj.SetActive(false);
            }
        }
    }

    public void StageSetActiveTrue(int iIndex)
    {
        GameObject[] targetArray = GetStageArray(iIndex);
        if (targetArray == null) return;

        foreach (GameObject obj in targetArray)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    private GameObject[] GetStageArray(int iIndex)
    {
        switch (iIndex)
        {
            case 1: return Stage_1;
            case 2: return Stage_2;
            case 3: return Stage_3;
            case 4: return Stage_4;
            default:
                Debug.LogWarning("Invalid stage index: " + iIndex);
                return null;
        }
    }


}
