using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
}
