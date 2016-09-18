using System;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Util.SoundFontPlayer;

namespace Util.Midi
{
	public class Playback
	{
		public D.Cb whenDone = () => {};
		public readonly MidJsDefinition song;

		int position = 0;
		Staff staff;
		int loopsLeft;
		float volumeFactor = 1;

		public Playback (MidJsDefinition song)
		{
			this.song = song;
			this.staff = song.staffList[0];
			loopsLeft = this.staff.staffConfig.loopTimes;
		}

		public D.Cb Play()
		{
			var flag = new StopFlag ();

			// timeouting so playback started in the next assync code block 
			// to allow user set some configuration in the next statements
			Tls.Inst().timeout.Real(0.001f, () => PlayNext (flag));

			return () => flag.isInterrupted = true;
		}

		void PlayNext(StopFlag flag)
		{
			if (!flag.isInterrupted) {
				if (position < staff.chordList.Length) {
					var chord = staff.chordList [position++];
					var seconds = PlayChord (chord);
					Tls.Inst ().timeout.Real (seconds, () => PlayNext (flag));
				} else if (loopsLeft > 0) {
					--loopsLeft;
					position = 0; // TODO: use loopStart
					PlayNext (flag);
				} else {
					whenDone ();
				}
			}
		}

		public Playback SetVolumeFactor(float value)
		{
			this.volumeFactor = value;
			return this;
		}

		float PlayChord(Chord chord)
		{
			float? chordLength = null;
			foreach (var note in chord.noteList) {
				var noteLength = ParseFraction (note.length);
				var instr = S.List(staff.staffConfig.channelList)
					.Where(c => c.channelNumber == note.channel)
					.Select(c => c.instrument)
					.FirstOrDefault();

				var isRest = note.channel == 9 && note.tune == 0;
				var stop = !isRest
					? Fluid.Inst ().PlayNote (note.tune, instr, volumeFactor)
					: () => {};
				Tls.Inst().timeout.Real (AcadToSeconds(noteLength), stop);

				chordLength = chordLength != null ? Mathf.Min (chordLength ?? 0, noteLength) : noteLength;
			}

			return AcadToSeconds(chordLength ?? 0);
		}

		float ParseFraction(string fraction)
		{
			var tokens = fraction.Split ('/').Select(t => t.Trim()).ToList();
			var num = float.Parse(tokens[0]);
			var denom = tokens.Count == 2 ? float.Parse(tokens[1]) : 1;
			return num / denom;

		}

		float AcadToSeconds(float length)
		{
			// semibreve takes 1 second when tempo is 240
			return 240f * length / staff.staffConfig.tempo;
		}

		public class StopFlag
		{
			public bool isInterrupted = false;
		}
	}
}

