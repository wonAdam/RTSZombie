using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class SAPursue : Action
    {
        private RZSimpleAttacker owner;
        public override void OnStart()
        {
            owner = GetComponent<RZSimpleAttacker>();
            owner.simpleAttackerAnimator.SetTrigger(RZSimpleAttacker.StateType.Run.ToString());
            owner.navMeshAgent.destination = owner.target.position;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            owner.navMeshAgent.destination = transform.position;
        }
    }

}
