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
		var speaker = i % 2 == 0 ? speakerA : speakerB;
		if (i < quotes.Length) {
			gui.Say (quotes [i], speaker);
			yield return new WaitForSeconds (GetReadingTime (quotes [i]));
			StartCoroutine (Say (i + 1, quotes));
		} else {
			gui.EndTalk ();
		}
	}

	static float GetReadingTime(string text)
	{
		return text.Length * 0.1f;
	}
}
