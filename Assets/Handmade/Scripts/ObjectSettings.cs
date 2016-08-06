using UnityEngine;
using System.Collections;

public class ObjectSettings : MonoBehaviour {

	public float spacing;
	public GameObject originalMesh;
	public Transform point;

	void Start () {
		PosForObjects ((pos, rot) => Object.Instantiate(originalMesh, pos, rot));
	}

	delegate void CreateObj (Vector3 pos, Quaternion rot);

	void PosForObjects (CreateObj makeObj) {
		Vector3 startPos = transform.position;
		Vector3 endPos = point.transform.position;
		int objc = (int)(Vector3.Distance (endPos, startPos) / spacing);
		for (float i = 0; i < objc; i++) {
			var drawPos = Vector3.Lerp (startPos, endPos, i / objc);
			makeObj (drawPos, new Quaternion (0,0,0,0));
		}

	}

	void OnDrawGizmos () {
		PosForObjects ((pos, rot) => Gizmos.DrawWireCube (pos, new Vector3 (1,1,1)));
	}

}
