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

        private bool firstTimeSceneLoaded = true;


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

            if(!firstTimeSceneLoaded)
            {
                foreach (var panel in panelInstances)
                    panel.ClosePanel();

                foreach (var hud in hudInstances)
                    hud.CloseHUD();

                panelInstances.Clear();
                hudInstances.Clear();
            }

            foreach(var hudData in RZUIData.Instance.hudMap)
            {
                var hudPrefab = hudData.Value.hudPrefab;
                var hudLifeTimeScene = hudData.Value.lifeTimeScene;

                if(RZSceneManager.Instance.GetCurrentScene() == hudLifeTimeScene)
                {
                    OpenHUD(hudPrefab);
                }
            }

            if (firstTimeSceneLoaded) 
                firstTimeSceneLoaded = false;
        }

        public RZUIHUD GetHUD(HUDEnum hudToFind)
        {
           return hudInstances.Find(hud => hud.hudEnum == hudToFind);
        }

        public T GetHUD<T>() where T : RZUIHUD
        {
            return (T)hudInstances.Find(hub => hub.GetType() == typeof(T));
        }

        public bool CloseHUD(HUDEnum hudToClose)
        {
            // 열려있는 HUD들을 검사하여 요청이 들어온 HUD이 있는지 확인
            // 있으면 전부 닫고 return true
            // 없으면 return false

            List<RZUIHUD> hudsOpened = hudInstances.FindAll(hud => hud.hudEnum == hudToClose);

            if (hudsOpened.Count == 1)
            {
                hudsOpened[0].CloseHUD();
                return true;
            }
            else if(hudsOpened.Count == 0)
            {
                return false;
            }
            else
            {
                RZDebug.LogError(this, "Multiple Same HUD Opened");
                return false;
            }
        }

        public bool CloseHUD(RZUIHUD hudInstance)
        {
            return CloseHUD(hudInstance.hudEnum);
        }

        public RZUIHUD OpenHUD(HUDEnum hudEnum)
        {
            if(RZUIData.Instance.hudMap.TryGetValue(hudEnum, out HUDData hudData))
            {
                if(hudData.lifeTimeScene == RZSceneManager.Instance.GetCurrentScene())
                {
                    List<RZUIHUD> hudsSpawned = hudInstances.FindAll(hud => hud.hudEnum == hudEnum);

                    if(hudsSpawned.Count == 1)
                    {
                        hudsSpawned[0].OpenHUD();
                        return hudsSpawned[0];
                    }

                    else if(hudsSpawned.Count == 0)
                        RZDebug.LogError(this, "Spawn되어야할 HUD가 해당 Scene에 Spawn되어있지 않았음.");

                    else
                        RZDebug.LogError(this, "HUD가 한개 이상 열려있었음.");
                }
                else
                    RZDebug.LogError(this, "해당 Scene에 맞지않는 HUD를 열려고 시도했음.");
            }
            else
                RZDebug.LogError(this, "HUD Enum은 있지만 UIData에 등록되지 않은 HUD를 열려고 시도했음.");

            return null;
        }

        // HUD는 Scene이 시작할 때 UIManager가 여므로 private으로 유지한다.
        private RZUIHUD OpenHUD(RZUIHUD hudPrefabToOpen)
        {
            List<RZUIHUD> hudsOpened = hudInstances.FindAll(hud => hud == hudPrefabToOpen);

            // 열려있는 같은 HUD이 없다면
            if (hudsOpened.Count == 0)
            {
                var newHud = Instantiate(hudPrefabToOpen, hudCanvasInstance.transform);
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

                    Destroy(hudToDestroy);
                }
                return null;
            }
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
                    List<RZUIPanel> panelsOpened = panelInstances.FindAll(panel => panel.panelEnum == uiToOpen);

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

        public RZUIPanel GetPanel(PanelEnum panelEnum)
        {
            return panelInstances.Find(panel => panel.panelEnum == panelEnum);
        }

        public T GetPanel<T>() where T : RZUIPanel
        {
            return (T)panelInstances.Find(panel => panel.GetType() == typeof(T));
        }

        public bool ClosePanel(PanelEnum uiToClsoe)
        {
            // 열려있는 Panel들을 검사하여 요청이 들어온 Panel이 있는지 확인
            // 있으면 닫고 return true
            // 없으면 return false
            List<RZUIPanel> panelsOpened = panelInstances.FindAll(panel => panel.panelEnum == uiToClsoe);

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
            return ClosePanel(panelInstance.panelEnum);
        }

        public void OnHUDDestroyed(RZUIHUD hud)
        {
            hudInstances.RemoveAll(h => h == hud);
        }

        public void OnPanelDestroyed(RZUIPanel panel)
        {
            panelInstances.RemoveAll(p => p == panel);
        }

    }

}
