using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZUIPanel : MonoBehaviour
    {
        [SerializeField] public PanelEnum uiEnum;

        [SerializeField] public bool duplicatable;

        private void Reset()
        {
            foreach (var uiE in Enum.GetValues(typeof(PanelEnum)))
            {
                if (uiE.ToString() == "RZUI" + GetType().Name)
                {
                    uiEnum = (PanelEnum)uiE;
                    return;
                }
            }
        }

        public virtual void ClosePanel()
        {
            Destroy(gameObject);
        }
    }
}

