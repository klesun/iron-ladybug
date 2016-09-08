using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

public class RepaintRoof : MonoBehaviour 
{
	public SpaceTrigger trigger;
	public Rainbow roof;
	public AudioClip repaintSfx;

	[HideInInspector]
	public Color repaintColor;

	void Start () 
	{
		trigger.callback = PromptToRepaint;
	}

	void PromptToRepaint(Collider collider)
	{
		foreach (var hero in collider.GetComponents<HeroControl>()) {
			if (repaintColor != null) {
				roof.color = repaintColor;
				Tls.inst ().PlayAudio (repaintSfx);
			}
		}
	}
}
