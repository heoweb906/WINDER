using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePositionChanger : MonoBehaviour
{
    private GameObject player;
    public string sChangeSceneName = "NULL";

    [Header("Position 이동일 경우에 필요한 정보들")]

    public CineCameraChager cienCamareChager;
    public Transform targetPosition;

    private void Start()
    {
        player = GameObject.Find("MainCharacter");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(sChangeSceneName != "NULL") InGameUIController.Instance.ChangeScene(sChangeSceneName);
            else
            {
                if(cienCamareChager != null && targetPosition != null) JustChangePosition();
            }
        }
    }

    // #. Scene은 변경하지 않고 위치만 변경
    private void JustChangePosition() 
    {
        StartCoroutine(ChangePosition_());
    }
    private IEnumerator ChangePosition_()
    {
        InGameUIController.Instance.bIsUIDoing = true;
        InGameUIController.Instance.FadeInOutImage(1f, 2f);

        yield return new WaitForSecondsRealtime(3f);

        cienCamareChager.CameraChange();
        player.transform.position = targetPosition.position;
        GameAssistManager.Instance.RespawnChangeAssist(targetPosition);

        yield return new WaitForSecondsRealtime(0.5f);

        InGameUIController.Instance.FadeInOutImage(0f, 3f);
        InGameUIController.Instance.bIsUIDoing = false;
    }

}
