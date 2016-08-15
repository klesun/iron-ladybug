using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Cockshot : MonoBehaviour, IPiercable
{
	public AudioClip explodingBaloonSound;

	public void GetPierced()
	{
		AudioSource.PlayClipAtPoint(explodingBaloonSound, transform.position);
		Destroy(gameObject);
	}
}
