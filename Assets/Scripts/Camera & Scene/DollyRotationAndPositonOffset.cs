using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 이 어트리뷰트를 추가해야 Inspector에서 인식됩니다.
public class Offset
{
    public float fPositionOffest;
    public Vector3 lookAtOffset;
}

public class DollyRotationAndPositonOffset : MonoBehaviour
{
    public Offset[] Offsets;
}
