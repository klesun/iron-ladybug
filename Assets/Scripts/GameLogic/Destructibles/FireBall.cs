using Assets.Scripts.GameLogic;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;
using Util;

namespace GameLogic.Destructibles {
    /**
     * an instance of Fire Ball game object created by SpellBook.cs
     * it flies in the specified direction and explodes on impact with anything (or on timeout)
     * explosion deals damage to INpc.cs and pushes them off
     */
    public class FireBall: MonoBehaviour {
        public SpaceTrigger impactTrigger;
        public Rigidbody rigidBody;
        public AudioClip explosionSfx; // optional

        void Awake ()
        {
            impactTrigger.OnIn(c => {
                S.Opt(explosionSfx).get = (sfx) => Tls.Inst ().PlayAudio (sfx);
                foreach (var col in Physics.OverlapSphere(transform.position, 2)) {
                    foreach (var npc in col.GetComponents<NpcControl>()) {
                        npc.ChangeHealth(-20);
                    }
                }
                // add explosion animation one of these days...
                Destroy(gameObject);
            });
            Tls.Inst().timeout.Game(15, () => Destroy(gameObject));
        }
    }
}