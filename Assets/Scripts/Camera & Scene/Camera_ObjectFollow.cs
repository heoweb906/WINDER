using System.Collections;
using UnityEngine;

public class Camera_ObjectFollow : CameraObj
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private HandheldCamera handheld;

    private void Awake()
    {
        handheld = GetComponent<HandheldCamera>();
    }

    void FixedUpdate()
    {
        if (!GameAssistManager.Instance.GetBoolPlayerDie())
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;

            if (handheld == null || !handheld.enabled)
            {
                Quaternion targetRotation = Quaternion.Euler(rotationOffset);
                transform.rotation = targetRotation;
            }
        }
    }
}
