using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

/** 
 * use this to communicate with main thread from any thread
 */
public class Nark : MonoBehaviour 
{
	private Queue<DCallback> callbacks = new Queue<DCallback>();
	
	// Update is called once per frame
	void Update () 
	{
		while (callbacks.Count > 0) {
			callbacks.Dequeue ()();
		}
	}

	public void RunInMainThread(DCallback callback)
	{
		callbacks.Enqueue (callback);
	}
}
