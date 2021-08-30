using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    [System.Serializable]
    public class HUDData
    {
        [SerializeField] public RZUIHUD hudPrefab;

        [SerializeField] public SceneEnum lifeTimeScene;
    }

    [System.Serializable]
    public class PanelEnum_UIPanelPrefab : SerializableDictionaryBase<PanelEnum, RZUIPanel> { }

    [System.Serializable]
    public class HUDEnum_HUDData : SerializableDictionaryBase<HUDEnum, HUDData> { }

    [CreateAssetMenu(fileName = "UIData", menuName = "RTSZombie/UIData", order = 0)]
    public class RZUIData : RZStaticData<RZUIData>
    {
        [SerializeField] public PanelEnum_UIPanelPrefab panelMap;

        [SerializeField] public HUDEnum_HUDData hudMap;

        public RZUIPanel GetPanel(PanelEnum uiEnum)
        {
            if (panelMap.TryGetValue(uiEnum, out RZUIPanel panel))
                return panel;
            else
                return null;
        }

        public RZUIHUD GetHUD(HUDEnum hudEnum)
        {
            if (hudMap.TryGetValue(hudEnum, out HUDData hudData))
                return hudData.hudPrefab;
            else
                return null;
        }
    }
}


