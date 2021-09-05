using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace RTSZombie
{
	public class SASetAttackTarget : Action
	{
		private RZSimpleAttacker owner;
		public override void OnStart()
		{
			owner = GetComponent<RZSimpleAttacker>();
			RZSimpleAttacker.AttackCommand attackCommand = (RZSimpleAttacker.AttackCommand)owner.currCommand;
			owner.target = attackCommand.target.transform;
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}
