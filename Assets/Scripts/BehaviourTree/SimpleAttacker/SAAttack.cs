using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class SAAttack : Action
    {
        private RZSimpleAttacker owner;
        public override void OnStart()
        {
            owner = transform.GetComponent<RZSimpleAttacker>();
            owner.simpleAttackerAnimator.SetTrigger(RZSimpleAttacker.StateType.Attack.ToString());
        }

        public override TaskStatus OnUpdate()
        {
            owner.transform.LookAt(owner.target, Vector3.up);
            return TaskStatus.Running;
        }
    }

}
