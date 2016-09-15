using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Timers;
using System.Collections.Generic;
using GameLogic;
using Interfaces;

namespace Util.GameLogic
{
	public class Dialogue : MonoBehaviour 
	{
		public TextAsset script;
		public INpcMb speakerA = null; // defaults to player
		public INpcMb speakerB;

		private D.Cb sayNext = null;

		void Awake ()
		{
			speakerA = speakerA ?? (Tls.inst ().GetHero ().GetNpc());
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
		public void Say(int i, IList<string> quotes, D.Cb whenDone = null)
		{
			whenDone = whenDone ?? (() => {});

			var speaker = i % 2 == 0 ? speakerA : speakerB;
			if (i < quotes.Count) {
				Sa.Inst().gui.Say (quotes [i], speaker);
				D.Cb cb = () => Say (i + 1, quotes, whenDone);
				var seconds = GetReadingTime (quotes [i]);
				// TODO: it actually does not work when paused - implement 
				// another SetTimeout that would not depend on game time
				sayNext = Tls.inst ().SetTimeout(seconds, cb);
			} else {
				Sa.Inst().gui.EndTalk ();
				whenDone ();
			}
		}

		static float GetReadingTime(string text)
		{
			return Mathf.Max(3, text.Length * 0.15f);
		}
	}
}
