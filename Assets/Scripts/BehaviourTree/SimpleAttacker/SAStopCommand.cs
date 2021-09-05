using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace RTSZombie
{
	public class SAStopCommand : Action
	{
		public override void OnStart()
		{

		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}

}
