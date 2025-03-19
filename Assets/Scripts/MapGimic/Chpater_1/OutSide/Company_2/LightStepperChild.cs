using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStepperChild : MonoBehaviour
{
    private LightStepperGroup parentStepper;

    private void Start()
    {
        parentStepper = GetComponentInParent<LightStepperGroup>(); // �θ� ã��
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerStep"))
        {
            if (parentStepper.iLightOn == 0)
                parentStepper.LightOnSteps(); // ���� ��
            else
                parentStepper.LightOffSteps(); // ���� ��
        }
    }
}
