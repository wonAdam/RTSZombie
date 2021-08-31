using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{ 
    public enum ManagerEnum
    {
        NONE,

        Game,
        Scene,
        Input,
        UI,
        Data,
    }

    // 각 Scene의 이름과 똑같아야합니다. (NONE 제외)
    // RZGameManager.OnSceneLoaded 참고
    public enum SceneEnum
    {
        NONE,

        Main,
        World,
    }

    // 각 "RZUI" + {UI 클래스의 이름}과 똑같아야합니다. (NONE 제외)
    // RZUIPanel.Reset 참고
    public enum PanelEnum
    {
        NONE,

        ClickReceiver,
    }

    // 각 "RZUI" + {UI 클래스의 이름}과 똑같아야합니다. (NONE 제외)
    // RZUIHUD.Reset 참고
    public enum HUDEnum
    {
        NONE,

        WorldBottomHUD,
    }

    // 각 Unit의 클래스 타입이름과 똑같아야합니다. (NONE 제외)
    // RZUnit.Reset 참고
    public enum UnitEnum
    {
        NONE,

        Goliath,
        Gastarias,
    }

    public static class SharedValue
    {
        public static string FriendlyTag = "Friendly";
        public static string EnemyTag = "Enemy";
        public static string ShotVFXSpawnTransform = "ShotVFXSpawn";
        public static string ShotSFXSpawnTransform = "ShotSFXSpawn";
    }

}
