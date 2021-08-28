using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZDebug
    {
        public static bool IsEditorPlay() => Application.platform == RuntimePlatform.WindowsEditor;

        public static void Log(MonoBehaviour requester, string msg) => Debug.Log($"[{requester.name}] {msg}");
    }

}
