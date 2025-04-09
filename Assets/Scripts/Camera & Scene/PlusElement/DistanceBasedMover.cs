using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedMover : MonoBehaviour
{
    public GameObject aaa;
    public GameObject bbb;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float maxDistance;
    public float minDistance;

    public HandheldCamera_DollyCart dollyCart_fAmount;

    private void FixedUpdate()
    {
        if (aaa == null || bbb == null) return;

        float distance = Vector3.Distance(aaa.transform.position, bbb.transform.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);

        float targetX = Mathf.Lerp(minX, maxX, t);
        float targetY = Mathf.Lerp(minY, maxY, t);

        transform.localPosition = new Vector3(targetX, targetY, transform.localPosition.z);

        if (dollyCart_fAmount != null)
        {
            dollyCart_fAmount.rotationAmount = Mathf.Lerp(0.1f, 0.8f, t);
            dollyCart_fAmount.rotationSpeed = Mathf.Lerp(0.4f, 2.4f, t);
        }
    }
}
