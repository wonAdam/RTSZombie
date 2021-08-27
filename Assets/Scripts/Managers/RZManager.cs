using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public abstract class RZManager : MonoBehaviour
    {
        [SerializeField] public ManagerType type;

        [SerializeField] public List<SceneType> lifeCycle;
    }
}

