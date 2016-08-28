using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuoteBox : MonoBehaviour 
{
	public RawImage icon;
	public Text textfield;

	private float? hideAt = null;

	private NpcControl npc = null;

	void Start () 
	{
		gameObject.SetActive (false);
	}

	void Update()
	{
		if (hideAt != null && hideAt < Time.fixedTime) {
			hideAt = null;
			RemoveSpeaker ();
		}
	}

	public void Say(string quote, NpcControl npc)
	{
		gameObject.SetActive (true);
		this.npc = npc;
		icon.texture = npc.icon;
		textfield.text = quote;
	}

	public void RemoveSpeaker()
	{
		gameObject.SetActive (false);
		npc = null;
		icon.texture = null;
		textfield.text = "";
	}

	public void ShowStats(HeroStats stats, NpcControl npc)
	{
		if (this.npc == null || this.npc == npc) {
			gameObject.SetActive (true);
			this.npc = npc;
			icon.texture = npc.icon;
			textfield.text = "";
			foreach (var entry in stats.trophyCounts) {
				var total = stats.trophyTotalCounts [entry.Key];
				textfield.text += entry.Key + ": " + entry.Value + "/" + total + "\n";
			}

			hideAt = Time.fixedTime + 3;
		}
	}
}
