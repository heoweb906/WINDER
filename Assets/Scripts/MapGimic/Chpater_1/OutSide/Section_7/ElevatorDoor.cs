using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public GameObject leftDoor; 
    public GameObject rightDoor;

    public Transform position_target_LeftDoor;
    public Transform position_target_RightDoor;
    public Transform position_middle_LeftDoor;
    public Transform position_middle_RightDoor;


    public Vector3 originalPosition_LeftDoor;     // 왼쪽 문의 원래 위치
    public Vector3 originalPosition_RightDoor;    // 오른쪽 문의 원래 위치

    void Start()
    {
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;

    }

  
}
