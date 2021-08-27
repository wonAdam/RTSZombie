using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZInputHelper
    {
        public static bool IsKeysDown(List<KeyCode> keys)
        {
            if (keys.Count == 0)
                return false;

            bool keysPressedExceptLast = true;
            for (int i = 0; i < keys.Count - 1; ++i)
                keysPressedExceptLast &= IsKeyPressed(keys[i]);

            if (!keysPressedExceptLast)
                return false;

            if (!Input.GetKeyDown(keys[keys.Count - 1]))
                return false;

            return true;
        }

        public static bool IsKeyPressed(KeyCode key)
        {
            return Input.GetKey(key);
        }

    }
}

