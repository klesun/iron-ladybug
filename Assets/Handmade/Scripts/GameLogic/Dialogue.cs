using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour 
{
	public TextAsset script;
	public NpcControl speakerA;
	public NpcControl speakerB;
	public GuiControl gui;
	public SpaceTrigger trigger;

	// Use this for initialization
	void Start () 
	{
		trigger.callback = Play;
	}

	void Play(Collider c)
	{
		foreach (var hero in c.gameObject.GetComponents<HeroControl>()) {
			StartCoroutine(Say(0, script.text.Split('\n')));
		}
	}

	IEnumerator Say(int i, string[] quotes)
	{
		var textfield = i % 2 == 0 ? gui.textfieldA : gui.textfieldB;
		if (i < quotes.Length) {
			textfield.text = quotes[i];
			yield return new WaitForSeconds (GetReadingTime(quotes[i]));
			StartCoroutine(Say(i + 1, quotes));
		}
	}

	static float GetReadingTime(string text)
	{
		return text.Length * 0.1f;
	}
}
