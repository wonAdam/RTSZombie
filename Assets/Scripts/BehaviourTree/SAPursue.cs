using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPursue : Action
{
    private SimpleAttacker owner;
    public override void OnStart()
    {
        owner = transform.GetComponent<SimpleAttacker>();
        owner.simpleAttackerAnimator.SetTrigger(SimpleAttacker.StateType.Run.ToString());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
