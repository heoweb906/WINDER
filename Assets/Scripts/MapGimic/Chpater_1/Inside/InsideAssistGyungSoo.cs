using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideAssistGyungSoo : MonoBehaviour
{
    public GameObject[] Stage_1;
    public GameObject[] Stage_2;
    public GameObject[] Stage_3;
    public GameObject[] Stage_4;



    public void StageSetActiveFalse(int iIndex)
    {
        GameObject[] targetArray = null;

        switch (iIndex)
        {
            case 1:
                targetArray = Stage_1;
                break;
            case 2:
                targetArray = Stage_2;
                break;
            case 3:
                targetArray = Stage_3;
                break;
            case 4:
                targetArray = Stage_4;
                break;
            default:
                Debug.LogWarning("Invalid stage index: " + iIndex);
                return;
        }

        foreach (GameObject obj in targetArray)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void StageSetActiveTrue(int iIndex)
    {
        GameObject[] targetArray = null;

        switch (iIndex)
        {
            case 1:
                targetArray = Stage_1;
                break;
            case 2:
                targetArray = Stage_2;
                break;
            case 3:
                targetArray = Stage_3;
                break;
            case 4:
                targetArray = Stage_4;
                break;
            default:
                Debug.LogWarning("Invalid stage index: " + iIndex);
                return;
        }

        foreach (GameObject obj in targetArray)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }



}
