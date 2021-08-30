using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZUIPanel : MonoBehaviour
    {
        [SerializeField] public UIEnum uiEnum;

        [SerializeField] public bool duplicatable;

        private void Reset()
        {
            foreach (var uiE in Enum.GetValues(typeof(UIEnum)))
            {
                if (uiE.ToString() == "RZUI" + GetType().Name)
                {
                    uiEnum = (UIEnum)uiE;
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

