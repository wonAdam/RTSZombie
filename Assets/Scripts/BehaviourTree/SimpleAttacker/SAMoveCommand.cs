using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Linq;

namespace RTSZombie
{
	public class SAMoveCommand : Action
	{
		private RZSimpleAttacker owner;

		private RZSimpleAttacker.MoveCommand currMoveCommand;

		public override void OnStart()
		{
			owner = transform.GetComponent<RZSimpleAttacker>();

			currMoveCommand = owner.currCommand as RZSimpleAttacker.MoveCommand;
			owner.navMeshAgent.SetDestination(currMoveCommand.destination);
			owner.simpleAttackerAnimator.SetTrigger(RZSimpleAttacker.StateType.Run.ToString());
		}

		public override TaskStatus OnUpdate()
		{
			if(owner.currCommand != currMoveCommand)
            {
				OnStart();
			}

			if (owner.friendlyUnitCollisions.Any(unit => {
				return unit.GetComponent<RZSimpleAttacker>().currCommand == null &&
						RZInputManager.Instance.selectedUnits.Contains(unit);
			}))
			{
				// 옆 아군은 Arrived 그리고 부딪히고 있으므로 자신도 도착한것으로 간주합니다.
				owner.navMeshAgent.velocity = Vector3.zero;
				owner.currCommand = null;
				return TaskStatus.Failure;
			}

			if (!owner.navMeshAgent.pathPending)
			{
				if (owner.navMeshAgent.remainingDistance <= owner.navMeshAgent.stoppingDistance)
				{
					if (!owner.navMeshAgent.hasPath || owner.navMeshAgent.velocity.sqrMagnitude == 0f)
					{
						
						// Arrived
						owner.currCommand = null;
						return TaskStatus.Failure;
					}
				}
			}

			// 아직 목적지에 도달하지 못했음
			return TaskStatus.Running;
		}

        public override void OnEnd()
        {
			owner.navMeshAgent.destination = transform.position;
        }
    }
}
