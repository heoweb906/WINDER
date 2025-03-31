using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GtungsooAtackTriiger : MonoBehaviour
{
    public GameObject gyungsoo;
    private BoxCollider boxCollider;

    private bool bTrigger = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !bTrigger)
        {
            bTrigger = true;

            StartCoroutine(StopPlayer());
            GameAssistManager.Instance.GetPlayerScript().SetPlayerDirectionLock(true, gyungsoo);
        }
    }

    IEnumerator StopPlayer()
    {
        boxCollider.isTrigger = false;
        GameAssistManager.Instance.PlayerInputLockOn();
        yield return new WaitForSeconds(1f);
        GameAssistManager.Instance.PlayerInputLockOff();
    }



}
