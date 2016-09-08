using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Linq;

/** 
 * quest about repainting roofs
 */
public class A01MyLife : IQuest
{
	public Dialogue dialogue;
	public SpaceTrigger questGiverSpace;
	public GameObject rewardPrison;

	void Start()
	{
		questGiverSpace.callback = Play;
	}

	void Play(Collider c)
	{
		foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
			var colors = GetRoofs ().Select (r => r.roof.color).Distinct ();
			// TODO: say, how many left and say that it is wrong color if all are of same color
			if (!CheckIsCompleted ()) {
				dialogue.Play (() => {
					dialogue.Say(1, new List<string>{"Какой МОЙ любимый цвет?"});
					Tls.inst ().AskForChoice (new Dictionary<string, Color>{
						{"red", Color.red},
						{"green", Color.green},
						{"blue", Color.blue},
						{"cyan", Color.cyan},
						{"magenta", Color.magenta},
						{"yellow", Color.yellow},
					}, (rgb) => GetRoofs().ForEach(r => r.repaintColor = rgb));
					dialogue.Say(1, new List<string>{"Вот значит что ты обо мне думаешь... Вот тебе краска, когда перекрасишь все крыши - возвращайся, я скажу был ли твой ответ верным."});
				});
			} else {
				LiberateReward ();
			}
		}
	}

	public void LiberateReward()
	{
		dialogue.Say (0, new List<string>{"...", "Отличная работа", "Ещё бы, это же моя работа"}, Tls.inst ().Pause ());
		if (rewardPrison != null) {
			Destroy (rewardPrison);
		}
	}

	override public bool CheckIsCompleted()
	{
		if (GetRoofs ().TrueForAll (r => r.roof.color == Color.cyan)) {
			return true;
		} else {
			return false;
		}
	}
	
	List<RepaintRoof> GetRoofs()
	{
		return GetComponentsInChildren<RepaintRoof> ().ToList ();
	}
}
