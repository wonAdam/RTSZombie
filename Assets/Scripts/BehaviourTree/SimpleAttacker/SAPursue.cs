using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPursue : Action
{
    private RZSimpleAttacker owner;
    public override void OnStart()
    {
        owner = transform.GetComponent<RZSimpleAttacker>();
        owner.simpleAttackerAnimator.SetTrigger(RZSimpleAttacker.StateType.Run.ToString());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
