using RotaryHeart.Lib.SerializableDictionary;
using RTSZombie.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTSZombie
{
    [System.Serializable]
    public class ManagerEnum_ManagerPrefab : SerializableDictionaryBase<ManagerEnum, RZManager> { }

    public class RZGameManager : SingletonBehaviour<RZGameManager>
    {
        [SerializeField] private DevCanvas devCanvasPrefab;

        [SerializeField] private ManagerEnum_ManagerPrefab managerPrefabs;

        protected override void SingletonAwakened()
        {
            if (RZDebug.IsEditorPlay())
            {
                if(devCanvasPrefab != null)
                    Instantiate(devCanvasPrefab);
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
            foreach(var sceneEnum in Enum.GetValues(typeof(SceneEnum)))
            {
                if(sceneEnum.ToString() == nextScene.name)
                {
                    RZDebug.Log(this, $"OnSceneLoaded {nextScene.name}");
                    UpdateManagersLifeCycle((SceneEnum)sceneEnum);
                    return;
                }
            }

            RZDebug.Log(this, $"OnSceneLoaded Wrong SceneName");
        }

        private void UpdateManagersLifeCycle(SceneEnum sceneType)
        {
            foreach(var managerPair in managerPrefabs)
            {
                RZManager manager = managerPair.Value;

                if(manager.lifeCycle.Contains(sceneType))
                {
                    if (!manager.IsManagerInstanceExist())
                        manager.CreateManagerInstance();
                }
                else
                {
                    if (manager.IsManagerInstanceExist())
                    {
                        if (manager.neverDestroyOnLoad)
                            continue;

                        manager.DestroyManagerInstance();
                    }
                        
                }
            }
        }
    }


}
