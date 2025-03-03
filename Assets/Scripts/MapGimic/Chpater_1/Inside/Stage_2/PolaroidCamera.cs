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
    public GameObject Picture_Grouping;         // 사진 조각들을 묶어줄 오브젝트
    public GameObject[] PicturePieces;         // 사진 조각들 (잘못된 사진을 출력했을 때)



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

        ShootCamera();
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
                FailShoot();
                return;
            }
        }

        SuccesShoot();
    }



    // #. 촬영 성공
    private void SuccesShoot()
    {


        
        PrintPicture(Picture_Complete);          // 완성된 사진 출력
    }

    // #. 촬영 실패
    private void FailShoot()
    {




        StartCoroutine(FailShootCoroutine());    // 실패한 사진 조각들 출력
    }
    private IEnumerator FailShootCoroutine()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < 4;)
        {
            tempList.Add(polaroidScanners[i].iFigureIndex);
            int temp = i;

            for (int j = 1; j < 4; j++)
            {
                if (temp + j < 4)
                {
                    if (polaroidScanners[temp].iFigureIndex == 0) break; // 만약 요소가 0번이라면 무시

                    if (polaroidScanners[temp].iFigureIndex == polaroidScanners[temp + j].iFigureIndex - j)
                    {
                        tempList.Add(polaroidScanners[temp + j].iFigureIndex);
                        i++;
                    }
                    else break;
                }
            }

            PrintPicture(Picture_Grouping, tempList);
            tempList.Clear();

            yield return new WaitForSeconds(1); // 1초 대기

            i++;
        }
    }









    // #. 사진 출력 (GameObject 출력할 사진)
    private void PrintPicture(GameObject picture_, List<int> _intList = null)
    {
        GameObject spawnedObject = Instantiate(picture_, transformPictureCreate.position, Quaternion.identity);

        if (_intList != null)
        {
            float spacing = 0.2f; // 간격 설정 (필요에 따라 조정 가능)
            int count = 0; // 자식 오브젝트의 순서를 추적

            foreach (int index in _intList)
            {
                if (index >= 0 && index < PicturePieces.Length) // 유효한 인덱스인지 확인
                {
                    GameObject piece = Instantiate(PicturePieces[index]);
                    piece.transform.SetParent(spawnedObject.transform);

                    // X 축 방향으로 간격을 주어 배치
                    piece.transform.localPosition = new Vector3(count * spacing, 0, 0);

                    count++; // 다음 자식 오브젝트의 위치를 위해 증가
                }
            }
        }


        // Rigidbody 참조
        Rigidbody objRigidbody = spawnedObject.GetComponent<Rigidbody>();

        // 초기 위치를 설정 (X축으로 오른쪽으로 이동)
        Vector3 startPosition = transformPictureCreate.position;
        startPosition.x += 2f; // 오른쪽에서 시작
        spawnedObject.transform.position = startPosition;

        // Rigidbody 초기화 설정
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = true; // DOTween 동안 물리 효과 비활성화
        }

        // 목표 위치 설정 (왼쪽으로 발사)
        Vector3 targetPosition = transformPictureCreate.position;
        //targetPosition.x -= 1f; // 왼쪽으로 이동

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
                    objRigidbody.AddForce(new Vector3(-2f, 0f, 0f), ForceMode.Impulse); // 왼쪽으로 추가 힘
                }
            });
    }


}
