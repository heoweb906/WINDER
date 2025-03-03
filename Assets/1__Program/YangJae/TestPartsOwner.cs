using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPartsOwner : MonoBehaviour, IPartsOwner
{
    // Start is called before the first frame update
    public void InsertOwnerFunc(GameObject parts, int iIndex = 0)
    {
        Debug.Log("InsertOwnerFunc");
    }
    public void RemoveOwnerFunc(int iIndex = 0)
    {
        Debug.Log("RemoveOwnerFunc");
    }
}
