using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;

namespace Util.Architecture {
    public class PlatformButton: MonoBehaviour {
        public SpaceTrigger region;
        
        void Awake()
        {
            var startPos = transform.position;
            S.Opt(region).get = region => {
                region.onIn =
                    (col) => U.Opt(col.GetComponent<IHeroMb>()).get =
                    (hero) => {
                        transform.position = startPos + Vector3.down * 0.5f;
                    };
                region.onOut =
                    (col) => U.Opt(col.GetComponent<IHeroMb>()).get =
                    (hero) => {
                        transform.position = startPos;
                    };
            };
        }
    }
}