using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

public class RepaintRoof : MonoBehaviour 
{
	public SpaceTrigger trigger;
	public Rainbow roof;

	void Start () 
	{
		trigger.callback = PromptToRepaint;
	}

	void PromptToRepaint(Collider collider)
	{
		// TODO: move dropdown implementation to a function that 
		// would take dict of options and lambda to call when selected

		foreach (var hero in collider.GetComponents<HeroControl>()) {
			Tls.inst ().AskForChoice (new Dictionary<string, Color>{
				{"red", Color.red},
				{"green", Color.green},
				{"blue", Color.blue},
				{"cyan", Color.cyan},
				{"magenta", Color.magenta},
				{"yellow", Color.yellow},
			}, (rgb) => roof.color = rgb);
		}
	}
}
