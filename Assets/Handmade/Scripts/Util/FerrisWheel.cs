using UnityEngine;
using System.Collections;

public class FerrisWheel : MonoBehaviour {

	Vector3 startPosition;
	public float amplY = 5;
	public float amplZ = 5;
	public float frequence = 0.125f;

	void Start () 
	{
		startPosition = transform.position;
	}
	
	void FixedUpdate () 
	{
		transform.position = getLocalPositionAt (2 * Mathf.PI * Time.fixedTime * frequence);
	}

	void OnDrawGizmos() 
	{
		var markCnt = 12;
		for (var i = 0; i < markCnt; ++i) {
			var dPos = getLocalPositionAt (2 * Mathf.PI * i / markCnt);
			Gizmos.DrawWireSphere (transform.position + dPos, 0.1f);
		}
	}

	Vector3 getLocalPositionAt(float radians) 
	{
		var dy = Mathf.Sin(radians) * amplY;
		var dz = Mathf.Cos(radians) * amplZ;
		return startPosition + transform.forward * dz + new Vector3 (0, dy, 0);
	}
}
