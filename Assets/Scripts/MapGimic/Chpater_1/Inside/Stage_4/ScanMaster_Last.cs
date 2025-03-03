using Keto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanMaster_Last : MonoBehaviour
{
    public ColorType[] ColorCorrects;       // ScanMaster의 정답 컬러
    public Scanner_Last[] scanners;              // 각 스테이지의 Scanner 

    private bool bWork = false;

    // Update is called once per frame
    void Update()
    {
        if(!bWork && BoolCheckObjOnScanner())
        {
            bWork = true;
            DirectLastScanner();
        }
    }


    // 스캐너 위의 오브젝트들의 색상이 정답과 일치하는지 검사하는 함수
    private bool BoolCheckObjOnScanner()
    {
        for (int i = 0; i < ColorCorrects.Length; i++)
        {
            bool matchFound = false;

            if (scanners[i] != null)
            {
                foreach (var colorObj in scanners[i].GetColorObjList())
                {
                    if (colorObj != null && colorObj.colorType == ColorCorrects[i])
                    {
                        matchFound = true;
                        break;
                    }
                }
            }
            if (!matchFound) return false;
        }

        return true;
    }



    // 스캐너 마지막 연출 함수
    private void DirectLastScanner()
    {
        foreach (var scanner in scanners)
        {
            GameObject topGameObject = scanner.GetTopGameObject();

            if (topGameObject != null)
            {
                DissolveCustom dissolveTest = topGameObject.GetComponent<DissolveCustom>();

                if (dissolveTest != null)
                {
                    Debug.Log("정상적으로 실행됨");
                    dissolveTest.Reset();
                    dissolveTest.Dissolve();
                }
                
                   
            }

        }
    }


}
