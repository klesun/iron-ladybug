using Assets.Scripts.Util.Shorthands;
using UnityEngine;
using Util.Shorthands;

namespace GameLogic.Entities {

    /**
     * represents a place player can teleport too if visited
     */
    public class Checkpoint: MonoBehaviour {
        public Transform spawnPoint = null;

        public Opt<Vector3> GetSpawnPoint()
        {
            var opt = U.Opt(spawnPoint);
            return opt.Map(pt => pt.position);
        }
    }
}
