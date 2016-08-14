using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour 
{
	public delegate void DCallback (Collider collider);
	public DCallback callback = null;

	void OnTriggerEnter(Collider collider)
	{
		if (callback != null) {
			callback(collider);
		}
	}
}
