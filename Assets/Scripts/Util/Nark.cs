using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

/** 
 * use this to communicate with main thread from any thread
 */
public class Nark : MonoBehaviour 
{
	private ConcurrentQueue<DCallback> callbacks = new ConcurrentQueue<DCallback>();
	
	void Update () 
	{
		while (callbacks.Count > 0) {
			var cb = callbacks.Dequeue ();
			if (cb != null) {
				cb ();
			}
		}
	}

	public void RunInMainThread(DCallback callback)
	{
		callbacks.Enqueue (callback);
	}
}
