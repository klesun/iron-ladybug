using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;

namespace Util.Architecture {
    public class ColorOnStep: MonoBehaviour {
        public SpaceTrigger trigger;
        public Rainbow rainbow;
        public Color color = new Color(197, 13, 67);

        void Awake()
        {
            U.Opt(trigger).get = trigger =>
            U.Opt(rainbow).get = rainbow => {
                Color initialColor = rainbow.color;
                trigger.onIn =
                    (col) => U.Opt(col.GetComponent<IHeroMb>()).get =
                    (hero) => rainbow.color = color;
                trigger.onOut =
                    (col) => U.Opt(col.GetComponent<IHeroMb>()).get =
                    (hero) => rainbow.color = initialColor;
            };
        }
    }
}
