using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriedObject : InteractableObject
{
    public PartOwnerType partOwnerType = PartOwnerType.Nothing;
    public CarriedObjectType carriedObjectType = CarriedObjectType.Normal;

    public Rigidbody rigid;
    public Collider col;

    [Header("Hold Position")]
    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    [Header("Put Down Position")]
    public Vector3 putDownPositionOffset;
    public Vector3 putDownRotationOffset;

    [Header("Put Parts Position")]
    public Vector3 putPartsPositionOffset;
    public Vector3 putPartsRotationOffset;

    private void Start()
    {
        type = InteractableType.Carrried;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
}

public enum CarriedObjectType
{
    Normal,
    Guitar,
}
