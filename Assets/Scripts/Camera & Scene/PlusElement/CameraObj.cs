using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraObj : MonoBehaviour
{
    [Header("카메라 전환시 적용될 보간")]
    public CinemachineBlendDefinition.Style blendStyle; // 블렌드 스타일
    public float duration; // 지속 시간
    [Space(50f)]

    public Vector3 rotationOffset; // 카메라의 회전 오프셋
    private GameObject emptyObj;




}
