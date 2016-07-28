using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour 
{
	public AudioSource collectedSound;

	// Use this for initialization
	void Start () 
	{
		print ("strawberry initialized!");
	}

	void OnCollisionEnter(Collision collision)
	{
		print ("strawberry collected!");
		collectedSound.Play ();
		Destroy (transform.parent.gameObject);
	}
}
