using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAssist : MonoBehaviour
{
    public CinemachineSmoothPath path;
    public CinemachineDollyCart[] dollyCarts;
    public float fSpeed;
    public float fInterval;

    private void Awake()
    {
        StartCoroutine(ActivateTrainsWithInterval());
    }

    private IEnumerator ActivateTrainsWithInterval()
    {
        foreach (var train in dollyCarts)
        {
            train.m_Path = path;
            train.m_Speed = fSpeed;

            yield return new WaitForSeconds(fInterval); // fInterval 간격으로 대기
        }
    }



}
