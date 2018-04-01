using Assets.Scripts.GameLogic;
using Assets.Scripts.Util.Shorthands;
using UnityEngine;
using Util.Shorthands;

namespace GameLogic.Entities {
    public class CheckpointUtil {
        private static CheckpointUtil inst = null;

        private Opt<Vector3> startPoint = Opt<Vector3>.None<Vector3>();

        public static CheckpointUtil Inst()
        {
            return inst ?? (inst = new CheckpointUtil());
        }

        private Vector3 GetNextPoint(HeroControl hero)
        {
            var checkpoints = U.L(Object.FindObjectsOfType<Checkpoint>())
                .Fop(cp => cp.GetSpawnPoint()).s;
            startPoint.get = pt => checkpoints.Add(pt);

            var currentIdx = -1;
            for (var i = 0; i < checkpoints.Count; ++i) {
                var checkpoint = checkpoints[i];
                var distance = (checkpoint - hero.transform.position).magnitude;
                if (distance < 1.0) {
                    currentIdx = i;
                    break;
                }
            }
            if (currentIdx > -1) {
                var nextIdx = (currentIdx + 1) % checkpoints.Count;
                return checkpoints[nextIdx];
            } else {
                startPoint = U.Opt(hero.transform.position);
                return checkpoints.Count > 0
                    ? checkpoints[0]
                    : hero.transform.position;
            }
        }

        public void JumpToNext(HeroControl hero)
        {
            Vector3 nextPoint = GetNextPoint(hero);
            hero.transform.position = nextPoint;
            hero.npc.body.velocity = Vector3.zero;
        }
    }
}
