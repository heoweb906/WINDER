using UnityEngine;
public interface BaseState
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
    public abstract void OnAnimationEnterEvent();
    public abstract void OnAnimationExitEvent();
    public abstract void OnAnimationTransitionEvent();

}
