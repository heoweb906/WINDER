using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Kyungsoo : MonoBehaviour
{
    private Animator anim;
    public Kyungsoo_StateMachine machine;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        anim = GetComponent<Animator>();
        machine = new Kyungsoo_StateMachine(this);
    }


    private void Update()
    {
        machine?.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }




    public Animator GetAnimator()
    {
        return anim;
    }



}
