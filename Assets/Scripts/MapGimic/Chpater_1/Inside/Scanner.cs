using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Scanner : MonoBehaviour
{
    public List<ColorObj> colorObjList = new List<ColorObj>();
    public SpriteRenderer sprite_Obj;

    // 기능 확인용으로 있는 스크립트, 나중에 지워야함
    public GameObject testEffect;

    // #. 스캐너 위에 있는 ColorObjList의 정보를 가져옴
    public List<ColorObj> GetColorObjList()
    {
        if (colorObjList != null) return colorObjList;
        return null;
    }



    // #. 특정한 컬러를 제외한 ColorObj를 모두 날려버리는 함수
    public void ThrowOtherColorObj(ColorType colorType = ColorType.None)
    {
        for (int i = colorObjList.Count - 1; i >= 0; i--)
        {
            if (colorObjList[i] != null && colorObjList[i].colorType != colorType)
            {
                Rigidbody objRigidbody = colorObjList[i].GetComponent<Rigidbody>();
                if (objRigidbody == null) objRigidbody = colorObjList[i].gameObject.AddComponent<Rigidbody>();

                Vector3 throwDirection = (transform.forward * -1f + transform.up * 3f).normalized;
                objRigidbody.AddForce(throwDirection * 40f, ForceMode.Impulse);
            }
        }
    }


    // #. Image의 RGB 변경 함수
    public void ChangeImageColor(float duration, Color targetColor)
    {
        if (sprite_Obj != null)
        {
            sprite_Obj.DOColor(targetColor, duration);
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();

            colorObjList.Add(colorObj_);
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

            if (colorObjList.Count == 0)
            {
                testEffect.SetActive(false);
            }
        }
    }


    public void ScannerCleaner()
    {
        for (int i = colorObjList.Count - 1; i >= 0; i--)
        {
            if (colorObjList[i] == null || colorObjList[i].colorType != ColorType.Yellow)
            {
                colorObjList.RemoveAt(i);
            }
        }

        if (colorObjList.Count == 0)
        {
            testEffect.SetActive(false);
        }
    }



}
