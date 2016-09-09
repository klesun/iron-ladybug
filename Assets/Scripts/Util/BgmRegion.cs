using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Util.Midi;

namespace Util
{
	public class BgmRegion : MonoBehaviour 
	{
		public TextAsset audioFile;
		public SpaceTrigger trigger;

		void Start () 
		{
			var parsedSong = JsonConvert.DeserializeObject<MidJsDefinition> (audioFile.text);
			trigger.callback = (c) => U.If(
				c.GetComponent<HeroControl>() != null, 
				() => BgmManager.Inst ().SetBgm (parsedSong)
			);
			trigger.exitCallback = (c) => U.If(
				c.GetComponent<HeroControl>() != null, 
				() => BgmManager.Inst ().UnsetBgm (parsedSong)
			);
		}
	}
}
