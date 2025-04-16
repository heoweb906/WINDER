using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanMaster_Last : MonoBehaviour
{
    public ColorType[] ColorCorrects;       // ScanMaster�� ���� �÷�
    public Scanner_Last[] scanners;              // �� ���������� Scanner 

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


    // ��ĳ�� ���� ������Ʈ���� ������ ����� ��ġ�ϴ��� �˻��ϴ� �Լ�
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



    // ��ĳ�� ������ ���� �Լ�
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
                    Debug.Log("���������� �����");
                    dissolveTest.Reset();
                    dissolveTest.Dissolve();
                }
                
                   
            }

        }
    }


}
