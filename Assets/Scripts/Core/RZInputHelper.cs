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

            foreach (var key in keys)
                if (!Input.GetKeyDown(key))
                    return false;

            return true;
        }

        public static bool IsKeysPressed(List<KeyCode> keys)
        {
            if (keys.Count == 0)
                return false;

            foreach (var key in keys)
                if (!Input.GetKey(key))
                    return false;

            return true;
        }

        public static bool IsKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public static bool IsKeyPressed(KeyCode key)
        {
            return Input.GetKey(key);
        }
    }
}

