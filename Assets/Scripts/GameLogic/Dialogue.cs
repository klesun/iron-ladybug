using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Timers;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour 
{
	public TextAsset script;
	public NpcControl speakerA;
	public NpcControl speakerB;
	public GuiControl gui;

	private DCallback sayNext = null;

	void Start () 
	{
//		trigger.callback = Play;
	}

	void Update()
	{
		var skipButtonClicked = 
			Input.GetKeyDown (KeyCode.Mouse0) ||
			Input.GetKeyDown (KeyCode.Space);

		if (sayNext != null && skipButtonClicked) {
			var tmp = sayNext;
			sayNext = null;
			tmp ();
		}
	}

	public void Play(DCallback cb = null)
	{
		cb = cb ?? (() => {});

		var unpause = Tls.inst ().Pause ();
		Say (0, script.text.Split ('\n'), () => { 
			unpause();
			cb();
		});
	}

	/** TODO: rename to SayCustom() */
	public void Say(int i, IList<string> quotes, DCallback whenDone = null)
	{
		whenDone = whenDone ?? (() => {});

		var speaker = i % 2 == 0 ? speakerA : speakerB;
		if (i < quotes.Count) {
			gui.Say (quotes [i], speaker);
			DCallback cb = () => Say (i + 1, quotes, whenDone);
			var seconds = GetReadingTime (quotes [i]);
			sayNext = Tls.inst ().SetTimeout(seconds, cb);
		} else {
			gui.EndTalk ();
			whenDone ();
		}
	}

	static float GetReadingTime(string text)
	{
		return text.Length * 0.15f;
	}
}
