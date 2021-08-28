using BehaviorDesigner.Runtime.Tasks;
using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAAttack : Action
{
    private SimpleAttacker owner;
    public override void OnStart()
    {
        owner = transform.GetComponent<SimpleAttacker>();
        owner.simpleAttackerAnimator.SetTrigger(SimpleAttacker.StateType.Attack.ToString());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
