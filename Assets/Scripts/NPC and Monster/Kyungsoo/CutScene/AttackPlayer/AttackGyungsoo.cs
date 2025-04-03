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


    public CineCameraChager cineChanger;
    public RoadCarCreator_City roadCarCreatorCity;
    public HandheldCamera_DollyCart cameraShake;
    public GameObject[] objCut;




    public void CutScencStart()
    {
        
        roadCarCreatorCity.bCarCreate = true;
        nowCoroutine = StartCoroutine(CutScencStart_Attack());
    }

    IEnumerator CutScencStart_Attack()
    {
        yield return new WaitForSeconds(1.2f);

        anim.SetTrigger("doAttackPlayer");
        cineChanger.CameraChange();
        cameraShake.TriggerStrongShake(2f, 0.6f);


        yield return new WaitForSeconds(1f);


        cart.enabled = true;
        anim.SetBool("Bool_Walk", true);


        yield return new WaitForSeconds(fDissapointTime);


        CutSceneStop();
    }
    private void CutSceneStop()
    {
        for(int i = 0; i < objCut.Length; ++i)
        {
            Destroy(objCut[i]);
        }


        GameAssistManager.Instance.GetPlayerScript().SetCanExit(true);
        GameAssistManager.Instance.GetPlayerScript().SetPlayerDirectionLock(false, null);


        StopCoroutine(nowCoroutine);
        Destroy(gameObject);
    }
}
