using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RoadCarOnTrack : CinemachineDollyCart
{
    private void Update()
    {
        // m_Position이 Path 길이와 같거나 크면 삭제
        if (m_Path != null && m_Position >= m_Path.PathLength)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameAssistManager.Instance.DiePlayerReset(3f, 0, 0.0f);
        }
    }

}
