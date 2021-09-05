using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public abstract class SACommand
    {
        public enum CommandType
        {
            Move, Attack, Stop
        }

        public abstract CommandType GetCommandType();
    }

}
