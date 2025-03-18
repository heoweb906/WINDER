using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulBugOnRoad_RunState : BulbBugStateOnRoad
{
    public BulBugOnRoad_RunState(BulbBug_OnRoad bulbBug, BBBmachine machine) : base(bulbBug, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("도망가!!");

        bulbBug.nav.enabled = true;

        bulbBug.anim.SetBool("isWalk", true);

        bulbBug.GetNav().SetDestination(bulbBug.checkPoints[bulbBug.CurrentCheckPointIndex].position);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (bulbBug.GetNav().enabled && !bulbBug.GetNav().pathPending && bulbBug.GetNav().remainingDistance <= bulbBug.GetNav().stoppingDistance)
        {
            MoveToNextCheckPoint();
        }

    }



    public override void OnExit()
    {
        base.OnExit();

        // 삭제하는 로직 만들어라

        
    }

    private void MoveToNextCheckPoint()
    {
        bulbBug.CurrentCheckPointIndex++;
        if (bulbBug.CurrentCheckPointIndex < bulbBug.checkPoints.Length)
        {
            bulbBug.GetNav().SetDestination(bulbBug.checkPoints[bulbBug.CurrentCheckPointIndex].position);
        }
        else
        {
            // Destroy(gameObject); // 마지막 체크포인트 도달 시 삭제
        }
    }
}
