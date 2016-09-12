using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Util;
using Interfaces;

namespace GameLogic
{
	public class Cockshot : ITrophy, IPiercable
	{
		public AudioClip explodingBaloonSound;
		private DCallback onCollected = null;

		public void GetPierced()
		{
			Tls.inst ().PlayAudio (explodingBaloonSound);
			if (onCollected != null) {
				onCollected ();
			}
			Destroy(gameObject);
		}

		public override ETrophy GetName ()
		{
			return ETrophy.COCKSHOT;
		}

		public override void SetOnCollected (DCallback callback)
		{
			onCollected = callback;
		}
	}
}