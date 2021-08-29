using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "RTSZombie/UnitData", order = 0)]
    [System.Serializable]
    public class RZUnitData : ScriptableObject
    {
        [SerializeField] public RZUnit unitPrefab;

        [SerializeField] public float sightRange;

        [SerializeField] public float attackRange;

        [SerializeField] public string targetTag;

        [SerializeField] public LayerMask targetLayer;

        [SerializeField] public string selfTag;

        [SerializeField] public LayerMask selfLayer;

    }

}

