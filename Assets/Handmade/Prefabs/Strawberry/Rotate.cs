using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{
	Vector3 startPosition;

	void Start()
	{
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		var posKoef = Mathf.Sin (Time.fixedTime) * 0.3f;

		//transform.Rotate (0, 0.5f, 0);
		transform.position = startPosition + new Vector3 (0, 1, 0) * posKoef;
	}
}
