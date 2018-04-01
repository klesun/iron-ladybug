using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Util.Shorthands;
using GameLogic;
using Interfaces;
using UnityEngine;
using Util;
using Object = UnityEngine.Object;

namespace Assets.Scripts.GameLogic.Skills
{
    /**
     * Caster creates a visible ring around the target.
     * After a second delay, icicles pop out from the ground
     * imprisioning target inside the ring if it did not escape
     *
     * After that, another icicles, horizontal, apear in front of the
     * prison directed at it. Target has to either jump the moment they
     * pierce the prison or to find a spot where they won't touch him
     */
    public class IciclePrison: ISkillMb
    {
        // mandatory
        public GameObject icicleRef;

        // optional
        public float radius = 6.0f;
        public float prisonDelay = 1.0f;
        public float prisonPopDuration = 0.25f;
        public float ironMaidDelay = 2.0f;
        public float nextBarDelay = 0.05f;
        public float decayDelay = 8.0f;
        public int barCount = 64;
        public AudioClip icicleSfx = null;
        public AudioClip decaySfx = null;

        enum E_SIDE {PLAYER, ENEMIES, NEUTRAL}

        private static E_SIDE GetSide(INpcMb npc)
        {
            if (npc.GetComponent<HeroControl>() != null) {
                return E_SIDE.PLAYER;
            } else if (npc.GetComponent<EnemyLogic>() != null) {
                return E_SIDE.ENEMIES;
            } else {
                return E_SIDE.NEUTRAL;
            }
        }

        private static bool AreEnemies(INpcMb a, INpcMb b)
        {
            var sideA = GetSide(a);
            var sideB = GetSide(b);
            return sideA != E_SIDE.NEUTRAL
                && sideB != E_SIDE.NEUTRAL
                && sideA != sideB;
        }

        private static float GetHeight(GameObject obj)
        {
            var bounds = new Bounds {size = Vector3.zero};
            var colliders = obj.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders) {
                bounds.Encapsulate(col.bounds);
            }
            return bounds.size.y;
        }

        private void Imprison(INpcMb target)
        {
            var basePos = target.GetGround().Def(target.transform.position);
            var offset = (float)(barCount * new System.Random().NextDouble());
            for (var i = 0; i < barCount; ++i) {
                var dx = Mathf.Sin (2 * Mathf.PI * (offset + i) / barCount) * radius;
                var dz = Mathf.Cos (2 * Mathf.PI * (offset + i) / barCount) * radius;
                var drawPos = basePos + Vector3.right * dx + Vector3.forward * dz;
                var currentI = i; // against "modified closure"
                Tls.Inst().SetGameTimeout(i * nextBarDelay, () => {
                    //var height = GetHeight(icicleRef);
                    var height = 15;
                    var bar = Object.Instantiate(icicleRef, drawPos, Quaternion.identity);
                    U.Opt(icicleSfx).get = sfx => AudioSource.PlayClipAtPoint(sfx, drawPos, 0.5f);
                    Tls.Inst().Animate(20, prisonPopDuration, progress => {
                        if (bar == null) return;
                        bar.transform.position = drawPos - Vector3.up * height * (1 - progress);
                    });
                    Tls.Inst().SetGameTimeout(decayDelay, () => {
                        Object.Destroy(bar);
                        U.Opt(decaySfx).get = sfx => AudioSource.PlayClipAtPoint(sfx, drawPos, 1.0f);
                    });
                });
            }
        }

        public override void Perform(INpcMb caster)
        {
            U.L(Object.FindObjectsOfType<INpcMb>())
                .Flt(npc => AreEnemies(npc, caster))
                .Srt(npc => (npc.transform.position - caster.transform.position).magnitude)
                .Fst().get = Imprison;
        }
    }
}
