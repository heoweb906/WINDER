using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class GlassWall : MonoBehaviour
{
    public GameObject realWall;
    public GameObject glassWall;

    public RayfireRigid rayfireRigid;
    public RayfireBomb rayfireBomb;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) CrashGlassWall();
    }

    public void CrashGlassWall()
    {
        realWall.SetActive(false);
        glassWall.SetActive(true);

        rayfireRigid.Demolish();
        rayfireBomb.Explode(0.05f);
    }
}
