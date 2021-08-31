using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAIdle : Action
{
    public override void OnStart()
    {
        RZSimpleAttacker owner = transform.GetComponent<RZSimpleAttacker>();
        owner.simpleAttackerAnimator.SetTrigger(RZSimpleAttacker.StateType.Idle.ToString());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
