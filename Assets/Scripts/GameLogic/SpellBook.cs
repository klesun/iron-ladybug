using System.Collections.Generic;
using Assets.Scripts.GameLogic;
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
        
        public void Open()
        {
            var spellMap = new Dictionary<string, D.Su<string>> {
                {"DoNothing", () => ""},
                {"MegaJump", MegaJump},
                {"Dash", Dash},
                {"Float", SayNotImplemented},
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