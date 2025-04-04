using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightStepper : MonoBehaviour
{
    public bool bStep;
    private Renderer renderer;
    private Material matMine;
   
    public Material[] materials;
    
    

    private bool isCooldown; // 쿨다운 상태를 체크하는 변수

    private void Awake()
    {
        // Renderer에서 현재 Material을 가져옴 (개별 Material 인스턴스 생성)
        renderer = GetComponent<Renderer>();
        matMine = renderer.material; // 초기 Material 저장

        if (bStep) renderer.material = materials[0]; // 첫 번째 Material로 변경
        else renderer.material = materials[1]; // 두 번째 Material로 변경
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCooldown)
        {
           
            isCooldown = true;
            bStep = !bStep;

            renderer = GetComponent<Renderer>();
            if (bStep) renderer.material = materials[0]; // 첫 번째 Material로 변경
            else renderer.material = materials[1]; // 두 번째 Material로 변경

            // 현재 Material 업데이트
            matMine = renderer.material;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCooldown = false;
    }


}
