using UnityEngine;
using System.Collections;
using System;

public class Sequence : MonoBehaviour {

	public float spacing;
	public GameObject originalMesh;
	public Transform point;

	private Transform originalTransform;

	void Start () 
	{
		PosForObjects ((pos, rot) => UnityEngine.Object.Instantiate(originalMesh, pos, rot));
		originalTransform = originalMesh.transform;
		try {
			//Hide (originalMesh);
		} catch (Exception exc) {
			// if it is prefab, it can't be deleted
		}
	}

	delegate void CreateObj (Vector3 pos, Quaternion rot);

	void PosForObjects (CreateObj makeObj) {
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

	void Hide(GameObject g)
	{
		g.transform.position = new Vector3 (9999,9999,9999);
	}
}
