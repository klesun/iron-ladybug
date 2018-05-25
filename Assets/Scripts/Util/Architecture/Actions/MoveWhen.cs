using System;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;

namespace Util.Architecture.Actions {
    /**
     * moves object to defined position when defined event happens
     */
    public class MoveWhen: MonoBehaviour {
        public SpaceTrigger trigger = null;
        public float seconds = 0.5f;
        public int yOffset = 1;
        public int xOffset = 0;
        public int zOffset = 0;

        private bool started = false;

        private void Awake()
        {
            var startPos = transform.position;
            U.Opt(trigger).get =
                trigger => trigger.onIn =
                col => U.Opt(col.GetComponent<IHeroMb>()).get =
                hero => U.If(!started).then =
                () => {
                    started = true;
                    var steps = (int) Math.Max(Math.Round(seconds / 0.0001), 1);
                    Tls.Inst().Animate(steps, seconds, prog => {
                        transform.position = startPos
                            + transform.up * yOffset * prog
                            + transform.forward * zOffset * prog
                            + transform.right * xOffset * prog
                            ;
                    });
                };
        }
    }
}
