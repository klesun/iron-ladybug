using UnityEngine;
using System.Collections;

public class ferrisWheel : MonoBehaviour {

	Vector3 startPosition;
	public float amplY = 5;
	public float amplZ = 5;
	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		var dy = Mathf.Sin(Time.fixedTime * speed) * amplY;
		var dz = Mathf.Cos(Time.fixedTime * speed) * amplZ;
		transform.localPosition = startPosition + transform.forward * dz + new Vector3 (0, dy, 0);
	}
}
