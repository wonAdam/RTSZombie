using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZDataManager : SingletonBehaviour<RZDataManager>
    {
        [HideInInspector] public RZUIData uiDataSO;

        [HideInInspector] public RZUnitDataContainer unitDataContainer;

        protected override void SingletonAwakened()
        {
            uiDataSO = Resources.Load<RZUIData>("Data/UI/UIData");
            uiDataSO.Init();
            unitDataContainer = Resources.Load<RZUnitDataContainer>("Data/Unit/UnitDataContainer");
            unitDataContainer.Init();
        }

        protected override void SingletonStarted()
        {

        }

        protected override void SingletonDestroyed()
        {

        }
    }


}
