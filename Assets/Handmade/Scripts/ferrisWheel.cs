using UnityEngine;
using System.Collections;

public class FerrisWheel : MonoBehaviour {

	Vector3 startPosition;
	public float amplY = 5;
	public float amplZ = 5;
	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = getLocalPositionAt (Time.fixedTime);
		//transform.GetComponent<Rigidbody>().MovePosition(getLocalPositionAt(Time.fixedTime));
	}

	void OnDrawGizmos() {
		var markCnt = 24;
		for (var i = 0; i < markCnt; ++i) {
			var dPos = getLocalPositionAt (4.0f * Mathf.PI * i / markCnt);
			Gizmos.DrawWireCube (transform.position + dPos, new Vector3(1,1,1));
		}
	}

	Vector3 getLocalPositionAt(float time) {
		var dy = Mathf.Sin(time * speed) * amplY;
		var dz = Mathf.Cos(time * speed) * amplZ;
		return startPosition + transform.forward * dz + new Vector3 (0, dy, 0);
	}
}
