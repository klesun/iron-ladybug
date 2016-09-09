using UnityEngine;
using System.Collections;
using Util;

public class TriggerDialog : MonoBehaviour 
{
	public Dialogue dialog;
	public SpaceTrigger trigger;

	void Start () 
	{
		trigger.callback = c => U.If(c.gameObject.GetComponent<HeroControl>() != null, () => dialog.Play());
	}
}
