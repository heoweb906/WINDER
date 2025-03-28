using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    public GameObject monitor; 
    public Material mat;
    public Material[] mats_Face;
    // public Texture2D[] textures_Face;
    public float fShake;
    
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        mat = new Material(mat); // ���� �ν��Ͻ� ����
        ApplyMaterial(); // ����Ϳ� ���׸��� ����
        shakeCoroutine = StartCoroutine(ShakeEffect());
    }


    public void SetTextureProperty(int textureIndex)
    {
        // 0 - ȭ��
        // 1 - ��� ���ڰ� ����
        // 2 - �Ǹ��� ǥ��
        // 3 - ��ǥ��
        // 4 - ��� ����
        mat = new Material(mats_Face[textureIndex]); // ���� �ν��Ͻ��� ����
        ApplyMaterial(); // ���� ���� ����
    }

    private void ApplyMaterial()
    {
        if (monitor != null)
        {
            Renderer renderer = monitor.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = mat; // monitor�� ���׸��� ����
            }
        }
    }



    void SetFloatProperty(float value)
    {
        mat.SetFloat("_fShake", value);
    }
    IEnumerator ShakeEffect()
    {
        while (true)
        {
            // 0.1 �Ǵ� 0.15 �� �ϳ��� ���� ����
            float shakeValue = Random.value > 0.5f ? 0.1f : 0.15f;
            SetFloatProperty(shakeValue);

            yield return new WaitForSeconds(0.1f); // 0.1�� ����

            SetFloatProperty(0.0f);
            float waitTime = Random.Range(0.5f, 1.8f); // 1~2�� ���� ���
            yield return new WaitForSeconds(waitTime);
        }
    }


}
