using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickOnBlock : MonoBehaviour
{
    public GameObject trickWall;
    public int iIndexOnStage;
    public int iIndexOffStage;


    private bool bInPlayer = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bInPlayer)
        {
            bInPlayer = true;
            ActionScript();
        }
    }


    private void ActionScript()
    {
        if (trickWall != null) trickWall.SetActive(true);

        InsideAssistGyungSoo.Instance.StageSetActiveFalse(iIndexOffStage);
        InsideAssistGyungSoo.Instance.StageSetActiveTrue(iIndexOnStage);
       
    }

}
