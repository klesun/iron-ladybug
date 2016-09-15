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
		public void SetTimeout(float seconds, D.Cb callback)
		{
			StartCoroutine (WaitAndPerform(seconds, callback));
		}

		IEnumerator WaitAndPerform(float seconds, D.Cb callback)
		{
			yield return new WaitForSeconds (seconds);
			callback ();
		}
	}
}