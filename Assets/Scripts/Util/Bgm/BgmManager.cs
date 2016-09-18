using UnityEngine;
using Util.Midi;
using System.Collections.Generic;
using Util.Shorthands;
using System.Linq;


namespace Util
{
	public class BgmManager 
	{
		private static BgmManager instance;
		private D.Cb stopPlayback = () => {};

		private L<Playback> pendingSongs;

		public BgmManager()
		{
			pendingSongs = new L<Playback> ();
		}

		public static BgmManager Inst()
		{
			return instance 
				?? (instance = new BgmManager());
		}

		public Playback SetBgm(MidJsDefinition song)
		{
			if (song != pendingSongs.Last ().Map (p => p.song).Or (null)) {
				var player = new Playback (song); 
//				{
//					whenDone = () => U.If(!interrupted, () => UnsetBgm(song)),
//				};
				pendingSongs.s.Add (player);
				Play ();
				return player;
			} else {
				return pendingSongs.Last ().Unwrap ();
			}
		}

		public void UnsetBgm(MidJsDefinition song)
		{
			if (song == pendingSongs.Last ().Map (p => p.song).Or (null)) {
				pendingSongs.Pop ();
				Play ();
			} else {
				var idx = pendingSongs.s.FindLastIndex (ps => ps.song == song);
				if (idx > -1) {
					pendingSongs.s.RemoveAt (idx);
				}
			}
		}

		void Play()
		{
			stopPlayback ();
			stopPlayback = () => {};
			pendingSongs.Last ().If (player => {
				var interrupted = false;
				player.whenDone = () => U.If(!interrupted, () => UnsetBgm(player.song));
				var stopPlaybackTmp = player.Play ();
				stopPlayback = () => {
					stopPlaybackTmp();
					interrupted = true;
				};
			});
		}
	}
}