using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class A02LaserRope : IQuest
{
	public SpaceTrigger questArea;
	public SpaceTrigger overJumpTrigger;
	public NpcControl opponent;
	public QuoteBox opponentIcon;
	public FerrisWheel rope;
	public Dialogue dialogue;

	private int hits = 0;
	private float initialFreqeuqnce;
	private int highscore = 12;
	private bool isComplete = false;
	
	void Start () 
	{
		initialFreqeuqnce = rope.frequence;

		overJumpTrigger.callback = (c) => {
			foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
				++hits;
				highscore = Mathf.Max(highscore, hits);
				opponentIcon.ShowStats("Hits: " + hits + "\n" + "Highscore: " + highscore, opponent);
				rope.SetFrequence(rope.frequence * 1.1f);
			}
		};

		questArea.exitCallback = (c) => {
			foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
				hits = 0;
				rope.SetFrequence(initialFreqeuqnce);
				if (!opponent.IsDead) {
					opponentIcon.ShowStats("Вернись, трус!", opponent);
				}
			}
		};
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
			dialogue.LiberateReward ();
			opponent.Die ();
			return isComplete = true;
		} else {
			return false;
		}
	}
}
