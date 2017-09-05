using System;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;
using Util;

namespace Assets.Scripts.Util.Architecture
{
    /** 
     * this is a button that reuires you stay in a 
     * region for some time to trigger the action 
     */
    public class StayStillButton: MonoBehaviour
    {
        // mandatory
        public SpaceTrigger region;

        // optional
        public float stayDuration = 2.0f;
        public AudioClip pressedSfx = null;
        public Rainbow platformColor = null;
        public Color pressedColor = Color.green;

        public bool wasPressed = false;
        
        void Awake()
        {
            region.onIn = 
                (col) => U.Opt(col.GetComponent<IHeroMb>()).get = 
                (hero) => {
                    wasPressed = true;
                    U.Opt(pressedSfx).get = Tls.Inst().PlayAudio;
                    U.Opt(platformColor).get = 
                        (clr) => clr.color = pressedColor;
                };
            region.onOut = (col) => {};
        }
    }
}