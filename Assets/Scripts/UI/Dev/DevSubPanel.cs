using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie.Dev
{
    public class DevSubPanel : DevSideOpenPanel
    {
        [HideInInspector] public DevPanel devPanel;

        public void OnClickedCloseButton()
        {
            if(ClosePanel())
            {
                devPanel.OpenPanel();
            }
        }
    }

}
