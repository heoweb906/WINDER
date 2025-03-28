using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.Unicode;

public class GameMachine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;


    [Header("게임기 관련")]
    private bool bPowerOn = false;


    [Header("전구 벌레 관련")]
    public bool[] bBulbBug;         // 전구 벌레가 장착된 상태인지
    public GameObject[] SpritesBulbBug;

    [Header("게임기")]
    public GameObject objGameMachine;
    public float launchForce;
    public float targetScale;
    public float scaleDuration;
    public Vector3 dirVec3;



    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(RebootingGameMachine());
    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        if (!bPowerOn)
        {
            base.TurnOffObj();
            BulbBugSpriteonOff(0, false, true);
        }
        else    // 충전에 성공한 경우
        {
            objGameMachine.transform.SetParent(null);

            // Rigidbody 가져오기
            Rigidbody rb = objGameMachine.GetComponent<Rigidbody>();
            CarriedObject carry = objGameMachine.GetComponent<CarriedObject>();
            if (rb == null) rb = objGameMachine.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            carry.canInteract = true;

            // 크기 조절 (DoTween 사용)
            objGameMachine.transform.DOScale(Vector3.one * targetScale, scaleDuration)
                .SetEase(Ease.OutBack);

            // 힘을 가해 발사
            rb.velocity = Vector3.zero; // 기존 속도 초기화
            rb.AddForce(dirVec3.normalized * launchForce, ForceMode.Impulse);


        }
    }




    IEnumerator RebootingGameMachine()
    {
        int iIndex = 0;
        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery >= 3 && CheckIfAnyFalse(bBulbBug))
            {
                bPowerOn = true;
            }
                
            yield return new WaitForSeconds(1.0f);

            if(iIndex <= 2) BulbBugSpriteonOff(iIndex, bBulbBug[iIndex]);
            iIndex++;

            fCurClockBattery -= 1;
        }

        yield return new WaitForSeconds(1.0f);

        TurnOffObj(); // 배터리가 다 되면 종료
    }


    private void BulbBugSpriteonOff(int index, bool bOnOff, bool bAllReset = false)
    {
        if(bAllReset)
        {
            foreach (GameObject sprite in SpritesBulbBug) sprite.SetActive(false);
            return;
        }
            
        if (index == 0)
        {
            if (bOnOff) SpritesBulbBug[0].SetActive(true);
            else SpritesBulbBug[3].SetActive(true);
        }
        else if(index == 1)
        {
            if (bOnOff) SpritesBulbBug[1].SetActive(true);
            else SpritesBulbBug[4].SetActive(true);
        }
        else if (index == 2)
        {
            if (bOnOff) SpritesBulbBug[2].SetActive(true);
            else SpritesBulbBug[5].SetActive(true);
        }
    }
    private bool CheckIfAnyFalse(bool[] bArray)
    {
        foreach (bool b in bArray)
        {
            if (!b)
            {
                return false;  // 하나라도 false가 있으면 false 반환
            }
        }
        return true;  // 모든 값이 true일 경우 true 반환
    }






    public void InsertOwnerFunc(GameObject bulbGub, int index)
    {
        bBulbBug[index] = true;
    }

    public void RemoveOwnerFunc(int index)
    {
        bBulbBug[index] = false;
    }
}
