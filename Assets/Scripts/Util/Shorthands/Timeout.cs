using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

namespace Util
{
	/** 
	 * use this to communicate with main thread from any thread
	 */
	public class Timeout : MonoBehaviour
	{
		public D.Cb Game(float seconds, D.Cb callback)
		{
			var alreadyPerformed = false;
			D.Cb perform = () => {
				if (!alreadyPerformed) {
					callback();
					alreadyPerformed = true;
				}
			};

			StartCoroutine (WaitAndPerformGame(seconds, perform));

			return perform;
		}

		IEnumerator WaitAndPerformGame(float seconds, D.Cb callback)
		{
			yield return new WaitForSeconds (seconds);
			callback ();
		}

		public D.Cb Real(float seconds, D.Cb callback)
		{
			var alreadyPerformed = false;
			D.Cb perform = () => {
				if (!alreadyPerformed) {
					callback();
					alreadyPerformed = true;
				}
			};

			StartCoroutine (WaitAndPerformReal(seconds, perform));

			return perform;
		}

		IEnumerator WaitAndPerformReal(float seconds, D.Cb callback)
		{
			yield return WaitForRealSeconds(seconds);
			callback ();
		}

		/**
		 * @see http://answers.unity3d.com/questions/301868/yield-waitforseconds-outside-of-timescale.html
		 */
		public static IEnumerator WaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}
	}
}