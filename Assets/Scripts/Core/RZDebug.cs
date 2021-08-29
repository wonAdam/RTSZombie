using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZDebug
    {
        public static bool IsEditorPlay() => Application.platform == RuntimePlatform.WindowsEditor;

        public static void Log(MonoBehaviour requester, string msg) => Debug.Log($"[{requester.name}] {msg}");

        public static void DrawRectangle(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight, Vector3 topLeft, Color color, float duration)
        {
            Debug.DrawLine(bottomLeft, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, topRight, color, duration);
            Debug.DrawLine(topRight, topLeft, color, duration);
            Debug.DrawLine(topLeft, bottomLeft, color, duration);
        }
    }

}
