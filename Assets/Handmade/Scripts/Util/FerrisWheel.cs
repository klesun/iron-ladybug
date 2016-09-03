using UnityEngine;
using System.Collections;

public class FerrisWheel : MonoBehaviour 
{
	public float amplY = 5;
	public float amplZ = 5;
	public float frequence = 0.125f;
	public float initialOffset = 0; // 1 = whole lap

	float startTime = Time.fixedTime;
	Vector3 startPosition;

	/** 
	 * @return float - 0 if wheel is at the start, 
	 * 0.33 if it is at 1/3 of the way and so on
	 */
	public float GetOffset()
	{
		return (initialOffset + (Time.fixedTime - startTime) * frequence) % 1;
	}

	void Start () 
	{
		startPosition = transform.localPosition;
	}
	
	void FixedUpdate () 
	{
		transform.localPosition = getLocalPositionAt (2 * Mathf.PI * GetOffset());
	}

	void OnDrawGizmos() 
	{
		var markCnt = 12;
		for (var i = 0; i < markCnt; ++i) {
			var dPos = getLocalPositionAt (2 * Mathf.PI * i / markCnt);
			Gizmos.DrawWireSphere (transform.localPosition + dPos, 0.1f);
		}
	}

	Vector3 getLocalPositionAt(float radians) 
	{
		var dy = Mathf.Sin(radians) * amplY;
		var dz = Mathf.Cos(radians) * amplZ;
		return startPosition + transform.forward * dz + new Vector3 (0, dy, 0);
	}

	public void SetFrequence(float value)
	{
		initialOffset = GetOffset ();
		startTime = Time.fixedTime;
		frequence = value;
	}
}
