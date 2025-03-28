using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGyungsoo : MonoBehaviour
{
    private Coroutine nowCoroutine;
    public Animator anim;
    public CinemachineDollyCart cart;

    public float fDissapointTime;
    
  


    public void CutScencStart()
    {
        nowCoroutine = StartCoroutine(CutScencStart_Attack());
    }

    IEnumerator CutScencStart_Attack()
    {
        yield return new WaitForSeconds(1f);

        anim.SetTrigger("doAttackPlayer");


        yield return new WaitForSeconds(3f);


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
}
