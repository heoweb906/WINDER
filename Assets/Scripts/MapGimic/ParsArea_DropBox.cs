using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParsArea_DropBox : PartsArea
{
    [SerializeField]
    private Event_BeforeStation_Controller eventController;

    public List<CarriedObject> items;

    public GameObject Wall;

    public int ItemCount = 0;
    public override void InsertParts(GameObject partsObj)
    {
        CarriedObject item = partsObj.GetComponent<CarriedObject>();
        eventController.ItemPickUp();
        item.canInteract = false;
        Parts = null;
        eventController.RemoveItems(item);
    }

    public void RemoveItem(CarriedObject partsObj){
        items.Remove(partsObj);
    }
    
    public List<CarriedObject> GetItems(){
        return items;
    }
    
}
