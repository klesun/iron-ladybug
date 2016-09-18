using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Util;
using System;

namespace GameLogic
{
	/** 
	 * shorthand for accessing text and 
	 * buttons shown to user on top of camera
	 * TODO: rename to just Gui
	 */
	public class GuiControl : MonoBehaviour 
	{
		[Serializable]
		public struct Kv {
			public string k;
			public Sprite v;
		}

		public QuoteBox[] quoteBoxArray;
		public Dropdown generalDropdown;
		public Kv[] colorSpriteTuples;
		private Dictionary<string, Sprite> colorSprites = new Dictionary<string, Sprite>();

		private Queue<QuoteBox> quoteBoxes;

		void Awake () 
		{
			quoteBoxes = new Queue<QuoteBox> (quoteBoxArray);
			foreach (var pair in colorSpriteTuples) {
				colorSprites[pair.k] = pair.v;
			}
		}

		public void Say(string quote, INpc speaker)
		{
			var qb = quoteBoxes.Dequeue ();
			qb.Say (quote, speaker);
			quoteBoxes.Enqueue (qb);
		}

		public void EndTalk()
		{
			quoteBoxes.ToList ().ForEach ((qb) => qb.RemoveSpeaker());
		}

		public void AskForChoice<T>(IDictionary<string, T> options, D.Cu<T> cb)
		{
			var unpause = Tls.Inst().Pause ();
			generalDropdown.options.Clear ();
			generalDropdown.options.Add (new Dropdown.OptionData("none"));
			generalDropdown.AddOptions (S.List(options.Keys).Select (k => MkDdOpt(k)).ToList());

			generalDropdown.gameObject.SetActive (true);
			var tmp = generalDropdown.onValueChanged;
			generalDropdown.value = 0;
			generalDropdown.onValueChanged = tmp;
			generalDropdown.Hide ();

			generalDropdown.onValueChanged.AddListener ((i) => {
				generalDropdown.gameObject.SetActive(false);
				unpause();
				cb(options[generalDropdown.captionText.text]);
				generalDropdown.onValueChanged.RemoveAllListeners();
			});
		}

		/** "MakeDropDownOption" */
		Dropdown.OptionData MkDdOpt(string text)
		{
			var opt = new Dropdown.OptionData (text);

			if (colorSprites.ContainsKey(text)) {
				opt.image = colorSprites [text];
			}

			return opt;
		}
	}
}
