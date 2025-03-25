using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorObj : MonoBehaviour
{ 
    public ColorType colorType;

    public Material _material; // 복사된 머테리얼을 저장할 변수

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer 컴포넌트가 없습니다!");
            return;
        }

        // 머테리얼 복사
        _material = new Material(renderer.material);

        // 복사한 머테리얼을 오브젝트에 적용
        renderer.material = _material;
    }
}
