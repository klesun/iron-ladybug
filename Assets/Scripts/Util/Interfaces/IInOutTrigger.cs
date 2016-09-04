using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/** 
 * anything that ocassionally emmits one of two events
 */
abstract public class IInOutTrigger : MonoBehaviour 
{
	abstract public void OnIn (DCallback cb);
	abstract public void OnOut (DCallback cb);
}
