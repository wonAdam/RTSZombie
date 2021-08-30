using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public abstract class RZStaticData<T> : ScriptableObject where T : RZStaticData<T>
    {
        public static T Instance;

        public void Init()
        {
            Instance = (T)this;
        }


    }


}
