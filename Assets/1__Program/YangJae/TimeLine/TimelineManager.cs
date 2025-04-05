using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private PlayableAsset[] timelines;
    [SerializeField]
    private GameObject dropBoxObject;
    [SerializeField]
    private List<GameObject> dropObjects;



    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayTimeline(int timelineID)
    {
        playableDirector.Play(timelines[timelineID]);
    }

    public void SetNPCAvoid(NPC_Simple npc){
        npc.SetAvoidState();
    }

    public void SetNPCDrop(NPC_Simple npc)
    {
        npc.SetDropState();
    }

    public void AddForceDropObjects(){
        foreach (GameObject dropObject in dropObjects){
            dropObject.transform.parent = null;
            dropObject.GetComponent<Rigidbody>().isKinematic = false;
            dropObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 50f + Vector3.left * 25f,ForceMode.Impulse);
        }
    }

    public void SetDropBox(GameObject dropBox)
    {
        dropBox.transform.position = dropBoxObject.transform.position;
        dropBox.transform.rotation = dropBoxObject.transform.rotation;
        dropBoxObject.SetActive(false);
        dropBox.SetActive(true);
    }
}
