using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Util
{
	/**
	 * please don't change this script fields manually
	 * it is supposed to be filled automagically from SceneLoader.cs
	 */
	public class ScenePaths : MonoBehaviour 
	{
		public List<string> scenePaths;

		void Awake () 
		{
			foreach (var path in scenePaths) {
				SceneManager.LoadScene (path, LoadSceneMode.Additive);
			}
		}
	}
}