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
        volume.DOComplete(); // ���� Ʈ���� ���� (��ġ�� �ִϸ��̼� ����)

        DOTween.To(() => volume.weight, x => volume.weight = x, targetWeight, duration);
    }




}
