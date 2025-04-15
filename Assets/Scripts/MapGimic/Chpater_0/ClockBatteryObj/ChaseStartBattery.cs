using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChaseStartBattery : ClockBattery
{
    private Coroutine nowCoroutine;

    public CineCameraChager changer_1;
    public CineCameraChager changer_2;


    [Header("라이트 관련")]
    public Volume volume;
    public GameObject[] objs_Light;
    public Streetlamp_InTutorial[] StreetLights;


    [Header("카메라 쉐이크")]
    public HandheldCamera handHeld;
    public CameraEvent cameraEvent;

    [SerializeField]
    private GGILICK_ChaseManager chaseManager;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery + 2);
        nowCoroutine = StartCoroutine(ChaseStart_1());
    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        GameAssistManager.Instance.PlayerInputLockOff();

        nowCoroutine = StartCoroutine(ChaseStart_2());


        bDoing = false;
        if (clockWork != null) clockWork.GetComponent<ClockWork>().canInteract = false;
        bBatteryFull = false;
        fCurClockBattery = 0f;
    }


    IEnumerator ChaseStart_1()
    {
        changer_1.CameraChange();



        while (fCurClockBattery > 0)
        {

            yield return new WaitForSeconds(1.0f);

            fCurClockBattery -= 1;
        }


        DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 4f);
        int count = Mathf.Min(objs_Light.Length, StreetLights.Length);
        for (int i = 0; i < count; i++)
        {
            if (objs_Light[i] != null)
                objs_Light[i].SetActive(true);

            if (StreetLights[i] != null)
                StreetLights[i].SetLightActive(true);

            yield return new WaitForSeconds(0.3f);
        }

        changer_2.CameraChange();

        yield return new WaitForSeconds(2.0f);

        TurnOffObj(); 
    }



    IEnumerator ChaseStart_2()
    {
        yield return new WaitForSeconds(2.0f);

        cameraEvent.CameraTriggerStart(4);

        yield return new WaitForSeconds(2.1f);


        chaseManager.ChaseStart();
    }



}
