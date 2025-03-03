using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : InteractableObject
{
    public Rigidbody rigid;
    public Collider col;
    public ConfigurableJoint joint;
    public List<Transform> grabPosition;

    private void Start()
    {
        type = InteractableType.Grab;
        canInteract = true;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public Transform GetClosestPosition(Transform _tf)
    {
        Transform closestPos = grabPosition[0];

        Vector2 vec1 = new Vector2(_tf.position.x, _tf.position.z);
        Vector2 vec2 = new Vector2(grabPosition[0].position.x, grabPosition[0].position.z);

        float closestDis = Vector2.Distance(vec1, vec2);

        if (grabPosition[1] == null) return closestPos;

        for(int i = 1; i < grabPosition.Count; i++)
        {
            vec2 = new Vector2(grabPosition[i].position.x, grabPosition[i].position.z);
            float curDis = Vector2.Distance(vec1, vec2);

            if (closestDis > curDis)
            {
                closestPos = grabPosition[i];
                closestDis = curDis;
            }
        }
        return closestPos;
    }


    public void AddJoint(Transform _tf)
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.anchor = _tf.localPosition + Vector3.down*0.5f;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
    }

    public void DeleteJoint()
    {
        joint = null;
        Destroy(gameObject.GetComponent<ConfigurableJoint>());
    }
}
