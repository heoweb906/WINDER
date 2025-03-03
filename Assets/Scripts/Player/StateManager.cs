using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Move,
    Pause
}

public class StateManager : MonoBehaviour
{
    [HideInInspector]
    public static StateManager instance = null;

    [SerializeField]
    private PlayerMovement playerMovement;

    public PlayerState state = PlayerState.Move; 

    private void Awake()
    {
        instance = this;
    }


    public bool CompareState(PlayerState _state)
    {
        if (state == _state) return true;
        else return false;
    }

    public void SetState(PlayerState _state)
    {
        state = _state;
    }


}
