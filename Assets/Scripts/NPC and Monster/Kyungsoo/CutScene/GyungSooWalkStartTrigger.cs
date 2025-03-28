using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyungSooWalkStartTrigger : MonoBehaviour
{
    private Coroutine nowCoroutine;

    public Animator anim;
    public CinemachineDollyCart cart;

    public float fDissapointTime;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CutScencStart();
        }
    }


    public void CutScencStart()
    {
        nowCoroutine = StartCoroutine(CutScencStart_Attack());
    }

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
}
