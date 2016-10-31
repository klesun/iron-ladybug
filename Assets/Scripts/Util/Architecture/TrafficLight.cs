using Interfaces;
using UnityEngine;
using Util;

namespace Assets.Scripts.Util.Architecture
{
    public class TrafficLight : IInOutTriggerMb
    {
        public Rainbow red;
        public Rainbow yellow;
        public Rainbow green;

        public float greenNumerator = 3;
        public float yellowNumerator = 1;
        public float redNumerator = 3;

        private D.Cb onIn = () => {};
        private D.Cb onOut = () => {};
        private TrafficLightBlock lastStoodBlock = null;

        void Update ()
        {
            Darken (yellow, 0.2f);
            var total = greenNumerator + yellowNumerator + redNumerator + yellowNumerator;

            // red/yellow/green = 2/1/2
            var offset = 1.0f * Bgm.Bgm.Inst().GetSongCurrentTime() % total;
            if (offset / total < greenNumerator / total) {
                if (green.color != Color.green) {
                    green.color = Color.green;
                    onOut ();
                    Darken (red, 0.2f);
                }
            } else if (offset / total < (redNumerator + yellowNumerator) / total ||
                       offset / total > (total - yellowNumerator) / total
            ) {
                yellow.color = Color.yellow;
                Darken (red, 0.2f);
                Darken (green, 0.2f);
            } else {
                if (red.color != Color.red) {
                    red.color = Color.red;
                    onIn ();
                    Darken (green, 0.2f);
                }
            }
        }

        static void Darken(Rainbow rainbow, float lightFactor)
        {
            var newColor = new Color (
                Mathf.Min(rainbow.color.r, lightFactor),
                Mathf.Min(rainbow.color.g, lightFactor),
                Mathf.Min(rainbow.color.b, lightFactor)
            );
            if (rainbow.color != newColor) {
                rainbow.color = newColor;
            }
        }

        public override void OnIn(D.Cb callback)
        {
            onIn = callback;
        }

        public override void OnOut(D.Cb callback)
        {
            onOut = callback;
        }

        public void ReportInteraction(TrafficLightBlock block, INpc npc)
        {
            if (green.color == Color.green) {
                if (lastStoodBlock != null) {
                    lastStoodBlock.colorController.color = Color.gray;
                }
                block.colorController.color = Color.green;
                lastStoodBlock = block;
            } else if (red.color == Color.red) {
                // kill hero if not last stood block
                if (lastStoodBlock != block) {
                    npc.Die();
                }
            } else {
                // TODO: when player stands on yellow, he should die when it becomes red
            }
        }

        public Color GetColor(TrafficLightBlock block)
        {
            if (block == lastStoodBlock) {
                return block.colorController.color;
            } else if (green.color == Color.green) {
                return Color.green;
            } else if (yellow.color == Color.yellow) {
                return Color.yellow;
            } else if (red.color == Color.red) {
                return Color.red;
            } else {
                return Color.gray;
            }
        }
    }
}