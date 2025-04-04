using UnityEngine;

public class HandheldCamera_DollyCart : MonoBehaviour
{
    public float rotationAmount = 1f;  // 기본 흔들림 크기
    public float rotationSpeed = 1f;   // 기본 흔들림 속도

    private Camera_DollyCart dollyCart;
    private Quaternion baseRotation;

    private float strongShakeAmount = 0f;
    private float strongShakeDuration = 0f;
    private float strongShakeTimer = 0f;
    public float shakeFrequency = 25f; // 지진처럼 빠르게 떨리는 속도

    public float amount = 5f;  // 강한 흔들림 기본 크기
    public float fTime = 1f;   // 강한 흔들림 지속 시간

    private void Start()
    {
        dollyCart = GetComponent<Camera_DollyCart>();
        if (dollyCart == null)
        {
            Debug.LogError("HandheldCamera: Camera_DollyCart 컴포넌트를 찾을 수 없습니다.");
            return;
        }
    }


    void FixedUpdate()
    {
        if (dollyCart == null) return;

        baseRotation = dollyCart.transform.rotation;

        float currentShakeAmount = rotationAmount;
        float currentShakeSpeed = rotationSpeed;

        if (strongShakeTimer > 0)
        {
            strongShakeTimer -= Time.deltaTime;

            // 흔들림 강도를 지수 함수 형태로 줄이기 (Easing 적용)
            float intensity = Mathf.Pow(strongShakeTimer / strongShakeDuration, 2);

            currentShakeAmount += strongShakeAmount * intensity;
            currentShakeSpeed += shakeFrequency * intensity;
        }

        // Perlin Noise로 흔들림 적용
        float xShake = (Mathf.PerlinNoise(Time.time * currentShakeSpeed, 0) - 0.5f) * 2 * currentShakeAmount;
        float yShake = (Mathf.PerlinNoise(0, Time.time * currentShakeSpeed) - 0.5f) * 2 * currentShakeAmount;

        Quaternion shakeRotation = Quaternion.Euler(xShake, yShake, 0);
        transform.rotation = baseRotation * shakeRotation;
    }

    // 강한 흔들림 트리거 함수 (지진 효과)
    public void TriggerStrongShake(float amount, float duration)
    {
        Debug.Log("카메라 강하게 흔들기");

        strongShakeAmount = amount;
        strongShakeDuration = duration;
        strongShakeTimer = duration;
    }
}
