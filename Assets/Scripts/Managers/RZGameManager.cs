using RTSZombie.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTSZombie
{
    public class RZGameManager : SingletonBehaviour<RZGameManager>
    {
        [SerializeField] private DevCanvas devCanvas;

        [SerializeField] private List<RZManager> managerPrefabs;

        protected override void SingletonAwakened()
        {
            if (RZDebug.IsEditorPlay())
            {
                if(devCanvas != null)
                    Instantiate(devCanvas);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        protected override void SingletonStarted()
        {

        }


        protected override void SingletonDestroyed()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            base.SingletonDestroyed();
        }

        protected override void NotifyInstanceRepeated()
        {
            Destroy(gameObject);
        }

        private void OnSceneLoaded(Scene nextScene, LoadSceneMode sceneMode)
        {
            foreach(var sceneEnum in Enum.GetValues(typeof(SceneType)))
            {
                if(sceneEnum.ToString() == nextScene.name)
                {
                    RZDebug.Log(this, $"OnSceneLoaded {nextScene.name}");
                    UpdateManagersLifeCycle((SceneType)sceneEnum);
                    return;
                }
            }

            RZDebug.Log(this, $"OnSceneLoaded Wrong SceneName");
        }

        private void UpdateManagersLifeCycle(SceneType sceneType)
        {
            foreach(var manager in managerPrefabs)
            {
                if(manager.lifeCycle.Contains(sceneType))
                {

                }
                else
                {
                    if (manager.IsManagerInstanceExist())
                        manager.DestroyManagerInstance();
                }
            }
        }
    }


}
