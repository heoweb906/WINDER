using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBomb : MonoBehaviour
{
    public RayfireRigid[] rayFireRigidArray;
    public RayfireBomb bomb;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            for (int i = 0; i < rayFireRigidArray.Length; i++)
            {
                if (rayFireRigidArray[i] != null)
                    rayFireRigidArray[i].Initialize();
            }

            bomb.Explode(0.3f);
        }

      


    }
}
