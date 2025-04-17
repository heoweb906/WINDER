using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyungSooWalkStartTrigger : MonoBehaviour
{
    private Coroutine nowCoroutine;
    private bool bFlag = false;

    public Animator anim;
    public CinemachineDollyCart cart;

    public float fDissapointTime;

    public void CutScencStart()
    {
        nowCoroutine = StartCoroutine(CutScencStart_Attack());
    }

    // Vector3(-27.7299995,-0.154797882,-1.41400003)
    IEnumerator CutScencStart_Attack()
    {

        cart.enabled = true;
        anim.SetBool("Bool_Walk", true);


        yield return new WaitForSeconds(fDissapointTime);

        CutSceneStop();
    }
    private void CutSceneStop()
    {
        StopCoroutine(nowCoroutine);
        Destroy(gameObject);
    }








    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bFlag)
        {
            bFlag = true;
            CutScencStart();
        }
    }


}
