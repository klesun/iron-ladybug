using UnityEngine;
using System.Collections;
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
            trigger.OnIn(c => U.If(c.gameObject.GetComponent<IHero>() != null, () => dialog.Play()));
        }
    }
}
