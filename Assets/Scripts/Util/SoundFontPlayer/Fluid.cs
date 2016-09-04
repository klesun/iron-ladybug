using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Fluid
	{
		private static Fluid instance;

		public Fluid ()
		{
		}

		public static Fluid Inst()
		{
			return instance 
				?? (instance = new Fluid());
		}

		public DCallback PlayNote(int semitone, int presetN)
		{
			var adapted = Adapter.Get (semitone, presetN);
			return PlaySample (adapted.audioClip, adapted.frequencyFactor);
		}

		private DCallback PlaySample(AudioClip audio, float frequencyFactor)
		{
			var audioSourceEl = new GameObject ("staticAudio", typeof(AudioSource));
			var audioSource = audioSourceEl.GetComponent<AudioSource> ();
			audioSource.clip = audio;
			audioSource.pitch = frequencyFactor;
			audioSource.Play ();

			return () => {
				audioSource.Stop();
				GameObject.Destroy(audioSourceEl);
			};
		}
	}
}

