using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCarSlopeRotate : MonoBehaviour
{
    public Vector3 targetEulerAngles; // 적용할 목표 회전 값
    public float rotateDuration; // 회전 시간

    private void OnTriggerEnter(Collider other)
    {
        RoadCar car = other.GetComponent<RoadCar>();
        if (car != null)
        {
            Debug.Log("기울이기 시작");
            if (car.iCarType == 0) rotateDuration = 0.3f;
            else rotateDuration = 0.6f;
            StartCoroutine(RotateCarSmoothly(car.GetComponent<Rigidbody>(), Quaternion.Euler(targetEulerAngles), rotateDuration));
        }
    }

    private IEnumerator RotateCarSmoothly(Rigidbody rb, Quaternion endRot, float duration)
    {
        Quaternion startRot = rb.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Quaternion newRot = Quaternion.Slerp(startRot, endRot, t);
            rb.MoveRotation(newRot); // Rigidbody 기반 회전

            yield return new WaitForFixedUpdate();
        }

        rb.MoveRotation(endRot);
    }
}