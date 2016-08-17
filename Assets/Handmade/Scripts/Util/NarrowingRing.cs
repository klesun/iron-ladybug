using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NarrowingRing : MonoBehaviour 
{
	public GameObject vaneReference;
	public int vaneCount = 24;
	public float amplitude = 1;

	private List<GameObject> vanes;

	// Use this for initialization
	void Start () 
	{
		PlaceVanes ((pos, rot) => Object.Instantiate(vaneReference, pos, rot));
		Destroy (vaneReference);
	}

	void OnDrawGizmos () {
		PlaceVanes ((pos, rot) => Gizmos.DrawWireCube (pos, new Vector3 (1,1,1)));
	}

	delegate void DPlaceTaker (Vector3 pos, Quaternion rot);

	void PlaceVanes (DPlaceTaker makeObj) 
	{
		for (float i = 0; i < vaneCount; ++i) {
			var drawPos = transform.position;// + transform.forward * dz + new Vector3 (0, dy, 0);
			makeObj (drawPos, transform.rotation * Quaternion.Euler(360.0f * i / vaneCount, 0, 15));
		}
	}
}
