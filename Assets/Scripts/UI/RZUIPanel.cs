using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZUIPanel : MonoBehaviour
    {
        [SerializeField] public PanelEnum panelEnum;

        [SerializeField] public bool duplicatable;

        private void Reset()
        {
            foreach (var uiE in Enum.GetValues(typeof(PanelEnum)))
            {
                if (uiE.ToString() == "RZUI" + GetType().Name)
                {
                    panelEnum = (PanelEnum)uiE;
                    return;
                }
            }
        }

        public virtual void ClosePanel()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            RZUIManager.Instance.OnPanelDestroyed(this);
        }
    }
}

