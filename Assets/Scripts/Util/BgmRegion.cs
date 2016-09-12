using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Util.Midi;
using GameLogic;
using Interfaces;

namespace Util
{
	/** 
	 * TODO: move to GameLogic namespace or use some other 
	 * way do determine that game object is player's npc 
	*/
	public class BgmRegion : MonoBehaviour 
	{
		public TextAsset audioFile;
		public SpaceTrigger trigger;

		void Awake () 
		{
			var text = audioFile.text;
			var parsedSong = JsonConvert.DeserializeObject<MidJsDefinition> (text);
			trigger.callback = (c) => U.If(
				c.GetComponent<IHero>() != null, 
				() => BgmManager.Inst ().SetBgm (parsedSong)
			);
			trigger.exitCallback = (c) => U.If(
				c.GetComponent<IHero>() != null, 
				() => BgmManager.Inst ().UnsetBgm (parsedSong)
			);
		}
	}
}
