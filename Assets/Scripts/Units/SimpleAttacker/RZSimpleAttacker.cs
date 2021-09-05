using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RTSZombie
{
    public class RZSimpleAttacker : RZUnit
    {
        public class MoveCommand : SACommand
        {
            public Vector3 destination;

            public MoveCommand(Vector3 destination)
            {
                this.destination = destination;
            }

            public override CommandType GetCommandType() => CommandType.Move;
        }

        public class AttackCommand : SACommand
        {
            public RZUnit target;

            public AttackCommand(RZUnit target)
            {
                this.target = target;
            }

            public override CommandType GetCommandType() => CommandType.Attack;
        }

        public class StopCommand : SACommand
        {
            public StopCommand()
            {
            }

            public override CommandType GetCommandType() => CommandType.Stop;
        }


        // Animator Trigger와 이름이 같아야합니다.
        public enum StateType
        {
            NONE,
            Idle,
            Run,
            Attack,
            Dead
        }

        [SerializeField /*DEBUG*/] public Animator simpleAttackerAnimator;

        [SerializeField /*DEBUG*/] public BehaviorTree behaviorTree;

        [SerializeField /*DEBUG*/] public float sightRadius;

        [SerializeField /*DEBUG*/] public float attackRange;

        [SerializeField /*DEBUG*/] public string targetTag;

        [SerializeField /*DEBUG*/] public LayerMask targetLayer;

        [HideInInspector] public Transform target;

        public SACommand currCommand = null;

        protected override void Start()
        {
            base.Start();
            behaviorTree = GetComponent<BehaviorTree>();
            simpleAttackerAnimator = GetComponentInChildren<Animator>();
            RZUnitData unitData = Resources.Load<RZUnitDataContainer>("Data/Unit/UnitDataContainer").dataPerUnit[unitEnum];
            sightRadius = unitData.sightRange;
            attackRange = unitData.attackRange;
            targetTag = unitData.targetTag;
            targetLayer = unitData.targetLayer;
        }

        public bool HasTarget()
        {
            return target != null;
        }

        public bool IsTargetInAttackRange()
        {
            if (HasTarget())
            {
                if (IsTargetInRange(attackRange))
                    return true;
            }

            return false;
        }

        public bool IsTargetInSightRange()
        {
            // 이미 target이 있음.
            if(HasTarget())
            {
                if (IsTargetInRange(sightRadius))
                    return true;
                else
                    target = null;
            }

            return IsTargetLayerInRange();
        }

        private bool IsTargetInRange(float range)
        {
            float distance = Vector2.Distance(
                        new Vector2(transform.position.x, transform.position.z),
                        new Vector2(target.position.x, target.position.z)
                        );

            if (distance <= range)
            {
                return true;
            }

            return false;
        }

        private bool IsTargetLayerInRange()
        {
            // 아직 target이 없으면 범위내에 새로운 타겟을 찾음
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, sightRadius, targetLayer);

            if (enemyColliders.Length > 0)
            {
                float minDistance = float.MaxValue;
                Transform minDistanceTransform = null;
                foreach (var col in enemyColliders)
                {
                    float distance = Vector2.Distance(
                        new Vector2(transform.position.x, transform.position.z),
                        new Vector2(col.transform.position.x, col.transform.position.z)
                        );

                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        minDistanceTransform = col.transform;
                    }
                }

                target = minDistanceTransform;
                return true;
            }


            target = null;
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            // Sight Radius
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, sightRadius);

            // Attack Radius
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, transform.up, attackRange);
        }

        public override void CommandMove(Vector3 destination)
        {
            currCommand = new MoveCommand(destination);
        }

        public override void CommandAttack(RZUnit target)
        {
            currCommand = new AttackCommand(target);
        }

        public override void CommandStop()
        {
            currCommand = new StopCommand();
        }
    }
}

