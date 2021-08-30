using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSZombie.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

namespace RTSZombie
{
    public class RZUIManager : SingletonBehaviour<RZUIManager>
    {
        [SerializeField] public HUDCanvas hudCanvasPrefab;

        [SerializeField] public MainCanvas mainCanvasPrefab;

        [SerializeField] public EventSystem mainEventSystemPrefab;

        public static HUDCanvas hudCanvasInstance;

        public static MainCanvas mainCanvasInstance;

        private static EventSystem mainEventSystemInstance;
        
        [SerializeField /*DEBUG*/ ] private List<RZUIPanel> panelInstances = new List<RZUIPanel>();

        [SerializeField /*DEBUG*/ ] private List<RZUIHUD> hudInstances = new List<RZUIHUD>();

        public static MainCanvas MainCanvas { get => mainCanvasInstance; }

        public static HUDCanvas HUDCanvas { get => hudCanvasInstance; }


        protected override void SingletonAwakened()
        {
            foreach (var hudnCanvas in FindObjectsOfType<HUDCanvas>())
                Destroy(hudnCanvas.gameObject);

            foreach (var mainCanvas in FindObjectsOfType<MainCanvas>())
                Destroy(mainCanvas.gameObject);

            foreach (var eventSystem in FindObjectsOfType<EventSystem>())
                Destroy(eventSystem.gameObject);

            hudCanvasInstance = Instantiate(hudCanvasPrefab);
            mainCanvasInstance = Instantiate(mainCanvasPrefab);
            mainEventSystemInstance = Instantiate(mainEventSystemPrefab);

            DontDestroyOnLoad(hudCanvasInstance.gameObject);
            DontDestroyOnLoad(mainCanvasInstance.gameObject);
            DontDestroyOnLoad(mainEventSystemInstance.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void SingletonStarted()
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        protected override void SingletonDestroyed()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene nextScene, LoadSceneMode loadSceneMode)
        {
            RZDebug.Log(this, $"OnSceneLoaded {nextScene.name}");

            foreach (var panel in panelInstances)
                panel.ClosePanel();

            foreach (var hud in hudInstances)
                hud.CloseHUD();

            panelInstances.Clear();
            hudInstances.Clear();

            foreach(var hudData in RZUIData.Instance.hudMap)
            {
                var hudPrefab = hudData.Value.hudPrefab;
                var hudLifeTimeScene = hudData.Value.lifeTimeScene;

                if(RZSceneManager.Instance.GetCurrentScene() == hudLifeTimeScene)
                {
                    Instantiate(hudPrefab, hudCanvasInstance.transform);
                }
            }
        }

        public RZUIHUD GetHUD(HUDEnum hudToFind)
        {
           return hudInstances.Find(hud => hud.hudEnum == hudToFind);
        }

        // HUD는 Scene이 시작할 때 UIManager가 여므로 private으로 유지한다.
        private RZUIHUD OpenHUD(HUDEnum hudToOpen)
        {
            RZUIHUD hudPrefab = RZUIData.Instance.GetHUD(hudToOpen);
            if (hudPrefab != null)
            {
                List<RZUIHUD> hudsOpened = hudInstances.FindAll(hud => hud.hudEnum == hudToOpen);

                // 열려있는 같은 HUD이 없다면
                if (hudsOpened.Count == 0)
                {
                    var newHud = Instantiate(hudPrefab, hudCanvasInstance.transform);
                    hudInstances.Add(newHud);
                    return newHud;
                }
                // 한개라면 열지않고 null return
                else if (hudsOpened.Count == 1)
                {
                    return null;
                }
                // 한개 이상이라면 하나 빼고 Destroy, null return
                else
                {
                    RZDebug.LogError(this, "HUD가 한개 이상 열려있었음.");
                    bool isFirstOne = true;
                    foreach (var hudToDestroy in hudsOpened)
                    {
                        if (isFirstOne)
                        {
                            isFirstOne = false;
                            continue;
                        }

                        hudToDestroy.CloseHUD();
                    }
                    return null;
                }
            }
            else
            {
                RZDebug.LogError(this, "등록되지 않은 HUD을 열려고 시도했음.");
            }

            return null;
        }

        public RZUIPanel OpenPanel(PanelEnum uiToOpen)
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

        public bool ClosePanel(PanelEnum uiToClsoe)
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
