using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
  
    [Header("Door, Monitor")]
    public Monitor monitor;
    public GameObject objDoor_1;
    public float duration = 1.0f; // 문이 열리는 시간


    [Header("Boss Hand")]
    public CameraEvent cameraEvent;
    public GameObject masterObject;         // 스캔에 성공하면 획득할 수 있는 오브젝트
    public Transform transformMasterObj;    // 마스터 오브젝트 생성 위치
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

       
        ScanStart_();
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
    private void ScanStart_()
    {
        if (BoolCheckObjOnScanner())  // 스캔 성공
        {
            RotateObject((int)fCurClockBattery + 15);
            StartCoroutine(Scan_Success());
        }
        else
        {
            RotateObject((int)fCurClockBattery + 5);
            StartCoroutine(Scan_Fail());
        }

        //yield return new WaitForSeconds(1f);

        //Scan_Reset();
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
    IEnumerator Scan_Success()
    {
        InGameUIController.Instance.bIsUIDoing = true;


        cameraEvent.CameraTriggerStart(25);
        yield return new WaitForSeconds(1f);
        List<ColorObj> colorObjs = new List<ColorObj>(); // 리스트 생성
        GameAssistManager.Instance.PlayerInputLockOn();

        yield return new WaitForSeconds(2f);


        // for (int i = 0; i < scanners.Length; i++) scanners[i].ThrowOtherColorObj(ColorCorrects[i]);
        // image_obj 컬러 변경
        for (int i = 0; i < scanners.Length; i++) scanners[i].ChangeImageColor(2f, new Color(1f, 1f, 1f));
        for (int i = 0; i < ColorCorrects.Length; i++)
        {
            if (scanners[i] != null)
            {
                foreach (var colorObj in scanners[i].GetColorObjList())
                {
                    colorObjs.Add(colorObj);
                }
            }
        }

        yield return new WaitForSeconds(3f);

        Hand_DokillTween();
        Hand_Move(transform_LeftHand.position + new Vector3(0, 15f, 0),
        transform_RightHand.position + new Vector3(0, 15f, 0), 2f);

        yield return new WaitForSeconds(3f);


     
        colorObjs.Add(scanners[0].GetColorObjList()[0]);
        colorObjs.Add(scanners[1].GetColorObjList()[0]);
        colorObjs.Add(scanners[2].GetColorObjList()[0]);
 


        for (int i = 0; i < colorObjs.Count; i++)
        {
            Renderer renderer = colorObjs[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                Material mat = renderer.material;
                // Emission 켜기
                mat.SetFloat("_UseEmission", 1f);
                mat.EnableKeyword("_EMISSION");

                // 3초 동안 _EmissionColor를 (1.7f, 1.7f, 1.7f, 1f)로 Tween
                DOTween.To(() => mat.GetColor("_EmissionColor"),
                           x => mat.SetColor("_EmissionColor", x),
                           new Color(1.7f, 1.7f, 1.7f, 1f),
                           2f)
                       .SetEase(Ease.Linear);
            }
        }

        yield return new WaitForSeconds(2.3f);

        for (int i = 0; i < colorObjs.Count; i++)
        {
            Renderer renderer = colorObjs[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                Material mat = renderer.material;

                // Transparent 렌더링 모드로 전환
                mat.SetFloat("_RenderingMode", 3f);
                mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetFloat("_ZWrite", 0f);
                mat.renderQueue = 3000; // Transparent 큐로 변경

                // Base Color 알파를 0으로 Tween
                Color startColor = mat.GetColor("_BaseColor");
                Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

                DOTween.To(() => mat.GetColor("_BaseColor"),
                           x => mat.SetColor("_BaseColor", x),
                           targetColor,
                           1.2f) // 4초 동안 변화
                       .SetEase(Ease.Linear);
            }
        }
        for (int i = 0; i < scanners.Length; i++) scanners[i].ChangeImageColor(3f, new Color(0f, 0f, 0f));



        yield return new WaitForSeconds(3f);
        for (int i = 0; i < colorObjs.Count; i++)
        {
            Destroy(colorObjs[i].gameObject);
        }



        GameObject spawnedObject = Instantiate(masterObject);
        spawnedObject.transform.localPosition = transformMasterObj.position;


        yield return new WaitForSeconds(2f);




        // 문 열기
        if (objDoor_1 != null)
            objDoor_1.transform.DORotate(new Vector3(0, 140, 0), duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutQuad);


        InGameUIController.Instance.bIsUIDoing = false;
        GameAssistManager.Instance.PlayerInputLockOff();
    }

    // #. 스캔 실패 ㅠㅠ
    IEnumerator Scan_Fail()
    {
        Debug.Log("스캔 실패 ");

        InGameUIController.Instance.bIsUIDoing = true;
        List<ColorObj> colorObjs = new List<ColorObj>(); // 리스트 생성

        cameraEvent.CameraTriggerStart(10);
        yield return new WaitForSeconds(1f);
        GameAssistManager.Instance.PlayerInputLockOn();

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < ColorCorrects.Length; i++)
        {
            if (scanners[i] != null)
            {
                foreach (var colorObj in scanners[i].GetColorObjList())
                {
                    if (colorObj != null && colorObj.colorType == ColorCorrects[i])
                    {
                        scanners[i].ChangeImageColor(2f, new Color(1f, 1f, 1f));
                    }
                    if(colorObj.colorType == ColorType.None)
                    {
                        colorObjs.Add(colorObj);
                        Debug.Log("확인됨");
                    }
                }
            }
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < colorObjs.Count; i++)
        {
            Renderer renderer = colorObjs[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                Material mat = renderer.material;
                // Emission 켜기
                mat.SetFloat("_UseEmission", 1f);
                mat.EnableKeyword("_EMISSION");

                // 3초 동안 _EmissionColor를 (1.7f, 1.7f, 1.7f, 1f)로 Tween
                DOTween.To(() => mat.GetColor("_EmissionColor"),
                           x => mat.SetColor("_EmissionColor", x),
                           new Color(1.7f, 1.7f, 1.7f, 1f),
                           2f)
                       .SetEase(Ease.Linear);
            }
        }

        yield return new WaitForSeconds(2.3f);

        for (int i = 0; i < colorObjs.Count; i++)
        {
            Renderer renderer = colorObjs[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                Material mat = renderer.material;

                // Transparent 렌더링 모드로 전환
                mat.SetFloat("_RenderingMode", 3f);
                mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetFloat("_ZWrite", 0f);
                mat.renderQueue = 3000; // Transparent 큐로 변경

                // Base Color 알파를 0으로 Tween
                Color startColor = mat.GetColor("_BaseColor");
                Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

                DOTween.To(() => mat.GetColor("_BaseColor"),
                           x => mat.SetColor("_BaseColor", x),
                           targetColor,
                           1.2f) // 4초 동안 변화
                       .SetEase(Ease.Linear);
            }
        }
        for (int i = 0; i < scanners.Length; i++) scanners[i].ChangeImageColor(3f, new Color(0f, 0f, 0f));

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < colorObjs.Count; i++)
        {
            Destroy(colorObjs[i].gameObject);
        }


        InGameUIController.Instance.bIsUIDoing = false;
        GameAssistManager.Instance.PlayerInputLockOff();
        TurnOffObj();



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

    // #. 위치 옮기기
    private void Hand_Move(Vector3? leftPosition = null, Vector3? rightPosition = null, float fDuration = 0f)
    {
        if (leftPosition.HasValue)
        {
            objLeftHand.transform.DOMove(leftPosition.Value, fDuration)
                .SetEase(Ease.OutQuad);
        }

        if (rightPosition.HasValue)
        {
            objRightHand.transform.DOMove(rightPosition.Value, fDuration)
                .SetEase(Ease.OutQuad);
        }
    }

    // #. 로테이션 돌리기
    private void Hand_Rotate(Quaternion? leftRotation = null, Quaternion? rightRotation = null, float fDuration = 0f)
    {
        if (leftRotation.HasValue) 
        {
            objLeftHand.transform.DORotateQuaternion(leftRotation.Value, fDuration)
                .SetEase(Ease.OutQuad);
        }

        if (rightRotation.HasValue)
        {
            objRightHand.transform.DORotateQuaternion(rightRotation.Value, fDuration)
                .SetEase(Ease.OutQuad);
        }
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
