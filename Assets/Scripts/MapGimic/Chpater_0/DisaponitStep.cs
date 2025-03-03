using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class DisaponitStep : MonoBehaviour
{
    public RayfireRigid rayfireRigid;
    public RayfireBomb rayfireBomb;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M)) BrokeStep();

    }

    void BrokeStep()
    {
        rayfireRigid.Demolish();
        rayfireBomb.Explode(0.05f);
    }
}
