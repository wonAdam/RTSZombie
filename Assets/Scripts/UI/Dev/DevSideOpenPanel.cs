using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSZombie.Dev
{
    public class DevSideOpenPanel : MonoBehaviour
    {
        public bool isOpened { get => panelAnim.GetBool("IsOpened"); }

        [SerializeField] protected Animator panelAnim;

        protected Action onPanelOpen;

        protected Action onPanelClose;

        public virtual bool OpenPanel()
        {
            if (panelAnim == null)
                return false;

            if (!panelAnim.GetBool("ReadyToBeToggled"))
                return false;

            if (!isOpened)
            {
                panelAnim.SetBool("IsOpened", true);
                panelAnim.SetTrigger("OpenCloseTrigger");

                if(onPanelOpen != null)
                    onPanelOpen.Invoke();

                return true;
            }

            return false;
        }

        public virtual bool ClosePanel()
        {
            if (panelAnim == null)
                return false;

            if (!panelAnim.GetBool("ReadyToBeToggled"))
                return false;

            if (isOpened)
            {
                panelAnim.SetBool("IsOpened", false);
                panelAnim.SetTrigger("OpenCloseTrigger");

                if (onPanelClose != null)
                    onPanelClose.Invoke();

                return true;
            }

            return false;
        }
    }


}
