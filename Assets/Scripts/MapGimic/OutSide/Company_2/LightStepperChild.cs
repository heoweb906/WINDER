using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStepperChild : MonoBehaviour
{
    private LightStepperGroup parentStepper;

    private void Start()
    {
        parentStepper = GetComponentInParent<LightStepperGroup>(); // 부모 찾기
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerStep"))
        {
            if (parentStepper.iLightOn == 0)
                parentStepper.LightOnSteps(); // 불을 켬
            else
                parentStepper.LightOffSteps(); // 불을 끔
        }
    }
}
