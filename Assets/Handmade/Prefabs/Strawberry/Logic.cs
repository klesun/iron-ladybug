using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour 
{
	public AudioSource collectedSound;

	void OnCollisionEnter(Collision collision)
	{
		collectedSound.Play ();
		Destroy (transform.parent.gameObject);
	}
}
