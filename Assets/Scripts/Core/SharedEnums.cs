using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{ 
    public enum ManagerEnum
    {
        Game,
        Scene,
        Input,
        UI,
    }

    // 각 Scene의 이름과 똑같아야합니다.
    // RZGameManager.OnSceneLoaded 참고
    public enum SceneEnum
    {
        Main,
        World,
    }

    // 각 Unit의 클래스 타입이름과 똑같아야합니다.
    // RZUnit.Reset 참고
    public enum UnitEnum
    {
        Soldier,
        Zombie,
    }

}
