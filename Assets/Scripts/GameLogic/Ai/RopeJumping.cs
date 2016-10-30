using UnityEngine;
using System.Collections;

namespace GameLogic.Ai
{
    /**
     * supposing for now that npc stands
     * in the center of the rope and does not move
     *
     * ordering npc to jump to avoid rope
     * selecting the best possible time - when npc
     * spends in air equal time before and after period
     * (note, this time may be insufficient whatsoever =P)
     */
    public class RopeJumping : MonoBehaviour
    {
        public NpcControl npc;
        public FerrisWheel rope;

        void Update ()
        {
            var period = 1.0f / rope.frequence;
            var time = rope.GetOffset() * period;
            var leftTilCenter = 3.0f / 4.0f * period - time;

            if (leftTilCenter > 0 && leftTilCenter < GetExpectedTimeInAir () / 2) {
                npc.Jump ();
            }
        }

        float GetExpectedTimeInAir()
        {
            float t = 2 * NpcControl.JUMP_BOOST / 9.80665f;
            return t;
        }
    }
}