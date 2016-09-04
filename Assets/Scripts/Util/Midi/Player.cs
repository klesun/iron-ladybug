using System;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Util.Midi
{
	public class Player
	{
		int position = 0;
		Staff song;
		int loopsLeft;

		public Player (MidJsDefinition song)
		{
			this.song = song.staffList[0];
			loopsLeft = this.song.staffConfig.loopTimes;
		}

		public void Play()
		{
			if (position < song.chordList.Length) {
				var chord = song.chordList[position++];
				var seconds = PlayChord (chord);
				Tls.inst ().SetTimeout (seconds, Play);
			} else if (loopsLeft > 0) {
				--loopsLeft;
				position = 0; // TODO: use loopStart
				Play ();
			}
		}

		float PlayChord(Chord chord)
		{
			float? chordLength = null;
			foreach (var note in chord.noteList) {
				var noteLength = ParseFraction (note.length);
				var instr = S.List(song.staffConfig.channelList)
					.Where(c => c.channelNumber == note.channel)
					.Select(c => c.instrument)
					.FirstOrDefault();

				var stop = Fluid.Inst ().PlayNote (note.tune, instr);
				Tls.inst ().SetTimeout (AcadToSeconds(noteLength), stop);

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
			// semibreve takes 1 second when tempo is 60
			return length * song.staffConfig.tempo / 60f;
		}
	}
}

