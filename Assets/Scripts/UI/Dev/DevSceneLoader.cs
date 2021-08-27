using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RTSZombie.Dev
{
    public class DevSceneLoader : DevSubPanel
    {
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            onPanelOpen += EmptyInputField;
        }

        public void OnClickedLoadButton()
        {

        }

        private void EmptyInputField()
        {
            inputField.text = "";
        }
    }

}

