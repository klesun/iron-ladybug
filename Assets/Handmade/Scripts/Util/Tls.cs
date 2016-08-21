using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyCSharp
{
	/** singletone for doing stuff */
	public class Tls
	{
		private GameObject  gameObject;
		private static Tls instance;

		private Tls ()
		{
			gameObject = new GameObject ();
		}

		public static Tls inst()
		{
			return instance 
				?? (instance = new Tls());
		}

		/* 
		 * get transform of a fake game object for access to 
		 * methods like "lookAt" to get rotation between two dots 
		*/
		public Transform DullTransform(Vector3 pos)
		{
			gameObject.transform.position = pos;
			gameObject.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
			return gameObject.transform;
		}

		/** 
		 * wrapper to built-in random that (i believe) guarantes 
		 * to return same sequence each time for same seed 
		 * this should be usefull when you want to preserve 
		 * some random combination or allow user to adjust it
		 */
		public static IEnumerable<float> Rand(int seed, int sequenceLength)
		{
			var wasSeed = UnityEngine.Random.seed;
			UnityEngine.Random.seed = seed;

			var result = 
				from i in Enumerable.Range (0, sequenceLength)
				select UnityEngine.Random.value;

			UnityEngine.Random.seed = wasSeed;

			return result;
		}
	}
}

