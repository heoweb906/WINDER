using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.EventSystems;


public class SoundBlockMachine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    [Header("소리 조각 관련")]
    public SoundPiece[] soundPieces = new SoundPiece[4];
    public int[] iCorrectArray;
    private bool bScanFail;

    [Header("오브젝트 애니메이션 관련")]
    public GameObject GlassCase;
    public GameObject Guitar;
    public GameObject[] Arms;
    private Vector3[] armInitialRotations;



    public GameObject[] PianoKeyboards;
    public GameObject[] dials;
    public GameObject[] levers;


    private void Awake()
    {
        armInitialRotations = new Vector3[Arms.Length];
        for (int i = 0; i < Arms.Length; i++)
        {
            armInitialRotations[i] = Arms[i].transform.localEulerAngles;
        }
    }



 

    



    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery + 2);
        nowCoroutine = StartCoroutine(PlayPitchSoundsCoroutine());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        if (bScanFail)
            FailPlayAction();
        else
            SuccesPlayAction();
    }




    // #. 악기 작동
    private IEnumerator PlayPitchSoundsCoroutine()
    {
        int iTempTime = (int)fCurClockBattery + 2;
        bScanFail = false;

        RotateZOverTime(dials[0], (int)fCurClockBattery + 2, true);
        RotateZOverTime(dials[1], (int)fCurClockBattery + 2, false);

        MoveYLoop(levers[0], -0.656f, -0.382f, 0.67f, 0.77f, fCurClockBattery + 2);
        MoveYLoop(levers[1], -0.382f, -0.656f, 0.77f, 0.67f, fCurClockBattery + 2);

        while (iTempTime > 0)
        {
            for (int i = 0; i < soundPieces.Length; i++)
            {
                if (soundPieces[i] != null)
                {
                    if (soundPieces[i].iSoundPieceNum != iCorrectArray[i]) bScanFail = true;
                    {
                        PressKeyEffect(PianoKeyboards[soundPieces[i].iSoundPieceNum], 0.3f, 0.2f);
                    }

                    soundPieces[i].PlayingPitchSound();


                }
                else
                {
                    bScanFail = true;
                    SoundAssistManager.Instance.GetSFXAudioBlock("POP Brust 08", gameObject.transform);
                }

                yield return new WaitForSeconds(1.0f);

                iTempTime -= 1;
                if (iTempTime <= 0)
                {
                    iTempTime = 0;
                    fCurClockBattery = 0;
                    TurnOffObj();
                    yield break;
                }
            }


            TurnOffObj();
            yield break;
        }

     
    }









    // #. 연주에 성공했을 때 실행시킬 함수
    private void SuccesPlayAction()
    {
        Debug.Log("연주에 성공했습니다.");

        StartCoroutine(SuccesPlayAction_());
        
    }
    IEnumerator SuccesPlayAction_()
    {
        RotateToX(GlassCase, -200f, 3f);

        yield return new WaitForSeconds(3.2f);

        MoveToPosition(Guitar, new Vector3(22.45f, 2.166f, 17.052f), 2f);

        RotateToZ(Arms[0], 109.121f, 2f);
        RotateToZ(Arms[1], -108.662f, 2f);
        RotateToZ(Arms[2], 118.734f, 2f);
        RotateToZ(Arms[3], -29.655f, 2f);


        yield return new WaitForSeconds(2.8f);

        for (int i = 0; i < Arms.Length; i++)
        {
            RotateToZ(Arms[i], armInitialRotations[i].z, 0.1f); // ← 기존 함수 사용
        }

        GuitarObj_1 guitar = Guitar.GetComponent<GuitarObj_1>();
        guitar.ChangeToFlyingState();




    }

    // 22.45    2.166  17.052


    // #. 연주에 실패했을 때 실행시킬 함수
    private void FailPlayAction()
    {
        Debug.Log("연주에 실패했습니다.");
    }




    // #. 포지션 애니메이션 (기타 케이스 용)
    public void MoveToPosition(GameObject target, Vector3 targetPosition, float duration = 1.5f)
    {
        target.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.InOutSine);
    }
    public void RotateToX(GameObject target, float targetXRotation, float duration)
    {
        Transform tf = target.transform;
        Vector3 currentRotation = tf.localEulerAngles;
        Vector3 targetRotation = new Vector3(targetXRotation, currentRotation.y, currentRotation.z);

        tf.DOLocalRotate(targetRotation, duration).SetEase(Ease.InOutSine);
    }
    public void RotateToZ(GameObject target, float targetZRotation, float duration)
    {
        Transform tf = target.transform;
        Vector3 currentRotation = tf.localEulerAngles;
        Vector3 targetRotation = new Vector3(currentRotation.x, currentRotation.y, targetZRotation);

        tf.DOLocalRotate(targetRotation, duration).SetEase(Ease.InOutSine);
    }





    // #. 피아노 건반 애니메이션 
    public void PressKeyEffect(GameObject target, float pressDistance, float pressDuration)
    {
        PressKeyRoutine_(target, pressDistance, pressDuration);
    }
    public void PressKeyRoutine_(GameObject target, float pressDistance, float pressDuration)
    {
        Transform tf = target.transform;
        Vector3 originalPos = tf.position;
        Vector3 pressedPos = originalPos + Vector3.down * pressDistance;

        tf.DOMove(pressedPos, pressDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            tf.DOMove(originalPos, pressDuration).SetEase(Ease.InSine);
        });
    }

    // #. 다이얼 애니메이션
    public void RotateZOverTime(GameObject target, int duration, bool bRotateDir)
    {
        StartCoroutine(RotateZRoutine(target.transform, duration, bRotateDir));
    }
    private IEnumerator RotateZRoutine(Transform targetTransform, int duration, bool bRotateDir)
    {
        float timer = 0f;
        float totalTime = duration;
        float speed = 360f / totalTime; // 1초당 360도 회전

        while (timer < totalTime)
        {
            float deltaRotation = speed * Time.deltaTime;
            targetTransform.Rotate(0f, 0f, deltaRotation * (bRotateDir ? 1 : -1));
            timer += Time.deltaTime;
            yield return null;
        }
    }


    // #. 레버 애니메이션
    public void MoveYLoop(GameObject target, float minY, float maxY, float minZ, float maxZ, float totalTime)
    {
        Debug.Log("여기서 실행해야 함");

        StartCoroutine(MoveLeverManualRoutine(target.transform, minY, maxY, minZ, maxZ, totalTime));
    }
    private IEnumerator MoveLeverManualRoutine(Transform target, float minY, float maxY, float minZ, float maxZ, float totalTime)
    {
        float timer = 0f;
        float moveSpeed = 1.5f;
        float t = 0f;
        bool forward = true;

        Vector3 startPos = target.localPosition;
        Vector3 from = new Vector3(startPos.x, minY, minZ);
        Vector3 to = new Vector3(startPos.x, maxY, maxZ);

        while (timer < totalTime)
        {
            t += Time.deltaTime * moveSpeed;
            float easedT = Mathf.Sin(t * Mathf.PI * 0.5f); // InOutSine 느낌

            target.localPosition = Vector3.Lerp(forward ? from : to, forward ? to : from, easedT);

            if (easedT >= 1f)
            {
                t = 0f;
                forward = !forward;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }









    // #. IPartOwner 인터페이스
    #region

    public void InsertOwnerFunc(GameObject soundPieceObj, int index)
    {
        SoundPiece soundPiece = soundPieceObj.GetComponent<SoundPiece>();
        soundPieces[index] = soundPiece;
    }

    public void RemoveOwnerFunc(int index)
    {
        soundPieces[index] = null;
    }



    #endregion









}
