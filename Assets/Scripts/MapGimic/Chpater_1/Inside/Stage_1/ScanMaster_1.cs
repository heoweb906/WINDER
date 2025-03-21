using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public enum ColorType
{
    None,
    Yellow,
    Red
}

public class ScanMaster_1 : ClockBattery
{
    private Coroutine nowCoroutine;

    [Header("스캔 기능 관련")]
    public ColorType[] ColorCorrects;       // ScanMaster의 정답 컬러
    public Scanner[] scanners;              // 각 스테이지의 Scanner 

    [Header("MasterObj 관련")]
    public GameObject masterObject;         // 스캔에 성공하면 획득할 수 있는 오브젝트
    public Transform transformMasterObj;    // 마스터 오브젝트 생성 위치

    [Header("Door, Monitor")]
    public Monitor monitor;
    public GameObject objDoor_1;
    public float duration = 1.0f; // 문이 열리는 시간


    [Header("Boss Hand")]
    public GameObject objLeftHand; 
    public GameObject objRightHand;
    private Transform transform_LeftHand;
    private Transform transform_RightHand;



    private void Awake()
    {
        transform_LeftHand = objLeftHand.transform;
        transform_RightHand = objRightHand.transform;


        Hand_Floating();
    }
 


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
        // 스캐너 별로 오브젝트 1개식을 제외하고 모두 날려버리기 
        for (int i = 0; i < scanners.Length; i++) scanners[i].ThrowOtherColorObj(ColorCorrects[i]);

        // image_obj 컬러 변경
        for (int i = 0; i < scanners.Length; i++) scanners[i].ChangeImageColor(1.2f, new Color(1f, 1f, 1f));




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
        if (objDoor_1 != null)
            objDoor_1.transform.DORotate(new Vector3(0, 140, 0), duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutQuad);




    }
    // #. 스캔 실패 ㅠㅠ
    private void Scan_Fail()
    {
        for (int i = 0; i < scanners.Length; i++) 
        {
            scanners[i].ThrowOtherColorObj(ColorCorrects[i]);
        }


    }
    // #. 스캐너 초기 상태로 돌리기
    private void Scan_Reset()
    {


   

        
    }


    private void FaceChange(int index)
    {
        // index별 표정
        // 0 - 화남
        // 1 - 웃음 (기분 나쁨)
        // 2 - 실망
        // 3 - 무표정
        // 4 - 웃음 (평범)

        monitor.SetTextureProperty(index);
    }



    #region // 보스 손 컨트롤

    // #. 본래 위치로 돌리기
    private void Hand_Move(Transform transformLeft = null, Transform transformRight = null, float fDuration = 0f)
    {
        objLeftHand.transform.DOMove(transform_LeftHand.position, fDuration)
            .SetEase(Ease.OutQuad);

        // RightHand를 지정된 위치로 부드럽게 이동
        objRightHand.transform.DOMove(transform_RightHand.position, fDuration)
            .SetEase(Ease.OutQuad);
    }

    // #. 둥둥 뜨게 하기
    private void Hand_Floating()
    {
        objLeftHand.transform.DOMoveY(objLeftHand.transform.position.y + 0.5f, 1.5f)
           .SetLoops(-1, LoopType.Yoyo)
           .SetEase(Ease.InOutSine);

        // RightHand를 위아래로 떠 있는 효과 적용
        objRightHand.transform.DOMoveY(objRightHand.transform.position.y + 0.5f, 1.7f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    // #. Dotween 애니메이션 종료
    private void Hand_DokillTween()
    {
        objLeftHand.transform.DOKill();
        objRightHand.transform.DOKill();
    }
    #endregion


}
