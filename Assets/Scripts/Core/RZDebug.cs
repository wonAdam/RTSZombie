using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZDebug
    {
        public static bool IsEditorPlay() => Application.platform == RuntimePlatform.WindowsEditor;
    }

}
