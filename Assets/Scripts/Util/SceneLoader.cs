using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


namespace Util
{
	/**
	 * this script does {description here}
	 */
	[ExecuteInEditMode]
	public class SceneLoader : MonoBehaviour 
	{
		public List<Object> sceneAssets;
		public ScenePaths scenePaths;

		#if UNITY_EDITOR
		void OnValidate() 
		{
			if (scenePaths != null && sceneAssets != null) {
				scenePaths.scenePaths = sceneAssets
					.Select (a => UnityEditor.AssetDatabase.GetAssetOrScenePath (a))
					.Select (p => System.IO.Path.ChangeExtension (p, null))
					.Select (p => p.Substring ("Assets/".Length))
					.ToList ();
			}
//			sceneAssets
//				.Select (a => AssetDatabase.GetAssetOrScenePath (a)).ToList()
			//				.ForEach (p => UnityEditor.SceneManagement.OpenScene (p));
		}
		#endif
	}
}