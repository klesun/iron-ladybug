using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Util.Logic;
using Util;
using Util.Bgm;
using Util.Midi;

namespace GameLogic
{
    /**
     * this class contains _only_ logic of how
     * ai reacts on circumstances in the world
     * ALL npc logic must be placed into NpcControl (todo: rename)
     */
    public class EnemyLogic : ITrophy
    {
        public SpaceTrigger enemyDetectionRadius;
        public NpcControl npc;
        /** optional */
        public MidJsFile bgm = null;

        private HeroControl enemy = null;
        private D.Cb onDeath = null;

        const float EPEE_LENGTH = 1.5f;

        void Awake ()
        {
            enemyDetectionRadius.OnIn(OnUfo);
        }

        void Update()
        {
            if (npc.IsDead && onDeath != null) {
                onDeath ();
                onDeath = null;
            }
            if (enemy != null && !enemy.npc.IsDead) {
                // TODO: with delay
                npc.Face (enemy.transform.position);

                var enemyDistance = enemy.transform.position - transform.position;
                enemyDistance.y = 0;
                if (npc.CanAttack ()) {
                    var attackDistance = GetAttackDistance (npc.GetVelocity () - enemy.npc.GetVelocity());
                    var chosenSkill = ChooseSkill();
                    if (chosenSkill != null) {
                        npc.UseSkill(chosenSkill);
                    } else if (enemyDistance.magnitude <= attackDistance * 0.75) {
                        // 0.75 cuz it's pretty easy to avoid when he atacks from longest distance
                        if (enemy.npc.IsGrounded ()) {
                            npc.Attack ();
                        }
                    } else {
                        npc.Move (CeilAxis(enemyDistance));
                    }
                } else {
                    var enemyAttackDistance = GetAttackDistance (enemy.npc.GetVelocity());
                    if (enemyAttackDistance - enemyDistance.magnitude > 0 &&
                        enemyAttackDistance - enemyDistance.magnitude < GetBackRoomDistance()
                    ) {
                        npc.Move (-CeilAxis(enemyDistance));
                    }
                }
                npc.anima.SetBool ("isInBattle", true);
            }
        }

        ISkillMb ChooseSkill()
        {
            int n = npc.skills.Count;
            for (int i = 0; i < n; ++i) {
                if (new System.Random().NextDouble() < 1.0 * n) {
                    return npc.skills[i];
                }
            }
            // no skills rolled
            return null;
        }

        float GetBackRoomDistance()
        {
            // TODO: raycast!
            return 0;
        }

        void OnUfo(Collider collider)
        {
            foreach (var hero in collider.gameObject.GetComponents<HeroControl>()) {
                enemy = hero;
                hero.AcquireEnemy (this);
            }
        }

        float GetAttackDistance(Vector3 speed)
        {
            float t = 2 * NpcControl.LOUNGE_HEIGHT / 9.80665f;
            return (speed.magnitude + NpcControl.LOUNGE_LENGTH) * t + EPEE_LENGTH;
        }

        /**
         * transforms every axis of vector to either -1 or 1
         * if it is less or greater than zero accordingly
         * need this to simulate how human would press buttons
        */
        Vector3 CeilAxis(Vector3 v)
        {
            return new Vector3 (
                Mathf.Sign(v.x),
                0, //Mathf.Sign(v.y),
                Mathf.Sign(v.z)
            );
        }

        override public ETrophy GetName()
        {
            return ETrophy.ENEMY;
        }

        public override void SetOnCollected (D.Cb callback)
        {
            onDeath = callback;
        }
    }
}
