using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Linq;

/** 
 * quest about repainting roofs
 */
public class A01MyLife : IQuest
{
	override public bool CheckIsCompleted()
	{
		if (GetRoofs ().TrueForAll (r => r.color == Color.cyan)) {
			return true;
		} else {
			return false;
		}
	}
	
	List<Rainbow> GetRoofs()
	{
		return GetComponentsInChildren<RepaintRoof> ().Select (r => r.roof).ToList ();
	}
}
