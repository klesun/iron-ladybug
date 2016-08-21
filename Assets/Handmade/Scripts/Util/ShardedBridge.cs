using UnityEngine;
using System.Collections;

/** 
 * this script generates sequence of flying 
 * platforms moving in random directions with 
 * configurable distance and max amplietude
 */
public class ShardedBridge : MonoBehaviour 
{
	public Transform endPoint;
	public GameObject shardRef;

	public float spacing = 1;
	public float minAmplietude = 1;
	public float maxAmplietude = 4;
	public float minPeriod = 1;
	public float maxPeriod = 4;

	delegate void DCreateObj (Vector3 pos, Quaternion rot, float amplX, float amplY);

	void Start () 
	{
//		PlaceShards ((pos, rot) => {
//			var shard = UnityEngine.GameObject.Instantiate(shardRef, pos, rot);
//			var ferrisWheel = new FerrisWheel();
//
//		});
	}

	void Update () 
	{
	
	}

	void PlaceShards (DCreateObj makeObj) 
	{
//		Vector3 startPos = transform.position;
//		Vector3 endPos = point.transform.position;
//		int objc = (int)(Vector3.Distance (endPos, startPos) / spacing);
//		for (float i = 0; i < objc; i++) {
//			var drawPos = Vector3.Lerp (startPos, endPos, i / objc);
//			makeObj (drawPos, originalMesh.transform.rotation);
//		}
	}
}
