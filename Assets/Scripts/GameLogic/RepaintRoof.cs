using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using Util;

namespace GameLogic
{
    [SelectionBase]
    public class RepaintRoof : MonoBehaviour
    {
        public SpaceTrigger trigger;
        public Rainbow roof;
        public AudioClip repaintSfx;

        [HideInInspector]
        public Color? repaintColor;

        void Awake ()
        {
            trigger.OnIn(PromptToRepaint);
        }

        void PromptToRepaint(Collider collider)
        {
            if (collider.GetComponent<HeroControl>() != null) {
                if (repaintColor.HasValue) {
                    roof.color = repaintColor.Value;
                    Tls.Inst ().PlayAudio (repaintSfx);
                }
            }
        }
    }
}