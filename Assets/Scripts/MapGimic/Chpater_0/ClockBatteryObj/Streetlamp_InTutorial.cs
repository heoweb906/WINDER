using System.Collections;
using UnityEngine;

public class Streetlamp_InTutorial : MonoBehaviour
{
    public GameObject[] Light;


    public void SetLightActive(bool isOn)
    {
        if (Light == null || Light.Length < 2) return;

        Light[0].SetActive(!isOn);
        Light[1].SetActive(isOn);
    }


}