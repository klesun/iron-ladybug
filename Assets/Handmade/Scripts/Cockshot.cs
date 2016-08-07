using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Cockshot : MonoBehaviour, IPiercable
{
	public AudioClip explodingBaloonSound;

	public void pierce()
	{
		AudioSource.PlayClipAtPoint(explodingBaloonSound, transform.position);
		Destroy(gameObject);
	}
}
