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
			textfield.text = 
				"Strawberries: " + stats.strawberryCount + "/" + stats.totalStrawberryCount + "\n" +
				"Cockshots: " + stats.cockshotCount + "/" + stats.totalCockshotCount + "\n" +
				"Enemies: " + stats.enemyCount + "/" + stats.totalEnemyCount + "\n";

			hideAt = Time.fixedTime + 3;
		}
	}
}
