using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RTSZombie.Dev
{
    public class DevCanvas : MonoBehaviour
    {
        [System.Serializable]
        public class ButtonNamePanelDictionary : SerializableDictionaryBase<string, DevSubPanel> { }

        [SerializeField] private List<KeyCode> devOpenKeys;

        [SerializeField] private ButtonNamePanelDictionary devSubPanelDictionary;

        [SerializeField] private DevPanel devPanelPrefab;

        public static DevCanvas Instance { get; protected set; }

        private DevPanel devPanelInst;

        private List<DevSubPanel> devSubPanels = new List<DevSubPanel>();

        // Start is called before the first frame update
        private void Awake()
        {
            if (!RZDebug.IsEditorPlay())
                Destroy(gameObject);

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

        private void Start()
        {
            devPanelInst = Instantiate(devPanelPrefab, transform);

            foreach(var devSubPanelPair in devSubPanelDictionary)
            {
                string devSubPanelName = devSubPanelPair.Key;
                DevSubPanel devSubPanel = devSubPanelPair.Value;

                var subPanelInst = Instantiate(devSubPanel, transform);
                subPanelInst.devPanel = devPanelInst;
                devSubPanels.Add(subPanelInst);

                DevButton devButton = devPanelInst.AddSubPanelButton(this, devSubPanelName);
                devButton.OnClicked(() => {
                    subPanelInst.OpenPanel();
                    devPanelInst.ClosePanel();
                });

            }

        }

        // Update is called once per frame
        private void Update()
        {
            if (RZInputHelper.IsKeysDown(devOpenKeys))
            {
                OpenToggleDevPanel();
            }
        }

        private void OpenToggleDevPanel()
        {
            if (devSubPanels.Any(panel => panel.isOpened))
            {
                CloseAllSubPanels();
            }
            else
            {
                if (devPanelInst.isOpened)
                {
                    devPanelInst.ClosePanel();
                }
                else
                {
                    devPanelInst.OpenPanel();
                }
            }
        }

        private void CloseAllSubPanels()
        {
            foreach (var openedPanel in devSubPanels.Where(panel => panel.isOpened))
            {
                openedPanel.ClosePanel();
            }
        }

    }


}
