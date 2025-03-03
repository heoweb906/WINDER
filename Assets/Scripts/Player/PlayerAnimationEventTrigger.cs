using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }


    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        //if (IsInAnimationTransition())
        //{
        //    return;
        //}

        player.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerOnMovementStateAnimationExitEvent()
    {
        //if (IsInAnimationTransition())
        //{
        //    Debug.Log("return");
        //    return;
        //}
        player.OnMovementStateAnimationExitEvent();
    }

    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        //if (IsInAnimationTransition())
        //{
        //    return;
        //}

        player.OnMovementStateAnimationTransitionEvent();
    }

    private bool IsInAnimationTransition(int layerIndex = 0)
    {
        return player.playerAnim.IsInTransition(layerIndex);
    }
}
