using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RTSZombie.Dev
{
    public class DevButton : MonoBehaviour
    {
        [HideInInspector] public DevSubPanel panelToOpen;

        public void SetButtonText(string text)
        {
            GetComponent<Button>().transform.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }

        public void OnClicked(UnityAction onClicked)
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(onClicked);
        }
    }


}
