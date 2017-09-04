using UnityEngine;
using System.Collections;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Util;
using Util.GameLogic;
using Interfaces;

namespace GameLogic
{
    public class TriggerDialog : MonoBehaviour
    {
        public Dialogue dialog;
        public SpaceTrigger trigger;
        public bool isRepeatable = true;

        private int timesTriggered = 0; 

        void Awake ()
        {
            trigger.OnIn(c => 
                U.If(c.gameObject.GetComponent<IHero>() != null).then = () => 
                U.If(timesTriggered++ == 0 || isRepeatable).then = () => 
                dialog.Play());
        }
    }
}
