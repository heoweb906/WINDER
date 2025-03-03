using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveCustom : MonoBehaviour
{
    public float DissolveSpeed = 0.01f;
    public float DissolveYield = 0.1f;

    public ParticleSystem Particle = null;

    private const string DISSOLVE_AMOUNT = "_DissolveAmount";

    private MeshRenderer[] m_meshRenderers = null;
    public List<Material> m_materials = new List<Material>();

    private float m_dissolveStart = -0.2f;
    private float m_dissolveEnd = 1.2f;

    private void Awake()
    {
        // 자식 포함 모든 MeshRenderer 가져오기
        m_meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in m_meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                m_materials.Add(material);
            }
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
        foreach (Material material in m_materials)
        {
            material.SetFloat(DISSOLVE_AMOUNT, m_dissolveStart);
        }
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveCoroutine());
    }

    private IEnumerator DissolveCoroutine()
    {
        if (Particle != null)
        {
            Particle.Play();
        }

        if (m_materials.Count > 0)
        {
            float dissolveAmount = m_dissolveStart;
            float speedMulti = 1f;
            while (dissolveAmount < m_dissolveEnd)
            {
                dissolveAmount += DissolveSpeed * speedMulti;
                speedMulti += 0.1f;
                foreach (Material material in m_materials)
                {
                    material.SetFloat(DISSOLVE_AMOUNT, dissolveAmount);
                }
                yield return new WaitForSeconds(DissolveYield);
            }
        }
    }
}
