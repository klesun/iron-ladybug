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
	}
}

