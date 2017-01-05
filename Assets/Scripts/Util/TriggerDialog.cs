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

        void Awake ()
        {
            trigger.OnIn(c => U.IfOld(c.gameObject.GetComponent<IHero>() != null, () => dialog.Play()));
        }
    }
}
