using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using Util.GameLogic;
using Util;

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
			initialFreqeuqnce = rope.frequence;

			overJumpTrigger.callback = (c) => {
				foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
					++hits;
					highscore = Mathf.Max(highscore, hits);
					var msg = "Hits: " + hits + "\n" + "Highscore: " + highscore;
					Sa.Inst().gui.quoteBoxArray[1].ShowStats(msg, opponent);
					rope.SetFrequence(rope.frequence * 1.1f);
				}
			};

			questArea.callback = Play;

			questArea.exitCallback = (c) => {
				foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
					hits = 0;
					rope.SetFrequence(initialFreqeuqnce);
					if (!opponent.IsDead) {
						Sa.Inst().gui.quoteBoxArray[1].ShowStats("Вернись, трус!", opponent);
					}
				}
			};
		}

		void Play(Collider c)
		{
			foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
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
			dialogue.Say (0, new List<string>{"...", "Отличная работа", "Ещё бы, это же моя работа"}, Tls.inst ().Pause ());
			if (rewardPrison != null) {
				Destroy (rewardPrison);
			}
		}
	}
}
