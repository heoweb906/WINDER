using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearobjEffect : MonoBehaviour
{
    public float DissolveSpeed = 0.01f;  // Dissolve 속도
    public float FadeSpeed = 0.05f;      // 알파 값 감소 속도
    public float DissolveYield = 0.1f;   // 코루틴 대기 시간

    public ParticleSystem Particle = null; // 파티클 시스템

    private MeshRenderer meshRenderer;
    private Material material;

    private float dissolveStart = -0.2f;
    private float dissolveEnd = 1.2f;

    private void Awake()
    {
        // 현재 오브젝트에서 MeshRenderer 컴포넌트 가져오기
        meshRenderer = GetComponent<MeshRenderer>();

        // MeshRenderer에서 Material 가져오기
        if (meshRenderer != null)
        {
            material = meshRenderer.material;
        }
    }

    private void Update()
    {
        // 테스트용: T 키를 누르면 Dissolve 효과 실행
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(DissolveCoroutine());
        }
    }

    private IEnumerator DissolveCoroutine()
    {
        if (Particle != null)
        {
            Particle.Play();
        }

        float dissolveAmount = dissolveStart;
        float speedMultiplier = 1f;
        Color color = material.color; // 현재 머테리얼 색상 가져오기

        while (dissolveAmount < dissolveEnd)
        {
            dissolveAmount += DissolveSpeed * speedMultiplier;
            speedMultiplier += 0.1f;

            if (material != null)
            {
                // 머테리얼의 알파 값을 감소
                color.a -= FadeSpeed;
                color.a = Mathf.Clamp01(color.a); // 알파 값이 0과 1 사이로 유지되도록 제한
                material.color = color;
            }

            yield return new WaitForSeconds(DissolveYield);
        }

        Destroy(gameObject);
    }
}
