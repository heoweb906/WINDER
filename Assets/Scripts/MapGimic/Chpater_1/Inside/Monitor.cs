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
        mat = new Material(mat); // 개별 인스턴스 생성
        ApplyMaterial(); // 모니터에 머테리얼 적용
        shakeCoroutine = StartCoroutine(ShakeEffect());
    }


    public void SetTextureProperty(int textureIndex)
    {
        // 0 - 화남
        // 1 - 기분 나쁘게 웃음
        // 2 - 실망한 표정
        // 3 - 무표정
        // 4 - 밝게 웃음
        mat = new Material(mats_Face[textureIndex]); // 개별 인스턴스로 변경
        ApplyMaterial(); // 변경 사항 적용
    }

    private void ApplyMaterial()
    {
        if (monitor != null)
        {
            Renderer renderer = monitor.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = mat; // monitor의 머테리얼 변경
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
            // 0.1 또는 0.15 중 하나를 랜덤 선택
            float shakeValue = Random.value > 0.5f ? 0.1f : 0.15f;
            SetFloatProperty(shakeValue);

            yield return new WaitForSeconds(0.1f); // 0.1초 유지

            SetFloatProperty(0.0f);
            float waitTime = Random.Range(0.5f, 1.8f); // 1~2초 랜덤 대기
            yield return new WaitForSeconds(waitTime);
        }
    }


}
