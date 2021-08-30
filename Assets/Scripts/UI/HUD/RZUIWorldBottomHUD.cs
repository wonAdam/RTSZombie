using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie.UI
{
    public class RZUIWorldBottomHUD : RZUIHUD
    {
        [SerializeField] public ActionSubPanel actionPanel;

        [SerializeField] public MapSubPanel MapPanel;

        [SerializeField] public UnitSubPanel unitPanel;
    }
}

