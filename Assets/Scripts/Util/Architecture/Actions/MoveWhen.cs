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
                    Tls.Inst().Animate(100, seconds, prog => {
                        transform.position = startPos + Vector3.up * yOffset * prog;
                    });
                };
        }
    }
}
