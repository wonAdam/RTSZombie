using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class GoliathAnimationHelper : MonoBehaviour
    {
        [SerializeField] private Goliath owner;

        private void Start()
        {
            Debug.Assert(owner != null);
        }

        public void SpawnAttackVFX()
        {
            owner.SpawnShotVFX();
        }
    }

}
