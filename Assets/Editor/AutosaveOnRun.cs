#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[InitializeOnLoad]
public class AutosaveOnRun: ScriptableObject
{
	static AutosaveOnRun()
	{
		EditorApplication.playmodeStateChanged = () =>
		{
			if(EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				Debug.Log("Auto-Saving scene before entering Play mode: " + EditorApplication.currentScene);

				EditorSceneManager.SaveOpenScenes();
				AssetDatabase.SaveAssets();
			}
		};
	}
}
#endif
