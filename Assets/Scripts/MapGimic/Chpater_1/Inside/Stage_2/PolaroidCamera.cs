using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidCamera : ClockBattery
{
    public PolaroidScanner[] polaroidScanners;
    public int[] iCorrectSequence;

    [Header("사진 생성 관리")]
    public Transform transformPictureCreate;
    public GameObject Picture_Complete;         // 사진 완성본
    public GameObject Picture_Fail;         // 사진 조각들을 묶어줄 오브젝트


    private Coroutine nowCoroutine;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(ShutterCountStart());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        PrintPicture(Picture_Complete);

        // ShootCamera();
    }

    // #. 카메라 셔터 카운트 다운
    IEnumerator ShutterCountStart()
    {
        while (fCurClockBattery > 0)
        {
            yield return new WaitForSeconds(1f);
            fCurClockBattery -= 1;
        }

        TurnOffObj();
    }

    

    // #. 카메라 촬영
    private void ShootCamera()
    {
        for(int i = 0; i < 4; i++)
        {
            if (polaroidScanners[i].iFigureIndex != iCorrectSequence[i])
            {
                PrintPicture(Picture_Fail);
                return;
            }
        }

        PrintPicture(Picture_Complete);
    }







    // #. 사진 출력 (GameObject 출력할 사진)
    private void PrintPicture(GameObject picture_)
    {
        // 사진을 Y축으로 90도 회전한 상태로 생성
        GameObject spawnedObject = Instantiate(picture_, transformPictureCreate.position, Quaternion.Euler(90, 0, 0));

        // Rigidbody 참조
        Rigidbody objRigidbody = spawnedObject.GetComponent<Rigidbody>();

        // 초기 위치를 설정 (Z축 앞으로 이동)
        Vector3 startPosition = transformPictureCreate.position;
        startPosition.z += 1.5f; // 앞쪽에서 시작
        spawnedObject.transform.position = startPosition;

        // Rigidbody 초기화 설정
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = true; // DOTween 동안 물리 효과 비활성화
        }

        // 목표 위치 설정 (앞으로 이동)
        Vector3 targetPosition = transformPictureCreate.position;
        targetPosition.z -= 1.2f; // 앞으로 이동

        // DOTween으로 부드러운 곡선 발사 애니메이션
        spawnedObject.transform.DOPath(
            new Vector3[] { startPosition, targetPosition },
            1f, // 애니메이션 지속 시간
            PathType.CatmullRom)
            .SetEase(Ease.OutQuad) // 자연스럽게 발사
            .OnComplete(() =>
            {
                // 애니메이션 완료 후 물리 효과 활성화
                if (objRigidbody != null)
                {
                    objRigidbody.isKinematic = false; // 물리 작용 활성화
                    objRigidbody.useGravity = true;  // 중력 활성화
                    objRigidbody.AddForce(new Vector3(0f, 0f, -2f), ForceMode.Impulse); // 앞으로 추가 힘
                }
            });
    }


}
