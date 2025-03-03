using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MovingPlatform : MonoBehaviour
{
    private Vector3 prePosition;
    private Vector3 curPosition;

  
    

    public virtual void FixedUpdate()
    {
        prePosition = curPosition;
        curPosition = gameObject.transform.position;
    }

    public Vector3 GetPlatformVelocity()
    {
        Vector3 velo = (curPosition - prePosition) / Time.fixedDeltaTime;
        velo.y = 0;
        return velo;
    }

}
