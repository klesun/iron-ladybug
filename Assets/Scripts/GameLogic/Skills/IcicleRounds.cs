using Assets.Scripts.Interfaces;
using Assets.Scripts.Util.Shorthands;
using GameLogic;
using Interfaces;
using UnityEngine;
using Util;

namespace Assets.Scripts.GameLogic.Skills {
    public class IcicleRounds: ISkillMb {

        public override void Perform(INpcMb caster)
        {
            U.L(Object.FindObjectsOfType<INpcMb>())
                .Flt(npc => NpcUtil.AreEnemies(npc, caster))
                .Srt(npc => (npc.transform.position - caster.transform.position).magnitude)
                .Fst().get = target => {
                    Tls.Inst().Animate(200, 20f, (prog) => {
                        var basePos = caster.transform.position;
                        var tarPos = target.transform.position;
                        var dir = tarPos - basePos;
                        var radius = dir.magnitude;
                    });
                };
        }
    }
}
