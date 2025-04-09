using System.Collections;
using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
    public float rotationAmount = 1f;
    public float rotationSpeed = 1f;

    private Quaternion baseRotation;
    private CameraObj cameraObj;
    private bool isShaking = false;

    private void Awake()
    {
        cameraObj = GetComponent<CameraObj>();
    }

    private void OnEnable()
    {
        baseRotation = Quaternion.Euler(cameraObj.rotationOffset);
    }

    private void FixedUpdate()
    {
        if (!isShaking)
        {
            float x = (Mathf.PerlinNoise(Time.time * rotationSpeed, 0f) - 0.5f) * 2 * rotationAmount;
            float y = (Mathf.PerlinNoise(0f, Time.time * rotationSpeed) - 0.5f) * 2 * rotationAmount;

            Quaternion shake = Quaternion.Euler(x, y, 0f);
            transform.rotation = baseRotation * shake;
        }
    }


    public void PulseShake(float tempAmount, float tempSpeed, float duration, bool bSmoothRetun = false)
    {
        StopAllCoroutines();
        StartCoroutine(PulseShakeRoutine(tempAmount, tempSpeed, duration, bSmoothRetun));
    }

    private IEnumerator PulseShakeRoutine(float tempAmount, float tempSpeed, float duration, bool bSmoothRetun)
    {
        Debug.Log("카메라 강하게 흔들기 작동!!!");

        float originalAmount = rotationAmount;
        float originalSpeed = rotationSpeed;

        rotationAmount = tempAmount;
        rotationSpeed = tempSpeed;

        yield return new WaitForSeconds(duration);

        rotationAmount = originalAmount;
        rotationSpeed = originalSpeed;
    }

}