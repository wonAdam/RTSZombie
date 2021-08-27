using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RTSZombie
{
    public class SimpleAttacker : MonoBehaviour
    {
        // Animator Trigger와 이름이 같아야합니다.
        public enum StateType
        {
            NONE,
            Idle,
            Run,
            Attack,
            Dead
        }

        public Dictionary<StateType, Func<StateType>> transitionConditions = new Dictionary<StateType, Func<StateType>>();

        protected virtual void Start()
        {
            transitionConditions.Add(StateType.Idle, IdleCondition);
            transitionConditions.Add(StateType.Run, RunCondition);
            transitionConditions.Add(StateType.Attack, AttackCondition);
            transitionConditions.Add(StateType.Dead, DeadCondition);
        }

        protected virtual StateType IdleCondition()
        {
            return StateType.NONE;
        }

        protected virtual StateType RunCondition()
        {
            return StateType.NONE;
        }

        protected virtual StateType DeadCondition()
        {
            return StateType.NONE;
        }

        protected virtual StateType AttackCondition()
        {
            return StateType.NONE;
        }
    }
}

