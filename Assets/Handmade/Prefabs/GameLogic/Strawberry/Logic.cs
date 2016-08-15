using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour 
{
	public AudioClip collectedSound;
	public AudioClip collectedEvilSound;

	void OnTriggerEnter(Collider hero)
	{
		var snd = Random.Range (0, 10) == 0
			? collectedEvilSound
			: collectedSound;

		AudioSource.PlayClipAtPoint(snd, transform.position);

		Destroy (transform.parent.gameObject);
	}
}
