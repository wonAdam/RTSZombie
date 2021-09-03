using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    [CreateAssetMenu(fileName = "GlobalConfig", menuName = "RTSZombie/GlobalConfig", order = 0)]
    public class RZGlobalConfig : RZStaticData<RZGlobalConfig>
    {
        [SerializeField] public float cameraMovementSpeed;

        [SerializeField] public int cameraMoveThreshold = 20;

        [SerializeField] public KeyCode cameraUp;

        [SerializeField] public KeyCode cameraDown;

        [SerializeField] public KeyCode cameraRight;

        [SerializeField] public KeyCode cameraLeft;

    }

}

