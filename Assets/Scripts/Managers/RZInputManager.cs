using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZInputManager : SingletonBehaviour<RZInputManager>
    {
        [SerializeField] private Animator stateMachine;

        public List<RZUnit> selectedUnits = new List<RZUnit>();

        protected override void SingletonAwakened()
        {

        }

        protected override void SingletonStarted()
        {

        }

        protected override void SingletonDestroyed()
        {

        }


        public void SelectUnits(List<RZUnit> units)
        {
            selectedUnits.Clear();

            selectedUnits = new List<RZUnit>(units);
        }

        public void SelectUnit(RZUnit unit)
        {
            selectedUnits.Clear();
            selectedUnits.Add(unit);
        }
    }
}
