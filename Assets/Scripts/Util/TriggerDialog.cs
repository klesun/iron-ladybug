using UnityEngine;
using System.Collections;
using Util;
using Util.GameLogic;

namespace GameLogic
{
	public class TriggerDialog : MonoBehaviour 
	{
		public Dialogue dialog;
		public SpaceTrigger trigger;

		void Awake () 
		{
			trigger.callback = c => U.If(c.gameObject.GetComponent<HeroControl>() != null, () => dialog.Play());
		}
	}
}
