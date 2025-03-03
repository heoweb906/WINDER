using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToyTruck : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    public GameObject Trunk;            

    private Vector3 originRotate;
    private Vector3 currentRotate;
    private bool isShaking = false;         // 덜덜거림 상태 확인

    public GameObject[] Baggages;           // 날려버릴 장난감들
    public float throwForce = 10f; // 물체를 날릴 힘
    public Vector3 throwDirection; // 물체를 날릴 방향



    private void Awake()
    {
        originRotate = Trunk.transform.localRotation.eulerAngles;
        currentRotate = originRotate; // 현재 로테이션을 초기값으로 설정
    }



    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(ActiveTrunk());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        if (nowCoroutine != null)
        {
            StopCoroutine(nowCoroutine);
            nowCoroutine = null;
        }

        ResetTrunkRotation(); // 트렁크를 초기 상태로 복원
    }


    // #. 트렁크가 원해 회전 값으로 돌아옴
    private void ResetTrunkRotation()
    {
        if (Trunk != null)
        {
            // DOTween으로 원래 상태로 부드럽게 복원
            Trunk.transform.DOLocalRotate(originRotate, 0.5f).SetEase(Ease.OutQuad);
            currentRotate = originRotate; // 현재 회전 상태도 원래 상태로 설정
        }
    }


    IEnumerator ActiveTrunk()
    {
        float zRotation = currentRotate.z; // 현재 Z축 로테이션 값 (현재 로테이션을 기준으로)

        while (fCurClockBattery > 0)
        {
            

            if (Trunk != null)
            {
                zRotation += 2f; // 회전 값 증가
                currentRotate.z = zRotation; // 현재 로테이션 업데이트
                Trunk.transform.DOLocalRotate(currentRotate, 0.5f)
                    .SetEase(Ease.Linear);

                if (zRotation >= 6f && !isShaking)
                {
                    isShaking = true;
                    yield return StartCoroutine(ShakeTrunk());
                    isShaking = false;


                    ThrowBaggages();
                    float targetRotation = 40f;
                    currentRotate.z = targetRotation; // 새로운 목표 회전 값을 설정
                    Trunk.transform.DOLocalRotate(currentRotate, 1f)
                    .SetEase(Ease.OutBack);
               

                    yield return new WaitForSecondsRealtime(2.0f);
                    
                    StopCoroutine(nowCoroutine);
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(1.0f); // 1초 대기

            fCurClockBattery -= 1;
            // 배터리가 다 되면 초기 상태로 복원
            if (fCurClockBattery <= 0)
            {
                fCurClockBattery = 0;
                ResetTrunkRotation();
                TurnOffObj();
                yield break;
            }

            
        }

        TurnOffObj(); // 배터리가 다 되면 종료
    }


    IEnumerator ShakeTrunk()
    {
        if (Trunk != null)
        {
            float duration = 1.2f; // 흔들림 시간
            float strength = 0.2f; // 흔들림 강도
            int vibrato = 10; // 진동 횟수 (빈도)

            // 트렁크의 현재 회전 상태를 기준으로 시작
            currentRotate = Trunk.transform.localRotation.eulerAngles;

            float elapsedTime = 0f;
            float shakeDuration = duration; // 흔들림 지속 시간
            float shakeStrength = strength; // 흔들림 강도

            // 흔들림 효과 부드럽게 구현
            while (elapsedTime < shakeDuration)
            {
                // 사인파를 이용한 자연스러운 흔들림
                float sineValue = Mathf.Sin(elapsedTime * vibrato * Mathf.PI * 2f);
                float randomZ = sineValue * shakeStrength; // 사인파에 강도를 곱함

                // 현재 회전값에서 Z축 회전을 변동시킴
                currentRotate.z = currentRotate.z + randomZ;

                Trunk.transform.localRotation = Quaternion.Euler(currentRotate);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 흔들림이 끝난 후 원래 회전 값으로 복원
            Trunk.transform.localRotation = Quaternion.Euler(originRotate);
        }
    }



    private void ThrowBaggages()
    {
        foreach (GameObject baggage in Baggages)
        {
            Rigidbody rb = baggage.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float randomX = Random.Range(-3f, 3f);
                float randomZ = Random.Range(-3f, 3f);

                float randomThrowForce = Random.Range(-2f, 1f);

                Vector3 modifiedThrowDirection = throwDirection + new Vector3(randomX, 0f, randomZ);

                rb.AddForce(modifiedThrowDirection * (throwForce + randomThrowForce), ForceMode.Impulse);
            }
        }
    }

    public void InsertOwnerFunc(GameObject parts, int iIndex = 0)
    {
        //throw new System.NotImplementedException();
    }

    public void RemoveOwnerFunc(int iIndex = 0)
    {
        //throw new System.NotImplementedException();
    }
} 
