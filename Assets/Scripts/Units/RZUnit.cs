using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZUnit : MonoBehaviour
    {
        [SerializeField] public UnitEnum unitEnum;

        protected virtual void Reset()
        {
            foreach (var unitE in Enum.GetValues(typeof(UnitEnum)))
            {
                if(unitE.ToString() == GetType().Name)
                {
                    unitEnum = (UnitEnum)unitE;
                }
            }

            RZUnitData unitData = Resources.Load<RZUnitDataContainer>("Data/Unit/UnitDataContainer").dataPerUnit[unitEnum];

            gameObject.tag = unitData.selfTag;

            gameObject.layer = (int)Mathf.Log(unitData.selfLayer, 2);

        }

    }
}
