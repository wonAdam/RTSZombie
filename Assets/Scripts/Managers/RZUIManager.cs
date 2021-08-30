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

        public static MainCanvas mainCanvasInstance;

        private static EventSystem mainEventSystemInstance;

        [SerializeField] private List<RZUIPanel> panelInstances = new List<RZUIPanel>();

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

        public RZUIPanel OpenPanel(UIEnum uiToOpen)
        {
            // 중복해서 열수 있는 Panel인지 확인 후 
            // 중복해서 열수 있는 Panel이라면 바로 열어줍니다.
            // 아니라면 열려있는 같은 종류의 Panel이 있는지 확인
            // 중복해서 열려있는 일이 없도록 합니다.

            RZUIPanel panelPrefab = RZUIData.Instance.GetPanel(uiToOpen);
            if (panelPrefab != null)
            {
                // 중복해서 열 수 있는 Panel인지 먼저 확인
                if (panelPrefab.duplicatable)
                {
                    var newPanel = Instantiate(panelPrefab, mainCanvasInstance.transform);
                    panelInstances.Add(newPanel);
                    return newPanel;
                }
                else
                {
                    List<RZUIPanel> panelsOpened = panelInstances.FindAll(panel => panel.uiEnum == uiToOpen);

                    // 열려있는 같은 Panel이 없다면
                    if(panelsOpened.Count == 0)
                    {
                        var newPanel = Instantiate(panelPrefab, mainCanvasInstance.transform);
                        panelInstances.Add(newPanel);
                        return newPanel;
                    }
                    // 한개라면 열지않고 null return
                    else if(panelsOpened.Count == 1)
                    {
                        return null;
                    }
                    // 한개 이상이라면 하나 빼고 Destroy, null return
                    else
                    {
                        bool isFirstOne = true;
                        foreach(var panelToDestroy in panelsOpened)
                        {
                            if(isFirstOne)
                            {
                                isFirstOne = false;
                                continue;
                            }

                            panelToDestroy.ClosePanel();
                        }
                        return null;
                    }

                }
            }
            else
            {
                RZDebug.LogError(this, "등록되지 않은 Panel을 열려고 시도했음.");
            }

            return null;
        }

        public bool ClosePanel(UIEnum uiToClsoe)
        {
            // 열려있는 Panel들을 검사하여 요청이 들어온 Panel이 있는지 확인
            // 있으면 닫고 return true
            // 없으면 return false

            List<RZUIPanel> panelsOpened = panelInstances.FindAll(panel => panel.uiEnum == uiToClsoe);

            if(panelsOpened.Count > 0)
            {
                panelInstances.Remove(panelsOpened[0]);
                panelsOpened[0].ClosePanel();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ClosePanel(RZUIPanel panelInstance)
        {
            RZUIPanel panelOpened = panelInstances.Find(panel => panel == panelInstance);

            if(panelOpened != null)
            {
                panelInstances.Remove(panelOpened);
                panelOpened.ClosePanel();
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
