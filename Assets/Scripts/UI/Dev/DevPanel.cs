using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSZombie.Dev
{
    public class DevPanel : DevSideOpenPanel
    {
        [SerializeField] private DevButton buttonPrefab;

        public DevButton AddSubPanelButton(DevCanvas devCanvas, string buttonName)
        {
            var buttonInst = Instantiate(buttonPrefab, transform);
            buttonInst.SetButtonText(buttonName);
            return buttonInst;
        }
    }


}
