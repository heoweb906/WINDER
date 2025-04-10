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

    public GameObject[] dials;
    public GameObject[] levers;


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
        MoveYLoop(levers[1], -0.656f, -0.382f, 0.67f, 0.77f, fCurClockBattery + 2);

        while (iTempTime > 0)
        {
            for (int i = 0; i < soundPieces.Length; i++)
            {
                if (soundPieces[i] != null)
                {
                    if (soundPieces[i].iSoundPieceNum != iCorrectArray[i]) bScanFail = true;
                    soundPieces[i].PlayingPitchSound();
                }
                else
                {
                    bScanFail = true;
                    SoundAssistManager.Instance.GetSFXAudioBlock("POP Brust 08", gameObject.transform);
                }

                yield return new WaitForSeconds(1.0f);

                // 배터리 감소
                iTempTime -= 1;
                if (iTempTime <= 0)
                {
                    iTempTime = 0;
                    fCurClockBattery = 0;
                    TurnOffObj();
                    yield break; // 코루틴 종료
                }
            }


            TurnOffObj();
            yield break; // 모든 작업이 완료되면 코루틴 종료
        }

     
    }









    // #. 연주에 성공했을 때 실행시킬 함수
    private void SuccesPlayAction()
    {
        Debug.Log("연주에 성공했습니다.");
    }

    // #. 연주에 실패했을 때 실행시킬 함수
    private void FailPlayAction()
    {
        Debug.Log("연주에 실패했습니다.");
    }










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
