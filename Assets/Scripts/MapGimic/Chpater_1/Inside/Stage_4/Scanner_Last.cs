using System.Collections.Generic;
using UnityEngine;

public class Scanner_Last : MonoBehaviour
{
    public List<ColorObj> colorObjList = new List<ColorObj>();
    public GameObject testEffect;

    private bool bPlayerIn = false;
    private GameObject playerColorObj;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();
            colorObjList.Add(colorObj_);
            testEffect.SetActive(true);
        }

        if (other.CompareTag("Player") && !bPlayerIn)
        {
            bPlayerIn = true;

            playerColorObj = new GameObject("PlayerColorObj");
            playerColorObj.AddComponent<ColorObj>();
            ColorObj newColorObj = playerColorObj.GetComponent<ColorObj>();
            newColorObj.colorType = ColorType.Red;

            colorObjList.Add(newColorObj);

            testEffect.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();
            if (colorObjList.Contains(colorObj_))
            {
                colorObjList.Remove(colorObj_);
            }

            if (colorObjList.Count == 0) testEffect.SetActive(false);
        }

        if (other.CompareTag("Player") && bPlayerIn)
        {
            bPlayerIn = false;

            if (playerColorObj != null)
            {
                Destroy(playerColorObj);  
                playerColorObj = null; 
            }

            ColorObj playerColorObj_ = colorObjList.Find(c => c.colorType == ColorType.Red);
            if (playerColorObj_ != null)
            {
                colorObjList.Remove(playerColorObj_);
            }

            if (colorObjList.Count == 0) testEffect.SetActive(false);  // 이 부분을 원하는 로직으로 수정 가능

        }
    }


    public List<ColorObj> GetColorObjList()
    {
        if (colorObjList != null) return colorObjList;
        return null;
    }


    public GameObject GetTopGameObject()
    {
        if (colorObjList != null && colorObjList.Count > 0)
        {
            return colorObjList[0].gameObject;  // 첫 번째 ColorObj의 GameObject 반환
        }
        return null;  // 리스트가 비었을 경우 null 반환
    }
}
