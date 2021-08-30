using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    [System.Serializable]
    public class UIEnum_UIPanelPrefab : SerializableDictionaryBase<UIEnum, RZUIPanel> { }

    [CreateAssetMenu(fileName = "UIData", menuName = "RTSZombie/UIData", order = 0)]
    public class RZUIData : RZStaticData<RZUIData>
    {
        [SerializeField] public UIEnum_UIPanelPrefab panelMap;

        public RZUIPanel GetPanel(UIEnum uiEnum)
        {
            if (panelMap.TryGetValue(uiEnum, out RZUIPanel panel))
                return panel;
            else
                return null;
        }
    }
}


