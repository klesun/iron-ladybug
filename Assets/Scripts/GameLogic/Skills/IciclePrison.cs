using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Util.Shorthands;
using GameLogic;
using Interfaces;
using UnityEngine;
using Util;
using Util.Shorthands;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
        public AudioClip pierceSfx = null;
        public AudioClip decaySfx = null;
        public Transform ground = null;

        private Opt<Vector3> GetGround(INpcMb target)
        {
            var tarPos = target.transform.position;
            var groundY = U.Opt(ground).Map(g => g.position.y);
            if (!groundY.Has()) {
                if (target.IsGrounded()) {
                    groundY = U.Opt(tarPos).Map(p => p.y);
                } else {
                    RaycastHit floor;
                    var didHit = Physics.Raycast (
                        tarPos, -Vector3.up * 0.1f, out floor, 100f,
                        layerMask: -5, queryTriggerInteraction: QueryTriggerInteraction.Ignore
                    );
                    if (didHit) {
                        groundY = U.Opt(floor.point).Map(p => p.y);
                    }
                }
            }
            return groundY.Map(y => new Vector3(tarPos.x, y, tarPos.z));
        }

        private static float GetHeight(GameObject obj)
        {
            return 15;
            // following gives astronomic values
//            var bounds = new Bounds {size = Vector3.zero};
//            var colliders = obj.GetComponentsInChildren<Collider>();
//            foreach (Collider col in colliders) {
//                bounds.Encapsulate(col.bounds);
//            }
//            return bounds.size.y;
        }

        /** create a cricle of vertical icicles around unit */
        private Tls.Animated Imprison(Vector3 basePos)
        {
            var offset = (float)(barCount * new System.Random().NextDouble());
            return Tls.Inst().Animate(barCount, barCount * nextBarDelay, (prog, i) => {
                var dx = Mathf.Sin (2 * Mathf.PI * (offset + i) / barCount) * radius;
                var dz = Mathf.Cos (2 * Mathf.PI * (offset + i) / barCount) * radius;
                var drawPos = basePos + Vector3.right * dx + Vector3.forward * dz;
                var height = GetHeight(icicleRef);
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

        private Vector3 degToVec(float degrees)
        {
            var radians = degrees * (Mathf.PI / 180);
            return new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        }

        private bool StillInPrison(INpcMb target, Vector3 basePos)
        {
            var basePosV2 = new Vector2(basePos.x, basePos.z);
            var curPos = target.transform.position;
            var curPosV2 = new Vector2(curPos.x, curPos.z);
            return (basePosV2 - curPosV2).magnitude < radius;
        }

        /** start launching horizontal icicles in the target's direction from outside */
        private void PierceInsides(Vector3 basePos, INpcMb target)
        {
            var pierceCnt = 10;
            var pierceTime = 1.5f;

            var height = GetHeight(icicleRef);
            Tls.Inst().Animate(pierceCnt, pierceCnt * ironMaidDelay, (totalProg) => {
                if (!StillInPrison(target, basePos)) return;

                var headShotY = 0.75f;
                var headPos = target.transform.position + Vector3.up * headShotY;
                var spawnDeg = Random.Range(0, 359);
                var spawnPos = new Vector3(basePos.x, headPos.y, basePos.z) + degToVec(spawnDeg) * (radius + height);
                var pierce = Object.Instantiate(icicleRef, spawnPos, Quaternion.identity);
                pierce.transform.LookAt(headPos);
                pierce.transform.Rotate(90, 0, 0);
                var startPos = pierce.transform.position;
                Tls.Inst().Timeout(2.0f).game = () => {
                    pierce.transform.LookAt(headPos);
                    pierce.transform.Rotate(90, 0, 0);
                    var dir = pierce.transform.up;
                    var soundPos = Vector3.Lerp(startPos, target.transform.position, 0.9f);
                    U.Opt(pierceSfx).get = sfx => AudioSource.PlayClipAtPoint(sfx, soundPos, 1.0f);
                    Tls.Inst().Animate(40, pierceTime, pierceProg => {
                        pierce.transform.position = startPos + dir * (radius * 2) * pierceProg;
                    }).thn = () => Tls.Inst().Timeout(10.0f).game =
                        () => Object.Destroy(pierce);
                };
            });
        }

        public override void Perform(INpcMb caster)
        {
            U.L(Object.FindObjectsOfType<INpcMb>())
                .Flt(npc => NpcUtil.AreEnemies(npc, caster))
                .Srt(npc => (npc.transform.position - caster.transform.position).magnitude)
                .Fst().get =
                    target => GetGround(target).get =
                    basePos => Imprison(basePos).thn =
                    () => U.If(StillInPrison(target, basePos)).then =
                    () => PierceInsides(basePos, target);
        }
    }
}
