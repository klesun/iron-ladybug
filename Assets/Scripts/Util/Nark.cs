using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

namespace Util
{
	/** 
	 * use this to communicate with main thread from any thread
	 */
	public class Nark : MonoBehaviour 
	{
		private ConcurrentQueue<D.Cb> callbacks = new ConcurrentQueue<D.Cb>();
		
		void Update () 
		{
			while (callbacks.Count > 0) {
				var cb = callbacks.Dequeue ();
				if (cb != null) {
					cb ();
				}
			}
		}

		public void RunInMainThread(D.Cb callback)
		{
			callbacks.Enqueue (callback);
		}
	}
}