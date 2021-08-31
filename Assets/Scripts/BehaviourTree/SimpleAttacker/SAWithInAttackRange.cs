using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAWithInAttackRange : Conditional
{
    private RZSimpleAttacker owner;
    public override void OnStart()
    {
        owner = transform.GetComponent<RZSimpleAttacker>();
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
