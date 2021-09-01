using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTSZombie
{
    public abstract class RZUnit : MonoBehaviour
    {
        [SerializeField] public UnitEnum unitEnum;

        [SerializeField] public NavMeshAgent navMeshAgent;

        [HideInInspector] public HashSet<RZUnit> friendlyUnitCollisions = new HashSet<RZUnit>();

        [SerializeField /*DEBUG*/] private GameObject selectionIndicatorInstance;

        [SerializeField /*DEBUG*/] private GameObject selectionIndicatorPrefab;

        protected virtual void Reset()
        {
            foreach (var unitE in Enum.GetValues(typeof(UnitEnum)))
            {
                if(unitE.ToString() == GetType().Name)
                {
                    unitEnum = (UnitEnum)unitE;
                }
            }

            navMeshAgent = GetComponent<NavMeshAgent>();

            NavMeshHit closestHit;

            if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
                gameObject.transform.position = closestHit.position;
            else
                Debug.LogError("Could not find position on NavMesh!");

            RZUnitDataContainer container = Resources.Load<RZUnitDataContainer>("Data/Unit/UnitDataContainer");

            RZUnitData unitData = container.dataPerUnit[unitEnum];

            gameObject.tag = unitData.selfTag;

            gameObject.layer = (int)Mathf.Log(unitData.selfLayer, 2);

            selectionIndicatorPrefab = container.selectionIndicatorPrefab;
        }

        public void SetSelected(bool isSelected)
        {
            if(selectionIndicatorInstance == null)
                selectionIndicatorInstance = Instantiate(selectionIndicatorPrefab, transform.position, transform.rotation, transform);

            selectionIndicatorInstance.SetActive(isSelected);
        }

        public abstract void Move(Vector3 destination);

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.layer == gameObject.layer && collision.transform.GetComponent<RZUnit>() != null) // same side
            {
                friendlyUnitCollisions.Add(collision.transform.GetComponent<RZUnit>());
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == gameObject.layer && collision.transform.GetComponent<RZUnit>() != null) // same side
            {
                friendlyUnitCollisions.Remove(collision.transform.GetComponent<RZUnit>());
            }
        }
    }
}
