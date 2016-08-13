using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class EpeeLogic : MonoBehaviour 
{
	void OnTriggerEnter(Collider prey) 
	{
		foreach (var preyScript in prey.gameObject.GetComponents<IPiercable>()) {
			preyScript.pierce ();
		}
	}
}
