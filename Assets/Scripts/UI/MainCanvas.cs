using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTSZombie.UI
{
    public class MainCanvas : MonoBehaviour
    {
        private RZUIClickReceiver clickReceiver;

        private Canvas canvas;

        private CanvasScaler canvasScaler;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            Debug.Assert(canvas != null);

            canvasScaler = GetComponent<CanvasScaler>();
            Debug.Assert(canvasScaler != null);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);
        }

        public void RegisterDragEvent(Action<Ray, Ray, Ray, Ray> onDragEnd) // bottomLeft, bottomRight, topRight, topLeft
        {
            if(clickReceiver == null)
            {
                clickReceiver = GetComponentInChildren<RZUIClickReceiver>();
                if (clickReceiver == null)
                    return;
            }
            clickReceiver.onDragEnd = onDragEnd;
        }

        public void UnregisterDragEvent()
        {
            if (clickReceiver == null)
            {
                clickReceiver = GetComponentInChildren<RZUIClickReceiver>();
                if (clickReceiver == null)
                    return;
            }
            clickReceiver.onDragEnd = null;
        }

        public void RegisterClickEvent(Action<Ray> onClick)
        {
            if (clickReceiver == null)
            {
                clickReceiver = GetComponentInChildren<RZUIClickReceiver>();
                if (clickReceiver == null)
                    return;
            }
            clickReceiver.onClick = onClick;
        }

        public void UnregisterClickEvent()
        {
            if (clickReceiver == null)
            {
                clickReceiver = GetComponentInChildren<RZUIClickReceiver>();
                if (clickReceiver == null)
                    return;
            }

            clickReceiver.onClick = null;
        }
    }

}
