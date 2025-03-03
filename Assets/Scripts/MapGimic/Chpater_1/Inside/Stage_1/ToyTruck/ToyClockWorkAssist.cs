using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyClockWorkAssist : MonoBehaviour, IPartsOwner
{
    public GameObject TruckClockWork;   // 활성화 할 트럭 태엽

    public void InsertOwnerFunc(GameObject soundPieceObj, int index)
    {
        // 잠시 뒤에 ClockWork 오브젝트로 바꿔버림
        StartCoroutine(ChangeLayerWithDelay(1.1f));
    }
    private IEnumerator ChangeLayerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TruckClockWork.SetActive(true);
        Destroy(gameObject);
    }



    public void RemoveOwnerFunc(int index)
    {
       
    }
}
