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

    public void SetDropBox(GameObject dropBox)
    {
        dropBox.transform.parent = null;
    }
}
