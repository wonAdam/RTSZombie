using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    [System.Serializable]
    public class UnitEnum_UnitData : SerializableDictionaryBase<UnitEnum, RTSZombie.RZUnitData> { }

    [CreateAssetMenu (fileName = "UnitDataContainer", menuName = "RTSZombie/UnitDataContainer", order = 0)]
    public class RZUnitDataContainer : ScriptableObject
    {
        [SerializeField] public UnitEnum_UnitData dataPerUnit;
    }
}

