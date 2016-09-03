using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Cockshot : ITrophy, IPiercable
{
	public AudioClip explodingBaloonSound;
	private DCallback onCollected = null;

	public void GetPierced()
	{
		AudioSource.PlayClipAtPoint(explodingBaloonSound, transform.position);
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
