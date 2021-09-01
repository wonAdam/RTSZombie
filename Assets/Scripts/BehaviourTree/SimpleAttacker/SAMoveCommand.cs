using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace RTSZombie
{
	public class SAMoveCommand : Conditional
	{
		private RZSimpleAttacker owner;
		public override void OnStart()
		{
			owner = transform.GetComponent<RZSimpleAttacker>();
		}

		public override TaskStatus OnUpdate()
		{
			if (owner.moveCommand != null)
            {
				return TaskStatus.Success;
			}
			else // MoveCommand가 아님
            {
				return TaskStatus.Failure;
            }
		}
	}
}

