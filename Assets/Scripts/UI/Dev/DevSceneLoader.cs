using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RTSZombie.Dev
{
    public class DevSceneLoader : DevSubPanel
    {
        [SerializeField] private TMP_Dropdown dropDown;

        private void Start()
        {
            dropDown.AddOptions(Enum.GetNames(typeof(SceneEnum)).ToList());
            onPanelOpen += EmptyInputField;
        }

        public void OnClickedLoadButton()
        {
            foreach(var sceneEnum in Enum.GetValues(typeof(SceneEnum)))
            {
                if (sceneEnum.ToString() == dropDown.options[dropDown.value].text)
                {
                    RZSceneManager.Instance.LoadScene((SceneEnum)sceneEnum);
                    return;
                }
            }
        }

        private void EmptyInputField()
        {
            dropDown.ClearOptions();
            dropDown.AddOptions(Enum.GetNames(typeof(SceneEnum)).ToList());
        }
    }

}

