using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSZombie.UI;
using UnityEngine.EventSystems;

namespace RTSZombie
{
    public class RZUIManager : SingletonBehaviour<RZUIManager>
    {
        [SerializeField] public MainCanvas mainCanvasPrefab;

        [SerializeField] public EventSystem mainEventSystemPrefab;

        private static MainCanvas mainCanvasInstance;

        private static EventSystem mainEventSystemInstance;

        public static MainCanvas MainCanvas { get => mainCanvasInstance; }

        protected override void SingletonAwakened()
        {
            foreach (var mainCanvas in FindObjectsOfType<MainCanvas>())
                Destroy(mainCanvas.gameObject);

            foreach (var eventSystem in FindObjectsOfType<EventSystem>())
                Destroy(eventSystem.gameObject);

            mainCanvasInstance = Instantiate(mainCanvasPrefab);
            mainEventSystemInstance = Instantiate(mainEventSystemPrefab);

        }

        protected override void SingletonStarted()
        {

        }

        protected override void SingletonDestroyed()
        {
        }

    }

}
