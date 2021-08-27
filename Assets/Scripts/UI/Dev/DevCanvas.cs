using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class DevCanvas : MonoBehaviour
    {


        public static DevCanvas Instance { get; protected set; }

        [SerializeField] private List<KeyCode> devOpenKey;

        [SerializeField] private Animator devCanvasAnim;

        // Start is called before the first frame update
        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            ProcessDevOpenKey();
        }

        private void ProcessDevOpenKey()
        {
            if (RZInputHelper.IsKeysPressed(devOpenKey))
            {
                if (devCanvasAnim != null)
                {
                    bool isOpened = devCanvasAnim.GetBool("IsOpened");
                    devCanvasAnim.SetBool("IsOpened", !isOpened);
                    devCanvasAnim.SetTrigger("OpenCloseTrigger");
                }
            }
        }
    }


}
