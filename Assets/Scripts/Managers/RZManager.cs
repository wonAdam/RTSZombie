using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public abstract class RZManager : MonoBehaviour
    {
        [SerializeField] public ManagerEnum type;

        [SerializeField] public List<SceneEnum> lifeCycle;

        [SerializeField] public bool neverDestroyOnLoad = false;

        public abstract bool IsManagerInstanceExist();

        public abstract bool DestroyManagerInstance();

        public abstract RZManager CreateManagerInstance();
    }
}

