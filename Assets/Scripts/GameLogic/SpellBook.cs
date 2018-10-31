using System.Collections.Generic;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Util.Shorthands;
using GameLogic.Destructibles;
using Interfaces;
using UnityEngine;
using Util;

namespace GameLogic {
    public class SpellBook {
        readonly HeroControl hero;
        private readonly string CASTED = "";
        
        public SpellBook(HeroControl hero)
        {
            this.hero = hero;
        }

        private string SayNotImplemented()
        {
            return "Not implemented yet";
        }
        
        private string MegaJump()
        {
            if (!hero.GetNpc().IsGrounded()) {
                return "Need a ground to jump from";
            } else {
                return hero.npc.Jump(NpcControl.JUMP_BOOST * 3)
                    ? CASTED : "Can not jump now";
            }
        }
        
        private string Dash()
        {
            var vector = hero.npc.transform.forward * NpcControl.RUNNING_BOOST * 0.25f;
            return hero.npc.Boost(vector) ? CASTED : "Can not dash now";
        }
        
        private string FireBall()
        {
            return S.Opt(Sa.Inst().fireBallRef).Match(
                (fireBallRef) => {
                    var fireBallGo = Object.Instantiate (fireBallRef.gameObject);
                    fireBallGo.name = "_generated_fireball";
                    fireBallGo.transform.rotation = hero.cameraAngle.transform.rotation;
                    fireBallGo.transform.position = hero.transform.position + Vector3.up * 1.0f + hero.transform.forward * 1.5f;
                    foreach (var fireBall in fireBallGo.GetComponents<FireBall>()) {
                        fireBall.rigidBody.velocity = hero.cameraAngle.transform.forward * 20;
                    }
                    return CASTED;
                }, 
                () => "Fire Ball reference object is not set, say thanks to the developer of this map =3"
            );
        }
        
        public void Open()
        {
            var spellMap = new Dictionary<string, D.Su<string>> {
                {"DoNothing", () => ""},
                {"MegaJump", MegaJump},
                {"Dash", Dash},
                {"Float", SayNotImplemented},
                {"FireBall", FireBall},
                {"Telekinesis", () => "Not Telekinesable"},
            };
                
            Sa.Inst().gui.AskForChoice (spellMap, (cb) => {
                var msg = cb();
                if (msg != CASTED) {
                    Sa.Inst().gui.SayShyly(msg, hero.GetNpc());
                }
            });
        }
    }
}