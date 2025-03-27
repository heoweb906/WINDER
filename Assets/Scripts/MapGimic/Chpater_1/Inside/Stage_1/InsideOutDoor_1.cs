using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideOutDoor_1 : MonoBehaviour
{
    public GameObject door;
    private bool bInPlayer = false;


    private void OnTriggerEnter(Collider other)
    {
       

        if (other.CompareTag("Player") && !bInPlayer)
        {
        

            bInPlayer = true;
            OpenDoor();
        }
    }



    private void OpenDoor()
    {
        if (door != null)
            door.transform.DORotate(new Vector3(0, 140, 0), 7f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutQuad);
    }

       
}


