using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficClockWorkAssist : MonoBehaviour
{
    public void RotateObject(int time, float fDirRotate = 1f)
    {
        float rotationAmount = time * 180f * fDirRotate;

        transform.DORotate(new Vector3(0, 0, rotationAmount), time, RotateMode.LocalAxisAdd)
                 .SetEase(Ease.OutQuad);
    }




}
