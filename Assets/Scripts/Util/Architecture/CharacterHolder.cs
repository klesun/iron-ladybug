using UnityEngine;
using System.Collections;
using Interfaces;

/** 
 * ties hero to this triggerbox, which in his 
 * turn is tied to some sort of visual platform
 */
public class CharacterHolder : MonoBehaviour {
	void OnTriggerEnter(Collider unit) {
		if (unit.gameObject.GetComponent<INpc>() != null) {
			unit.transform.parent = transform.parent;
		}
	}
	void OnTriggerExit(Collider unit) {
		if (unit.gameObject.GetComponent<INpc>() != null) {
			unit.transform.parent = null;
		}
	}
}
