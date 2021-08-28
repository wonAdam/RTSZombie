using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAWithInAttackRange : Conditional
{
    private SimpleAttacker owner;
    public override void OnStart()
    {
        owner = transform.GetComponent<SimpleAttacker>();
    }

    public override TaskStatus OnUpdate()
    {
        if (owner.IsTargetInAttackRange())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
