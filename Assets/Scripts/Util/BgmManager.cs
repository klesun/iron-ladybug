using UnityEngine;
using Util.Midi;


namespace Util
{
	public class BgmManager 
	{
		private static BgmManager instance;
		private float lastSwitchTime;
		private MidJsDefinition currentSong = null;
		private Player player = null;
		private D.Cb stopPlayback;

		public BgmManager()
		{
			lastSwitchTime = Time.fixedTime;
			stopPlayback = () => {};
		}

		public static BgmManager Inst()
		{
			return instance 
				?? (instance = new BgmManager());
		}

		public void SetBgm(MidJsDefinition song)
		{
			currentSong = song;
			player = new Player (song);
			stopPlayback = player.Play ();
		}

		public void UnsetBgm(MidJsDefinition song)
		{
			if (currentSong == song) {
				stopPlayback ();
				stopPlayback = null;
				currentSong = null;
			}
		}
	}
}