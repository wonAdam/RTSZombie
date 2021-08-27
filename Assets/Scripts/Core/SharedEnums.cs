using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{ 
    public enum ManagerType
    {
        Game,
        Scene,
    }

    public enum SceneType
    {
        Main,
        World,
    }

    public enum StateStage
    {
        OnEnter,
        OnUpdate,
        OnExit
    }

    [System.Serializable]
    public class Condition : SerializableCallback<bool> { }
}
