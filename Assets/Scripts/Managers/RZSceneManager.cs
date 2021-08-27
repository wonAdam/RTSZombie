using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTSZombie
{
    public class RZSceneManager : SingletonBehaviour<RZSceneManager>
    {
        protected override void SingletonAwakened()
        {

        }

        protected override void SingletonStarted()
        {

        }

        protected override void SingletonDestroyed()
        {

        }

        /// </remarks>
        protected override void NotifyInstanceRepeated()
        {
            Destroy(gameObject);
        }


        public void LoadScene(SceneType type, Action<float> onLoad = null)
        {
            StartCoroutine(LoadSceneCoroutine(type, onLoad));
        }

        private IEnumerator LoadSceneCoroutine(SceneType type, Action<float> onLoad)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(type.ToString(), LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                yield return null;

                if(onLoad != null)
                    onLoad(asyncOperation.progress);
            }

            if (onLoad != null)
                onLoad(1.0f);

            yield return new WaitForSeconds(1f);
            asyncOperation.allowSceneActivation = true;
        }
    }

}
