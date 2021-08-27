using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZGameManager : SingletonBehaviour<RZGameManager>
    {
        [SerializeField] private DevCanvas devCanvas;

        /// <summary>
        ///     Unity3D Awake method.
        /// </summary>
        /// <remarks>
        ///     This method will only be called once even if multiple instances of the
        ///     singleton MonoBehaviour exist in the scene.
        ///     You can override this method in derived classes to customize the initialization of your MonoBehaviour
        /// </remarks>
        protected override void SingletonAwakened()
        {
            if (RZDebug.IsEditorPlay())
            {
                if(devCanvas != null)
                    Instantiate(devCanvas);
            }

        }

        /// <summary>
        ///     Unity3D Start method.
        /// </summary>
        /// <remarks>
        ///     This method will only be called once even if multiple instances of the
        ///     singleton MonoBehaviour exist in the scene.
        ///     You can override this method in derived classes to customize the initialization of your MonoBehaviour
        /// </remarks>
        protected override void SingletonStarted()
        {

        }


        /// <summary>
        ///     Unity3D OnDestroy method.
        /// </summary>
        /// <remarks>
        ///     This method will only be called once even if multiple instances of the
        ///     singleton MonoBehaviour exist in the scene.
        ///     You can override this method in derived classes to customize the initialization of your MonoBehaviour
        /// </remarks>
        protected override void SingletonDestroyed()
        {

        }


        /// <summary>
        ///     If a duplicated instance of a Singleton MonoBehaviour is loaded into the scene
        ///     this method will be called instead of SingletonAwakened(). That way you can customize
        ///     what to do with repeated instances.
        /// </summary>
        /// <remarks>
        ///     The default approach is delete the duplicated MonoBehaviour
        /// </remarks>
        protected override void NotifyInstanceRepeated()
        {
            Destroy(gameObject);
        }
    }


}
