using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZDebug
    {
        public static bool IsEditorPlay() => Application.platform == RuntimePlatform.WindowsEditor;

        public static void Log(MonoBehaviour requester, string msg) => Debug.Log($"[{requester.name}] {msg}");

        public static void Log(Task requester, string msg) => Debug.Log($"[{requester.ToString()}] {msg}");

        public static void Log(ScriptableObject requester, string msg) => Debug.Log($"[{requester.name}] {msg}");

        public static void LogError(MonoBehaviour requester, string msg) => Debug.LogError($"[{requester.name}] {msg}");

        public static void LogError(ScriptableObject requester, string msg) => Debug.LogError($"[{requester.name}] {msg}");

        public static void LogWarning(MonoBehaviour requester, string msg) => Debug.LogWarning($"[{requester.name}] {msg}");

        public static void LogWarning(ScriptableObject requester, string msg) => Debug.LogWarning($"[{requester.name}] {msg}");

        public static void DrawRectangle(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight, Vector3 topLeft, Color color, float duration)
        {
            Debug.DrawLine(bottomLeft, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, topRight, color, duration);
            Debug.DrawLine(topRight, topLeft, color, duration);
            Debug.DrawLine(topLeft, bottomLeft, color, duration);
        }
    }

}
