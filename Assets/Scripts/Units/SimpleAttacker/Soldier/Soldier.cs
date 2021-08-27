using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class Soldier : SimpleAttacker
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override StateType IdleCondition()
        {
            return base.IdleCondition();
        }

        protected override StateType RunCondition()
        {
            return base.RunCondition();
        }

        protected override StateType AttackCondition()
        {
            return base.AttackCondition();
        }

        protected override StateType DeadCondition()
        {
            return base.DeadCondition();
        }
    }
}

