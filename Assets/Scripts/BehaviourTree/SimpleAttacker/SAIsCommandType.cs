using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace RTSZombie
{
	public class SAIsCommandType : Conditional
	{
		[SerializeField] private SACommand.CommandType commandType;

		private RZSimpleAttacker owner;

        public override void OnStart()
        {
			owner = GetComponent<RZSimpleAttacker>();
        }
        public override TaskStatus OnUpdate()
		{
			if(owner.currCommand == null)
				return TaskStatus.Failure;

			if (commandType == owner.currCommand.GetCommandType())
				return TaskStatus.Success;
			else
				return TaskStatus.Failure;
		}
	}
}
