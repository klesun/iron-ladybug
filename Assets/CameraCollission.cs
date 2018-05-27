using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollission : MonoBehaviour {
	private Vector3 dir;
	private float distance;
	private float maxDistance = 2.3F;
	private float minDistance = 0.95F;
	
	void Start ()
	{
		dir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}
	
	void Update () {
		if (transform.parent) {
			Vector3 desiredCameraPos = transform.parent.TransformPoint( dir * maxDistance );
		
			RaycastHit hit;
		
			if ( Physics.Linecast( transform.parent.position, desiredCameraPos, out hit ) )
			{
				distance = Mathf.Clamp( hit.distance, minDistance, maxDistance );
			}
			else
			{
				distance = maxDistance;
			}
		
			transform.localPosition=Vector3.Lerp(transform.localPosition, dir * distance, Time.deltaTime * 10.0F);
		}
	}
}
