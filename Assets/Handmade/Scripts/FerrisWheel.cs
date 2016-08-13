using UnityEngine;
using System.Collections;

public class FerrisWheel : MonoBehaviour {

	Vector3 startPosition;
	public float amplY = 5;
	public float amplZ = 5;
	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = getLocalPositionAt (Time.fixedTime * speed);
		//transform.GetComponent<Rigidbody>().MovePosition(getLocalPositionAt(Time.fixedTime));
	}

	void OnDrawGizmos() {
		var markCnt = 12;
		for (var i = 0; i < markCnt; ++i) {
			var dPos = getLocalPositionAt (2 * Mathf.PI * i / markCnt);
			Gizmos.DrawWireSphere (transform.position + dPos, 0.1f);
		}
	}

	Vector3 getLocalPositionAt(float radians) {
		var dy = Mathf.Sin(radians) * amplY;
		var dz = Mathf.Cos(radians) * amplZ;
		return startPosition + transform.forward * dz + new Vector3 (0, dy, 0);
	}
}
