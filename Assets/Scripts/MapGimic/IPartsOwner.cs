using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartsOwner
{
    void InsertOwnerFunc(GameObject parts, int iIndex = 0);
    void RemoveOwnerFunc(int iIndex = 0);
}
