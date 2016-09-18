using UnityEngine;
using System.Collections;
using System;

public class Sequence : MonoBehaviour {

	public float spacing;
	public GameObject originalMesh;
	public Transform point;

	void Awake () 
	{
		PosForObjects ((pos, rot) => UnityEngine.Object.Instantiate(originalMesh, pos, rot));
	}

	delegate void CreateObj (Vector3 pos, Quaternion rot);

	void PosForObjects (CreateObj makeObj) 
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = point.transform.position;
		int objc = (int)(Vector3.Distance (endPos, startPos) / spacing);
		for (float i = 0; i < objc; i++) {
			var drawPos = Vector3.Lerp (startPos, endPos, i / objc);
			makeObj (drawPos, originalMesh.transform.rotation);
		}
	}

	void OnDrawGizmos () {
		PosForObjects ((pos, rot) => Gizmos.DrawWireCube (pos, new Vector3 (1,1,1)));
	}
}
