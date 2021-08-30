using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSZombie
{
    public class RZUIHUD : MonoBehaviour
    {
        [SerializeField] public HUDEnum hudEnum;

        [SerializeField] public SceneEnum sceneEnum;

        private void Reset()
        {
            foreach (var hudE in Enum.GetValues(typeof(HUDEnum)))
            {
                if (hudE.ToString() == "RZUI" + GetType().Name)
                {
                    hudEnum = (HUDEnum)hudE;
                    return;
                }
            }
        }

        public void CloseHUD()
        {
            Destroy(gameObject);
        }

    }

}
