using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Util.Architecture;
using Assets.Scripts.Util.Logic;
using Util.GameLogic;
using Util;
using Util.Architecture.Animated;

namespace GameLogic.Missions
{
    public class A02LaserRope : IQuest
    {
        public SpaceTrigger questArea;
        public SpaceTrigger overJumpTrigger;
        public NpcControl opponent;
        public FerrisWheel rope;
        public Dialogue dialogue;
        public GameObject rewardPrison;

        private int hits = 0;
        private float initialFreqeuqnce;
        private int highscore = 12;
        private bool isComplete = false;

        void Awake ()
        {
            initialFreqeuqnce = rope.periodDenominator;

            overJumpTrigger.OnIn((c) => {
                if (c.gameObject.GetComponent<HeroControl>() != null) {
                    ++hits;
                    highscore = Mathf.Max(highscore, hits);
                    var msg = "Hits: " + hits + "\n" + "Highscore: " + highscore;
                    Sa.Inst().gui.quoteBoxArray[1].ShowStats(msg, opponent);
                    rope.SetFrequence(rope.periodDenominator * 1.1f);
                }
            });

            questArea.OnIn(Play);

            questArea.OnOut((c) => {
                if (c.gameObject.GetComponent<HeroControl>() != null) {
                    hits = 0;
                    rope.SetFrequence(initialFreqeuqnce);
                    if (!opponent.IsDead) {
                        Sa.Inst().gui.quoteBoxArray[1].ShowStats("Вернись, трус!", opponent);
                    }
                }
            });
        }

        void Play(Collider c)
        {
            if (c.gameObject.GetComponent<HeroControl>() != null) {
                if (!CheckIsCompleted ()) {
                    dialogue.Play ();
                } else {
                    LiberateReward ();
                }
            }
        }

        void Update ()
        {
            CheckIsCompleted ();
        }

        override public bool CheckIsCompleted()
        {
            if (isComplete) {
                return true;
            } else if (hits >= 12) {
                LiberateReward ();
                opponent.Die ();
                return isComplete = true;
            } else {
                return false;
            }
        }

        public void LiberateReward()
        {
            dialogue.Say (0, new List<string>{"...", "Отличная работа", "Ещё бы, это же моя работа"}, Tls.Inst ().Pause ());
            if (rewardPrison != null) {
                Destroy (rewardPrison);
            }
        }
    }
}
