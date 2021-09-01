using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class SAWithInSightRange : Conditional
    {
        private RZSimpleAttacker owner;
        public override void OnStart()
        {
            owner = transform.GetComponent<RZSimpleAttacker>();
        }

        public override TaskStatus OnUpdate()
        {
            if (owner.IsTargetInSightRange())
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}

