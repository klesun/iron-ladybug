using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;

namespace Util.Architecture.Actions {
    /** links trigger and action that should happen on that */
    public class ApplyAt: MonoBehaviour {
        public SpaceTrigger trigger;
        public IActionMb action;
        
        private void Awake()
        {
            U.Opt(trigger).get =
                trigger => trigger.onIn =
                col => U.Opt(col.GetComponent<IHeroMb>()).get =
                (hero) => {
                    action.execute();
                };
        }
    }
}