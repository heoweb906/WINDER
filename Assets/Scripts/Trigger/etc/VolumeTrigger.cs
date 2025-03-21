using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
    private bool bTrigger = false;

    public Volume volume;

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bTrigger)
        {
            bTrigger = true;

            DOFadeVolume(0.4f, 2f);


        }
    }

    void DOFadeVolume(float targetWeight, float duration)
    {
        volume.DOComplete(); // 기존 트윈을 중지 (겹치는 애니메이션 방지)

        DOTween.To(() => volume.weight, x => volume.weight = x, targetWeight, duration);
    }




}
