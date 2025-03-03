using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStepperGroup : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[] originalMaterials;
    public Material[] materials;

    private int triggerCount = 0; // ���� �浹 ���� �ݶ��̴� ����
    public int iLightOn = 0; // 0: ����, 1: ����
    private bool bCooldown = false; // ��Ÿ�� ����

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // ������ ��Ƽ���� ����
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            renderers[i].material = materials[iLightOn]; // �ʱ� ����
        }
    }

    public void LightOffSteps()
    {
        if (bCooldown) return; // ��Ÿ�� ���̸� ���� �� ��
        StartCoroutine(Cooldown()); // ��Ÿ�� ����

        triggerCount--; // Ʈ���ſ��� ���
        if (triggerCount <= 0) // ��� �浹�� ������ Off ����
        {
            foreach (Renderer rend in renderers) rend.material = materials[0];

            iLightOn = 0;
            triggerCount = 0; // �����ϰ� 0���� ����
        }
    }

    public void LightOnSteps()
    {
        if (bCooldown) return; // ��Ÿ�� ���̸� ���� �� ��
        StartCoroutine(Cooldown()); // ��Ÿ�� ����

        if (triggerCount == 0) // ó�� �� ���� ����
        {
            foreach (Renderer rend in renderers) rend.material = materials[1];

            iLightOn = 1;
        }
        triggerCount++; // Ʈ���� ���� ����
    }

    private IEnumerator Cooldown()
    {
        bCooldown = true;
        yield return new WaitForSeconds(0.2f); // 0.2�� ���
        bCooldown = false;
    }
}
