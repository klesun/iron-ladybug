using UnityEngine;
using System.Collections;

public class SpaceTrigger : MonoBehaviour 
{
	public delegate void DCallback (Collider collider);
	public DCallback callback = null;
	public DCallback exitCallback = null;

	void OnTriggerEnter(Collider collider)
	{
		if (callback != null) {
			callback(collider);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (exitCallback != null) {
			exitCallback (collider);
		}
	}
}
