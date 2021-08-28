using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAIdle : Action
{
    public override void OnStart()
    {
        SimpleAttacker owner = transform.GetComponent<SimpleAttacker>();
        owner.simpleAttackerAnimator.SetTrigger(SimpleAttacker.StateType.Idle.ToString());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
