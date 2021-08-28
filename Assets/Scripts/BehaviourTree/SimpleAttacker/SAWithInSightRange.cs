using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAWithInSightRange : Conditional
{
    private SimpleAttacker owner;
    public override void OnStart()
    {
        owner = transform.GetComponent<SimpleAttacker>();
    }

    public override TaskStatus OnUpdate()
    {
        if(owner.IsTargetInSightRange())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
