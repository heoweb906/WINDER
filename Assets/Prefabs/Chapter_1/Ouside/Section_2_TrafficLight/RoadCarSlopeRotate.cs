using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCarSlopeRotate : MonoBehaviour
{
    public Vector3 targetEulerAngles; // 적용할 목표 회전 값
    private float rotateDuration;
    public float rotateDuration_1; // 회전 시간
    public float rotateDuration_2; // 회전 시간
    public float rotateDuration_3;

    private class RotationData
    {
        public Rigidbody rb;
        public Quaternion startRot;
        public Quaternion endRot;
        public float duration;
        public float elapsed;
    }

    private List<RotationData> rotatingCars = new List<RotationData>();

    private void FixedUpdate()
    {
        for (int i = rotatingCars.Count - 1; i >= 0; i--)
        {
            RotationData data = rotatingCars[i];
            data.elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(data.elapsed / data.duration);
            Quaternion newRot = Quaternion.Slerp(data.startRot, data.endRot, t);
            data.rb.MoveRotation(newRot);

            if (t >= 1f)
            {
                rotatingCars.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RoadCar car = other.GetComponent<RoadCar>();
        if (car != null)
        {
            float rotateDuration = car.iCarType switch
            {
                0 => rotateDuration_1,
                1 => rotateDuration_2,
                _ => rotateDuration_3
            };

            Rigidbody rb = car.GetComponent<Rigidbody>();
            rotatingCars.Add(new RotationData
            {
                rb = rb,
                startRot = rb.rotation,
                endRot = Quaternion.Euler(targetEulerAngles),
                duration = rotateDuration,
                elapsed = 0f
            });
        }
    }
}