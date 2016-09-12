using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;
using Util;

namespace GameLogic
{
	[SelectionBase]
	public class RepaintRoof : MonoBehaviour 
	{
		public SpaceTrigger trigger;
		public Rainbow roof;
		public AudioClip repaintSfx;

		[HideInInspector]
		public Color repaintColor;

		void Awake () 
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
}