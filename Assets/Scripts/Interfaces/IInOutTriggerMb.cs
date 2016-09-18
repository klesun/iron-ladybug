using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Util;

namespace Interfaces
{
	/** 
	 * anything that ocassionally emmits one of two events
	 */
	abstract public class IInOutTriggerMb : MonoBehaviour, IInOutTrigger
	{
		abstract public void OnIn (D.Cb cb);
		abstract public void OnOut (D.Cb cb);
	}
}
