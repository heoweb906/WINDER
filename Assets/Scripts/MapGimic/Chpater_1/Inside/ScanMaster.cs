using DG.Tweening;
using System.Collections;
using UnityEngine;

public enum ColorType
{
    None,
    Yellow,
    Red
}

public class ScanMaster : ClockBattery
{
    private Coroutine nowCoroutine;

    [Header("스탠 기능 관련")]
    public ColorType[] ColorCorrects;       // ScanMaster의 정답 컬러
    public Scanner[] scanners;              // 각 스테이지의 Scanner 

    [Header("MasterObj 관련")]
    public GameObject masterObject;         // 스캔에 성공하면 획득할 수 있는 오브젝트
    public Transform transformMasterObj;    // 마스터 오브젝트 생성 위치


    [Header("Door 관련")]
    public GameObject Door_1;
    public GameObject Door_2;
    public Transform transformTarget_1;
    public Transform transformTarget_2;
    public float duration = 1.0f; // 문이 열리는 시간
    private Vector3 door1StartPos;
    private Vector3 door2StartPos;


    // 테스트로 사용할 이미지
    public GameObject[] testFaces;


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(ScanStart_());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        if (nowCoroutine != null)
        {
            StopCoroutine(nowCoroutine);
            nowCoroutine = null;
        }
    }




    // #. ScanMaster 메인 스캔 동작

    private IEnumerator ScanStart_()
    {


        if (BoolCheckObjOnScanner())  // 스캔 성공
        {
            Scan_Success();
            yield break;
        }
        else
        {
            Scan_Fail();
        }

        yield return new WaitForSeconds(1f);

        Scan_Reset();
    }







    // #. 스캐너 위의 오브젝트들의 색상이 정답과 일치하는지 검사하는 함수
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



    // #. 스캔 성공!
    private void Scan_Success()
    {
        // 마스터 오브젝트 생성
        GameObject spawnedObject = Instantiate(masterObject, transformMasterObj.position, Quaternion.identity);
        Collider objCollider = spawnedObject.GetComponent<Collider>();
        Rigidbody objRigidbody = spawnedObject.GetComponent<Rigidbody>();

        // 초기 위치를 바닥으로 설정 (Y축으로 -2만큼 아래로)
        Vector3 startPosition = transformMasterObj.position;
        startPosition.y -= 2f;
        spawnedObject.transform.position = startPosition;

        if (objCollider != null) objCollider.enabled = false;
        if (objRigidbody != null) objRigidbody.isKinematic = true;

        // DOTween으로 부드럽게 올라오는 애니메이션
        spawnedObject.transform.DOMoveY(transformMasterObj.position.y, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                // 애니메이션 완료 후 Collider 및 Rigidbody 활성화
                if (objCollider != null) objCollider.enabled = true;
                if (objRigidbody != null) objRigidbody.isKinematic = false;
            });


        // 문 열기
        if (Door_1 != null) door1StartPos = Door_1.transform.position;
        if (Door_2 != null) door2StartPos = Door_2.transform.position;
        if (Door_1 != null) Door_1.transform.DOMove(transformTarget_1.position, duration).SetEase(Ease.OutQuad);
        if (Door_2 != null) Door_2.transform.DOMove(transformTarget_2.position, duration).SetEase(Ease.OutQuad);



        // 추가 기능들
        testFaces[0].SetActive(true);
        testFaces[1].SetActive(false);
        testFaces[2].SetActive(false);

        for (int i = 0; i < scanners.Length; i++)
        {
            scanners[i].ThrowOtherColorObj(ColorCorrects[i]);
        }
    }
    // #. 스캔 실패 ㅠㅠ
    private void Scan_Fail()
    {
        testFaces[0].SetActive(false);
        testFaces[1].SetActive(false);
        testFaces[2].SetActive(true);

        for (int i = 0; i < scanners.Length; i++) 
        {
            scanners[i].ThrowOtherColorObj(ColorCorrects[i]);
        }
    }
    // #. 스캐너 초기 상태로 돌리기
    private void Scan_Reset()
    {


        // #. 테스트용 함수
        testFaces[0].SetActive(false);
        testFaces[1].SetActive(true);
        testFaces[2].SetActive(false);

        
    }





}
