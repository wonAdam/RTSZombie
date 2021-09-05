using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace RTSZombie
{
    public class SADoesAnyCommandExists : Conditional
    {
        private RZSimpleAttacker owner;
        public override void OnStart()
        {
            owner = GetComponent<RZSimpleAttacker>();
        }
        public override TaskStatus OnUpdate()
        {
            if (owner.currCommand != null)
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }
    }
}
