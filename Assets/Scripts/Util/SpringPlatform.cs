using UnityEngine;
using System.Collections;

/**
 * throws any rigidbody that steps on it into 
 * the chosen direction with chosen force
 * should draw with gizmos the trajectory curve
 */
public class SpringPlatform : MonoBehaviour 
{
	public Transform forcePoint;
	public SpaceTrigger trigger;

	public float scale = 4;
	public int markCnt = 36;

	private Vector3 speed;

	// Update is called once per frame
	void Awake () 
	{
		speed = (forcePoint.position - transform.position) * scale; 
		trigger.callback = (c) => {
			foreach (var body in c.gameObject.GetComponents<Rigidbody>()) {
				body.velocity = speed;
			}
		};
	}

	void OnDrawGizmos () 
	{
		var dt = 0.1f;
		for (var i = 0; i < markCnt; ++i) {
			Gizmos.DrawWireCube (PredictPositionAt(dt * i), new Vector3 (1, 1, 1));
		}
	}

	Vector3 PredictPositionAt(float time)
	{
		var speed = (forcePoint.position - transform.position) * scale; 

		return transform.position + speed * time - (Vector3.up * 9.8f * time * time) / 2;
	}
}
