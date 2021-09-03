using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class Goliath : RZSimpleAttacker
    {
        [SerializeField /*DEBUG*/] public List<Transform> attackVFXPosition;

        [SerializeField /*DEBUG*/] public ParticleSystem attackVFXPrefab;

        protected override void Start()
        {
            base.Start();

            attackVFXPosition.Clear();

            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.name.StartsWith(SharedValue.ShotVFXSpawnTransform))
                    attackVFXPosition.Add(child);
            }

            RZUnitData unitData = Resources.Load<RZUnitDataContainer>("Data/Unit/UnitDataContainer").dataPerUnit[unitEnum];
            attackVFXPrefab = unitData.attackVFXPrefab;
            Debug.Assert(attackVFXPrefab != null);
        }

        public void SpawnShotVFX()
        {
            foreach(var tr in attackVFXPosition)
            {
                Instantiate(attackVFXPrefab, tr.position, tr.rotation, tr);
            }
        }
    }
}

