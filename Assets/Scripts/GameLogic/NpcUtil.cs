using Assets.Scripts.GameLogic;
using Interfaces;

namespace GameLogic {
    /** provides frequently used functions to do stuff with units */
    public class NpcUtil {

        private enum E_SIDE {PLAYER, ENEMIES, NEUTRAL}

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

        public static bool AreEnemies(INpcMb a, INpcMb b)
        {
            var sideA = GetSide(a);
            var sideB = GetSide(b);
            return sideA != E_SIDE.NEUTRAL
                   && sideB != E_SIDE.NEUTRAL
                   && sideA != sideB;
        }
    }
}
