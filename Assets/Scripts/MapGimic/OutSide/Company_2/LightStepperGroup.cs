using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStepperGroup : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[] originalMaterials;
    public Material[] materials;

    private int triggerCount = 0; // 현재 충돌 중인 콜라이더 개수
    public int iLightOn = 0; // 0: 꺼짐, 1: 켜짐
    private bool bCooldown = false; // 쿨타임 상태

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // 원래의 머티리얼 저장
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            renderers[i].material = materials[iLightOn]; // 초기 상태
        }
    }

    public void LightOffSteps()
    {
        if (bCooldown) return; // 쿨타임 중이면 실행 안 함
        StartCoroutine(Cooldown()); // 쿨타임 시작

        triggerCount--; // 트리거에서 벗어남
        if (triggerCount <= 0) // 모든 충돌이 끝나면 Off 실행
        {
            foreach (Renderer rend in renderers) rend.material = materials[0];

            iLightOn = 0;
            triggerCount = 0; // 안전하게 0으로 고정
        }
    }

    public void LightOnSteps()
    {
        if (bCooldown) return; // 쿨타임 중이면 실행 안 함
        StartCoroutine(Cooldown()); // 쿨타임 시작

        if (triggerCount == 0) // 처음 들어갈 때만 실행
        {
            foreach (Renderer rend in renderers) rend.material = materials[1];

            iLightOn = 1;
        }
        triggerCount++; // 트리거 개수 증가
    }

    private IEnumerator Cooldown()
    {
        bCooldown = true;
        yield return new WaitForSeconds(0.2f); // 0.2초 대기
        bCooldown = false;
    }
}
