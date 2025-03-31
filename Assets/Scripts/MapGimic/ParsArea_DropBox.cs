using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsArea_DropBox : PartsArea
{

    public GameObject Wall;

    public int ItemCount = 0;
    public override void InsertParts(GameObject partsObj)
    {
        ItemCount++;
        if(ItemCount >= 3){
            Wall.SetActive(false);
        }
    }
    
    
}
