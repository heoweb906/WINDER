using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    public Material mat;
    public Texture2D[] textures_Face;
    public float fShake;


    private Coroutine shakeCoroutine;


    private void Start()
    {
        shakeCoroutine = StartCoroutine(ShakeEffect());
    }

    void Update()
    {
        // ���� ���, Ű�� ������ �� float ���� ����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetFloatProperty(0.15f);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SetFloatProperty(0.0f);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetTextureProperty(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetTextureProperty(1);
        }



    }

    void SetFloatProperty(float value)
    {
        mat.SetFloat("_fShake", value);
    }

    void SetTextureProperty(int textureIndex)
    {
        Debug.Log("�۵�!!!");

        if (textureIndex >= 0 && textureIndex < textures_Face.Length)
        {
            mat.SetTexture("_textureFace", textures_Face[textureIndex]);
        }
        else
        {
            Debug.LogWarning("�ؽ�ó �ε����� ������ ������ϴ�.");
        }
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
