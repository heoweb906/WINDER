using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundBlockMachine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    [Header("소리 조각 관련")]
    public SoundPiece[] soundPieces = new SoundPiece[4];
    public int[] iCorrectArray;
    private bool bScanFail;


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
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
        bScanFail = false;

        while (fCurClockBattery > 0)
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

                yield return new WaitForSecondsRealtime(1.0f);

                // 배터리 감소
                fCurClockBattery -= 1;
                if (fCurClockBattery <= 0)
                {
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

    }

    // #. 연주에 실패했을 때 실행시킬 함수
    private void FailPlayAction()
    {

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
