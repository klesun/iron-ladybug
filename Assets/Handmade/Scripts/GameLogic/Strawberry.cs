using UnityEngine;
using System.Collections;

public class Strawberry : MonoBehaviour 
{
	public AudioClip collectedSound;
	public AudioClip collectedEvilSound;
	public SpaceTrigger trigger;

	void Start()
	{
		trigger.callback = OnGrab;
	}

	void OnGrab(Collider collider)
	{
		foreach (var hero in collider.gameObject.GetComponents<HeroControl>()) {
			++hero.stats.strawberryCount;
			var snd = Random.Range (0, 10) == 0
				? collectedEvilSound
				: collectedSound;

			AudioSource.PlayClipAtPoint(snd, transform.position);

			Destroy (transform.parent.gameObject);
		}
	}
}
