using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    [CreateAssetMenu(fileName = "GlobalData", menuName = "RTSZombie/GlobalData", order = 0)]
    public class RZGlobalData : ScriptableObject
    {
        public static RZGlobalData Instance;

        [SerializeField] public RZUnitDataContainer unitData;
    }
}

