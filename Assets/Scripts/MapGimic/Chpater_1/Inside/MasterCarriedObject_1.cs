using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCarriedObject_1 : CarriedObject
{
    public override void PickUpEvent()
    {
        base.PickUpEvent();

        StartCoroutine(ChangeLayerCoroutine());
    }

    private IEnumerator ChangeLayerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        Debug.Log("레이어 변경");
    }



}
